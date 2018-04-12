using Aliyun.OSS;
using Aliyun.OSS.Common;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
namespace Aliyun.OSS
{
    public class UploadObject
    {

        IEnumerator UploadFile(string localPath, string buckName, string key)
        {
            bool waitForResult = true;

            if (!File.Exists(localPath))
            {
                Debug.LogError("File path not exist: " + localPath);
                yield break;
            }

            using (var fs = File.Open(localPath, FileMode.Open))
            {
                OssClient ossClient = new OssClient(Config.Endpoint, Config.AccessKeyId, Config.AccessKeySecret);
                try
                {
                    var metadata = new ObjectMetadata();
                    metadata.ContentLength = fs.Length;
                    metadata.CacheControl = "public";

                    ossClient.BeginPutObject(buckName, key, fs, metadata, (asyncResult) =>
                    {
                        try
                        {
                            ossClient.EndPutObject(asyncResult);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError(ex);
                        }
                        finally
                        {
                            waitForResult = false;
                        }
                    }, null);
                }
                catch (OssException ex)
                {
                    Debug.LogError(string.Format("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                        ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId));
                    waitForResult = false;
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                    waitForResult = false;
                }

                while (waitForResult)
                {
                    // Debug.LogError(met);
                    yield return null;
                }
            }
        }
    }

}
