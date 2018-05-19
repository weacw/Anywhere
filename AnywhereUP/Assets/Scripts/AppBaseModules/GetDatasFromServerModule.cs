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
            m_HttpGetDataHelper = _notif.param as HttpGetDataHelper;
            HttpRequestHelper helper = new HttpRequestHelper()
            {
                m_URI = Configs.GetConfigs.m_GetInfoHost + m_HttpGetDataHelper.m_PageIndex,
                m_Starting = () => NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.UI_SHOWHIDELOADING, new UICtrlHelper() { m_State = true }),
                m_Succeed = (json) =>
                {
                    HttpSaveDataHelper tmp_SaveDataHelper = new HttpSaveDataHelper();
                    tmp_SaveDataHelper.m_PageItemArray = (JsonHelper.FromJson<PageItem>(json));
                    NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.HTTP_GETPAGEITEM, tmp_SaveDataHelper);
                    if (m_HttpGetDataHelper.m_Finished != null) m_HttpGetDataHelper.m_Finished.Invoke();
                    NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.UI_SHOWHIDELOADING, new UICtrlHelper() { m_State = false });
                    Configs.GetConfigs.ContentPageNum++;
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