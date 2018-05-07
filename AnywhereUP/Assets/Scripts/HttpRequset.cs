using System.IO;
using System.Net;
using System;
public class HttpRequset
{
    public void GetHttpResponse(Anywhere.Notification _notif)
    {
        HttpRequestHelper tmp_RequestHelper = _notif.param as HttpRequestHelper;
        if (string.IsNullOrEmpty(tmp_RequestHelper.m_URI))
            throw new ArgumentNullException("url");
        HttpWebRequest request = WebRequest.Create(tmp_RequestHelper.m_URI) as HttpWebRequest;
        request.Method = "GET";
        if (tmp_RequestHelper.m_TimeOut != 0)
            request.Timeout = tmp_RequestHelper.m_TimeOut;
        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
        using (StreamReader tmp_Reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
        {
            UnityEngine.Debug.Log(tmp_Reader.ReadToEnd());
        }
    }
}
