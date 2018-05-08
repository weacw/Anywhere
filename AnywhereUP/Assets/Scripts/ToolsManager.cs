using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anywhere
{
    public class ToolsManager : MonoBehaviour
    {
        public HttpRequest m_HttpRequest;
        public HttpDownload m_HttpDownload;

        private const string CONFIGPATH = "Configs/Configs";

        private void Awake()
        {
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.HTTP_GETREQUEST, m_HttpRequest.GetHttpResponse);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.HTTP_DOWNLOADFILE, m_HttpDownload.HttpDownloadFile);
        }
    }
}
