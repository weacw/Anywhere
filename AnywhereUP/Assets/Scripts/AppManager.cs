
using System.Collections.Generic;


namespace Anywhere
{
    public class AppManager : Singleton<AppManager>
    {
        public List<BaseModule> m_ScriptModules = new List<BaseModule>();

        private Dictionary<string, BaseModule> m_ScriptModulesDict = new Dictionary<string, BaseModule>();
        public override void Awake()
        {

            foreach (var module in m_ScriptModules)
            {
                if (!m_ScriptModulesDict.ContainsKey(module.name))
                    m_ScriptModulesDict.Add(module.name, module);
            }


            NotifCenter.GetNotice.AddEventListener(NotifEventKey.HTTP_GETREQUEST, (m_ScriptModulesDict["HttpWebRequest"] as HttpRequest).GetHttpResponse);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.HTTP_DOWNLOADFILE, (m_ScriptModulesDict["HttpDownload"] as HttpDownload).HttpDownloadFile);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.SEARCH_GETRESULT, (m_ScriptModulesDict["SearchModule"] as SearchModule).Search);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.HTTP_GETPAGEDATAS, (m_ScriptModulesDict["GetDatasFromServerModule"] as GetDatasFromServerModule).GetDatasFromServer);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.SOCIAL_SHARE, (m_ScriptModulesDict["NativeBridge"] as NativeBridge).Share);        
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.EVERYPLAY_RECORDING_START, (m_ScriptModulesDict["EveryplayModule"] as EveryplayModule).OnRecordingStart);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.EVERYPLAY_RECORDING_STOP, (m_ScriptModulesDict["EveryplayModule"] as EveryplayModule).OnRecordingStop);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.ASSETS_ABINSTANCE, (m_ScriptModulesDict["AssetbundleLoaderModule"] as AssetbundleLoaderModule).InstaniateAB);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.ASSETS_VIDEOPLAY, (m_ScriptModulesDict["EveryplayModule"] as EveryplayModule).OnRecordingStop);            

        }


        private void OnDisable()
        {
            if (m_ScriptModulesDict.Count <= 0 || m_ScriptModulesDict == null)
                m_ScriptModulesDict.Clear();
        }
    }
}