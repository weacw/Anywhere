using UnityEngine;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Anywhere
{
    [CreateAssetMenu(menuName = "Anywhere/Http/Http Download")]
    public class HttpDownload : ScriptableObject
    {
        public void HttpDownloadFile(Notification _notif)
        {
            HttpRequestHelper tmp_httpRequestHelper = _notif.param as HttpRequestHelper;
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            string tmpExtension = Path.GetExtension(tmp_httpRequestHelper.m_URI);
            string tmpPath = Path.GetDirectoryName(tmp_httpRequestHelper.m_LocalPath) + @"\tmp";
            Directory.CreateDirectory(tmpPath);
            string tmpFile = tmpPath + @"\" + Path.GetFileName(tmp_httpRequestHelper.m_LocalPath) + tmpExtension;
            if (File.Exists(tmpPath))
                File.Delete(tmpPath);
            try
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

                    //汇入主线程
                    Loom.QueueOnMainThread((parm) =>
                    {
                        if (tmp_httpRequestHelper.m_Downloading != null)
                            tmp_httpRequestHelper.m_Downloading.Invoke(progress);
                    }, null);
                }
                tmpFileStream.Close();
                tmpResponseStream.Close();
                if (tmp_httpRequestHelper.m_Succeed != null)
                    tmp_httpRequestHelper.m_Succeed.Invoke(null);
            }
            catch (WebException ex)
            {
                if (tmp_httpRequestHelper.m_Failed != null)
                    tmp_httpRequestHelper.m_Failed.Invoke();
                Debug.LogError(ex.Message);
            }
        }
    }
}
