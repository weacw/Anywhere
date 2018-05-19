﻿using UnityEngine;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Threading;

namespace Anywhere
{
    [CreateAssetMenu(menuName = "Anywhere/AppModules/Http/Http Download")]
    public class HttpDownload : BaseModule
    {

        private object obj;
        public void HttpDownloadFile(Notification _notif)
        {
            if (obj == null) obj = new object();
            lock (obj)
            {
                HttpRequestHelper tmp_httpRequestHelper = _notif.param as HttpRequestHelper;
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                string tmpPath = Path.GetDirectoryName(tmp_httpRequestHelper.m_LocalPath);

                if (!Directory.Exists(tmpPath))
                    Directory.CreateDirectory(tmpPath);

                try
                {
                    string tmpFile = Path.Combine(tmpPath, Path.GetFileName(tmp_httpRequestHelper.m_URI));

                    //本地文件不存在则从网上下载
                    if (!CacheMachine.IsCache(tmpFile))
                    {
                        HttpWebRequest tmpRequest = WebRequest.Create(tmp_httpRequestHelper.m_URI) as HttpWebRequest;
                        tmpRequest.Timeout = tmp_httpRequestHelper.m_TimeOut;
                        HttpWebResponse tmpResponse = tmpRequest.GetResponse() as HttpWebResponse;
                        long tmpTotalBytes = tmpResponse.ContentLength;

                        Stream tmpResponseStream = tmpResponse.GetResponseStream();
                        byte[] tmpBytes = new byte[1024];
                        int size = tmpResponseStream.Read(tmpBytes, 0, (int)tmpBytes.Length);
                        FileStream tmpFileStream = new FileStream(tmpFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                        long tmpCurTotalBytes = 0;
                        while (size > 0)
                        {
                            tmpCurTotalBytes += size;
                            tmpFileStream.Write(tmpBytes, 0, size);
                            size = tmpResponseStream.Read(tmpBytes, 0, (int)tmpBytes.Length);
                            float progress = (float)tmpCurTotalBytes / (float)tmpTotalBytes;
                            if (tmp_httpRequestHelper.m_Downloading != null)
                            {
                                Loom.QueueOnMainThread((parm) =>
                                {
                                    tmp_httpRequestHelper.m_Downloading.Invoke(progress);
                                }, null);
                            }
                        }
                        tmpFileStream.Close();
                        tmpResponseStream.Close();
                    }

                    if (tmp_httpRequestHelper.m_Succeed != null)
                    {
                        Loom.QueueOnMainThread((parm) =>
                        {
                            tmp_httpRequestHelper.m_Succeed.Invoke(null);
                        }, null);
                    }
                }
                catch (WebException ex)
                {
                    if (tmp_httpRequestHelper.m_Failed != null)
                        tmp_httpRequestHelper.m_Failed.Invoke();
                    Debug.LogError(ex.Message);
                }
            }
        }
        private void OnDisable()
        {
            obj = null;
        }
    }
}
