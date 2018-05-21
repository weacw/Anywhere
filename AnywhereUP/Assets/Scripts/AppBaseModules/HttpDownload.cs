//用于多线程http文件下载

namespace Anywhere
{
    using UnityEngine;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Net.Security;

    [CreateAssetMenu(menuName = "Anywhere/AppModules/Http/Http Download")]
    public class HttpDownload : BaseModule
    {
        //线程锁-锁头，可以保证当有线程操作某个共享资源时，其他线程必须等待直到当前线程完成操作
        private object m_LockObject;

        public void HttpDownloadFile(Notification _notif)
        {
            if (m_LockObject == null) m_LockObject = new object();
            lock (m_LockObject)
            {
                //获取NoticeCenter发来的消息参数
                HttpRequestHelper tmp_httpRequestHelper = _notif.param as HttpRequestHelper;

                //证书
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                string tmpPath = Path.GetDirectoryName(tmp_httpRequestHelper.m_LocalPath);

                //创建文件夹
                if (!Directory.Exists(tmpPath))
                    Directory.CreateDirectory(tmpPath);

                try
                {
                    string tmpFile = Path.Combine(tmpPath, Path.GetFileName(tmp_httpRequestHelper.m_URI));

                    //检查缓存,缓存不存在则从网上下载
                    if (!CacheMachine.IsCache(tmpFile))
                    {
                        //正式发起http请求
                        HttpWebRequest tmpRequest = WebRequest.Create(tmp_httpRequestHelper.m_URI) as HttpWebRequest;
                        tmpRequest.Timeout = tmp_httpRequestHelper.m_TimeOut;
                        HttpWebResponse tmpResponse = tmpRequest.GetResponse() as HttpWebResponse;
                        long tmpTotalBytes = tmpResponse.ContentLength;

                        //获取服务器返回的数据，并写入本地指定目录(tmpFile目录)
                        Stream tmpResponseStream = tmpResponse.GetResponseStream();
                        byte[] tmpBytes = new byte[1024];
                        int size = tmpResponseStream.Read(tmpBytes, 0, (int)tmpBytes.Length);
                        FileStream tmpFileStream = new FileStream(tmpFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

                        //计算http下载文件的进度
                        long tmpCurTotalBytes = 0;
                        while (size > 0)
                        {
                            tmpCurTotalBytes += size;
                            tmpFileStream.Write(tmpBytes, 0, size);
                            size = tmpResponseStream.Read(tmpBytes, 0, (int)tmpBytes.Length);
                            float progress = (float)tmpCurTotalBytes / (float)tmpTotalBytes;

                            //下载中的回调
                            if (tmp_httpRequestHelper.m_Downloading != null)
                            {
                                //划入主线程的回调
                                Loom.QueueOnMainThread((parm) => { tmp_httpRequestHelper.m_Downloading.Invoke(progress); }, null);
                            }
                        }
                        tmpFileStream.Close();
                        tmpResponseStream.Close();
                    }

                    //下载完毕的回调
                    if (tmp_httpRequestHelper.m_Succeed != null)
                    {
                        Loom.QueueOnMainThread((parm) => { tmp_httpRequestHelper.m_Succeed.Invoke(null); }, null);
                    }
                }
                catch (WebException ex)
                {
                    //下载出错
                    if (tmp_httpRequestHelper.m_Failed != null)
                        tmp_httpRequestHelper.m_Failed.Invoke();
                    Debug.LogError(ex.Message);
                }
            }
        }

        private void OnDisable()
        {
            //由于script object table 是可存储的，一旦设置变量后就会被保存下来。固在disable后把线程锁清空
            m_LockObject = null;
        }
    }
}
