using System.IO;
using System.Net;
using System;
using UnityEngine;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Anywhere
{
    [CreateAssetMenu(menuName = "Anywhere/Http/Http Requset")]
    public class HttpRequest : ScriptableObject
    {
        public void GetHttpResponse(Anywhere.Notification _notif)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            HttpRequestHelper tmp_RequestHelper = _notif.param as HttpRequestHelper;
            if (string.IsNullOrEmpty(tmp_RequestHelper.m_URI))
                throw new ArgumentNullException("url");
            try
            {
                HttpWebRequest request = WebRequest.Create(tmp_RequestHelper.m_URI) as HttpWebRequest;
                request.Method = "GET";
                if (tmp_RequestHelper.m_TimeOut != 0)
                    request.Timeout = tmp_RequestHelper.m_TimeOut;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                StreamReader tmp_Reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
                Loom.QueueOnMainThread((parm) =>
                {
                    if (tmp_RequestHelper.m_Succeed != null)
                        tmp_RequestHelper.m_Succeed.Invoke(tmp_Reader.ReadToEnd());
                }, null);
            }
            catch (WebException ex)
            {
                Debug.LogError(ex.Message);
                switch ((ex.Response as HttpWebResponse).StatusCode)
                {
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.GatewayTimeout:
                        if (tmp_RequestHelper.m_Failed != null)
                            tmp_RequestHelper.m_Failed.Invoke();
                        break;
                }
            }

        }
    }
}