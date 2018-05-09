using Anywhere.Net;
using UnityEngine;

namespace Anywhere
{
    [CreateAssetMenu(menuName = "Anywhere/Http/ThumbnialDownloader")]
    public class GetDatasFromServerModule : BaseModule
    {
        private HttpGetDataHelper m_HttpGetDataHelper;
        public void GetDatasFromServer(Notification _notif)
        {
            if (m_HttpGetDataHelper == null)
                m_HttpGetDataHelper = _notif.param as HttpGetDataHelper;

            HttpRequestHelper helper = new HttpRequestHelper()
            {
                m_URI = Configs.GetConfigs.m_GetInfoHost + m_HttpGetDataHelper.m_PageIndex,
                m_Succeed = (json) =>
                {
                    HttpSaveDataHelper tmp_SaveDataHelper = new HttpSaveDataHelper();
                    tmp_SaveDataHelper.m_PageItemArray = (JsonHelper.FromJson<PageItem>(json));
                    NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.HTTP_GETPAGEITEM, tmp_SaveDataHelper);
                    m_HttpGetDataHelper.m_PageIndex++;
                },
                m_TimeOut = 30000
            };
            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.HTTP_GETREQUEST, helper);
        }

        private void OnDisable()
        {
            m_HttpGetDataHelper = null;
        }

    }
}