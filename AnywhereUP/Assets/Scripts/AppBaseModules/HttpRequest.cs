//用于向服务器发起http-get请求模块
namespace Anywhere
{
    using System.IO;
    using System.Net;
    using UnityEngine;
    using System.Security.Cryptography.X509Certificates;
    using System.Net.Security;
    using System.Threading;

    [CreateAssetMenu(menuName = "Anywhere/AppModules/Http/Http Requset")]
    public class HttpRequest : BaseModule
    {
        public void GetHttpResponse(Anywhere.Notification _notif)
        {
            //证书验证
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            //获取NoticeCenter发送过来的消息参数
            HttpRequestHelper tmp_RequestHelper = _notif.param as HttpRequestHelper;

            //划出另外一个线程
            Loom.RunAsync(() => new Thread(() =>
            {
                try
                {
                    //开始向服务器发送请求的回调
                    if (tmp_RequestHelper.m_Starting != null) tmp_RequestHelper.m_Starting.Invoke();

                    //正式向服务器发起http-get请求
                    HttpWebRequest request = WebRequest.Create(tmp_RequestHelper.m_URI) as HttpWebRequest;
                    request.Method = "GET";
                    if (tmp_RequestHelper.m_TimeOut != 0)
                        request.Timeout = tmp_RequestHelper.m_TimeOut;
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    //获取服务器反馈回来的消息
                    StreamReader tmp_Reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);

                    //由于不是主线程无法执行UnityEngine-API，所以需要将当前线程划入主线程
                    Loom.QueueOnMainThread((parm) =>
                    {
                        //执行请求成功后的回调
                        if (tmp_RequestHelper.m_Succeed != null)
                            tmp_RequestHelper.m_Succeed.Invoke(tmp_Reader.ReadToEnd());
                    }, null);
                }
                catch (WebException ex)
                {
                    //请求错误
                    Debug.LogError(ex.Message);
                    switch ((ex.Response as HttpWebResponse).StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                        case HttpStatusCode.GatewayTimeout:
                            Loom.QueueOnMainThread((parm) =>
                            {
                                if (tmp_RequestHelper.m_Failed != null)
                                    tmp_RequestHelper.m_Failed.Invoke();
                            }, null);
                            break;
                    }
                }
            }).Start());
        }
    }
}