
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Security.Cryptography;
using Aliyun.OSS.Common;
using UnityEngine;
using System.Collections;
using Anywhere;

namespace Aliyun.OSS
{
    public enum DownLoadState
    {
        STARTDOWNLOAD,
        DOWNLOADING,
        DOWNLOADCOMPLETE
    }

    public static class GetObject
    {
        private static float m_DownLoadProgress;
        private static DownLoadState m_DownLoadState;


        static AutoResetEvent _event = new AutoResetEvent(false);
        public static void SyncGetObject(string bucketName, string key)
        {
            OssClient ossClient = new OssClient(Config.Endpoint, Config.AccessKeyId, Config.AccessKeySecret);
            try
            {

                var result = ossClient.GetObject(bucketName, key);

                using (var requestStream = result.Content)
                {
                    using (var fs = File.Open(Config.DirToDownload + "/" + key, FileMode.OpenOrCreate))
                    {
                        int length = 4 * 1024;
                        var buf = new byte[length];
                        do
                        {
                            length = requestStream.Read(buf, 0, length);
                            fs.Write(buf, 0, length);
                        }
                        while (length != 0);


                    }
                }

                Debug.LogError("Get object succeeded");
            }
            catch (OssException ex)
            {
                Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                    ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with error info: {0}", ex.Message);
            }
        }


        public static void GetObjectByRequest(string bucketName, string key)
        {
            OssClient ossClient = new OssClient(Config.Endpoint, Config.AccessKeyId, Config.AccessKeySecret);
            try
            {

                var request = new GetObjectRequest(bucketName, key);
                request.SetRange(0, 100);

                var result = ossClient.GetObject(request);

                UnityEngine.Debug.LogError("Get object succeeded, length:{0}" + result.Metadata.ContentLength);
            }
            catch (OssException ex)
            {
                Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                    ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with error info: {0}", ex.Message);
            }
        }

        public static void AsyncGetObject(string bucketName, string key)
        {
            OssClient ossClient = new OssClient(Config.Endpoint, Config.AccessKeyId, Config.AccessKeySecret);
            try
            {
                ossClient.BeginGetObject(bucketName, key, GetObjectCallback, key.Clone());

                // _event.WaitOne();
            }
            catch (OssException ex)
            {
                Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                    ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with error info: {0}", ex.Message);
            }

        }

        private static void GetObjectCallback(IAsyncResult ar)
        {
            OssClient ossClient = new OssClient(Config.Endpoint, Config.AccessKeyId, Config.AccessKeySecret);

            try
            {
                var result = ossClient.EndGetObject(ar);
                Debug.LogError("total length:" + result.Metadata.ContentLength);
                int downloadLen = 0;
                using (var requestStream = result.Content)
                {
                    using (var fs = File.Open(Config.DirToDownload + "/" + (ar.AsyncState as string), FileMode.OpenOrCreate))
                    {
                        int length = 4 * 1024;
                        var buf = new byte[length];
                        do
                        {
                            length = requestStream.Read(buf, 0, length);
                            fs.Write(buf, 0, length);
                            downloadLen += length;
                            Debug.LogError("download length:" + downloadLen);
                            m_DownLoadProgress = (float)downloadLen / result.Metadata.ContentLength;
                            m_DownLoadState = DownLoadState.DOWNLOADING;
                        } while (length != 0);
                    }

                }
                m_DownLoadState = DownLoadState.DOWNLOADCOMPLETE;
                Debug.LogError("download done!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _event.Set();
            }
        }


        public static float GetDownLoadProgress()
        {
            return m_DownLoadProgress;
        }

        public static DownLoadState GetDownLoadState()
        {
            return m_DownLoadState;
        }

    }
}

