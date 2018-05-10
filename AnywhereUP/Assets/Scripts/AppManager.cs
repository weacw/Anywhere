
using System.Collections.Generic;
using Anywhere.UI;

namespace Anywhere
{

    public class AppManager : Singleton<AppManager>
    {
        public List<BaseModule> m_ScriptModules = new List<BaseModule>();
        private Dictionary<string, BaseModule> m_ScriptModulesDict = new Dictionary<string, BaseModule>();

        private UIManager m_UIManager;

        /// <summary>
        /// Init the configs
        /// </summary>
        public override void Awake()
        {

            foreach (var module in m_ScriptModules)
            {
                if (!m_ScriptModulesDict.ContainsKey(module.name))
                    m_ScriptModulesDict.Add(module.name, module);
            }

            //Get maanger
            m_UIManager = FindObjectOfType<UIManager>();



            //Register operater
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.HTTP_GETREQUEST, (m_ScriptModulesDict["HttpWebRequest"] as HttpRequest).GetHttpResponse);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.HTTP_DOWNLOADFILE, (m_ScriptModulesDict["HttpDownload"] as HttpDownload).HttpDownloadFile);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.HTTP_GETPAGEDATAS, (m_ScriptModulesDict["GetDatasFromServerModule"] as GetDatasFromServerModule).GetDatasFromServer);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.SEARCH_GETRESULT, (m_ScriptModulesDict["SearchModule"] as SearchModule).Search);

            NotifCenter.GetNotice.AddEventListener(NotifEventKey.SOCIAL_SHARE, (m_ScriptModulesDict["NativeBridge"] as NativeBridge).Share);

            NotifCenter.GetNotice.AddEventListener(NotifEventKey.EVERYPLAY_RECORDING_START, (m_ScriptModulesDict["EveryplayModule"] as EveryplayModule).OnRecordingStart);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.EVERYPLAY_RECORDING_STOP, (m_ScriptModulesDict["EveryplayModule"] as EveryplayModule).OnRecordingStop);

            NotifCenter.GetNotice.AddEventListener(NotifEventKey.ASSETS_ABINSTANCE, (m_ScriptModulesDict["AssetbundleLoaderModule"] as AssetbundleLoaderModule).InstaniateAB);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.ASSETS_VIDEOPLAY, (m_ScriptModulesDict["EveryplayModule"] as EveryplayModule).OnRecordingStop);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.ASSETS_SETUP, AssetsManager.Instance.SetupContent);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.ASSETS_REMOVEALL, AssetsManager.Instance.RemoveABSource);

            NotifCenter.GetNotice.AddEventListener(NotifEventKey.OPERATER_PLACECONTENT, AssetsManager.Instance.PlaceContent);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.NET_GETALLPAGEINFO, LoopListViewHelper.Instance.CreatPages);

            NotifCenter.GetNotice.AddEventListener(NotifEventKey.UI_SHOWHIDELOADING, m_UIManager.ShowHideLoading);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.UI_SHOWCALLBTN, m_UIManager.ShowHideCallBtn);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.UI_HIDECALLBTN, m_UIManager.ShowHideCallBtn);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.UI_GOTOARSCENE, m_UIManager.GoToARScene);
        }


        private void OnDisable()
        {
            if (m_ScriptModulesDict.Count <= 0 || m_ScriptModulesDict == null)
                m_ScriptModulesDict.Clear();
        }
    }
}