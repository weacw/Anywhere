using Aliyun.OSS;
using Aliyun.OSS.Common;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
namespace Anywhere
{
    public class UploadObject
    {
        public static Action OnUploaded;
        public static Action OnUploading;

        public static void UploadFile(string localPath, string buckName, string key)
        {
            bool waitForResult = true;

            if (!File.Exists(localPath))
            {
                Debug.LogError("File path not exist: " + localPath);

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
                             var r = ossClient.EndPutObject(asyncResult);
                         }
                         catch (Exception ex)
                         {
                             Debug.LogError(ex);
                         }
                         finally
                         {
                             if (OnUploaded != null) OnUploaded.Invoke();
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
                    if (OnUploading != null) OnUploading.Invoke();
                }
            }
        }
    }

}
