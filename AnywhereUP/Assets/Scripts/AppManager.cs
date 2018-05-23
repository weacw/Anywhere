
using System.Collections;
using System.Collections.Generic;
using Anywhere.UI;
using UnityEngine;

namespace Anywhere
{

    public class AppManager : Singleton<AppManager>
    {
        public List<BaseModule> m_ScriptModules = new List<BaseModule>();
        private Dictionary<string, BaseModule> m_ScriptModulesDict = new Dictionary<string, BaseModule>();

        private UIManager m_UIManager;
        private AssetsManager m_AssetsManager;
        private LoopListViewHelper m_LoopListViewHelper;
        private DataSource m_DataSource;
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
            m_AssetsManager = FindObjectOfType<AssetsManager>();
            m_LoopListViewHelper = FindObjectOfType<LoopListViewHelper>();
            m_DataSource = FindObjectOfType<DataSource>();

            //Register operater
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.HTTP_GETREQUEST, (m_ScriptModulesDict["HttpWebRequest"] as HttpRequest).GetHttpResponse);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.HTTP_DOWNLOADFILE, (m_ScriptModulesDict["HttpDownload"] as HttpDownload).HttpDownloadFile);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.HTTP_GETPAGEDATAS, (m_ScriptModulesDict["GetDatasFromServerModule"] as GetDatasFromServerModule).GetDatasFromServer);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.HTTP_GETPAGEITEM, m_DataSource.GetPageItem);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.HTTP_GETALLPAGEINFO, m_LoopListViewHelper.CreatPages);

            NotifCenter.GetNotice.AddEventListener(NotifEventKey.SEARCH_GETRESULT, (m_ScriptModulesDict["SearchModule"] as SearchModule).Search);

            NotifCenter.GetNotice.AddEventListener(NotifEventKey.SOCIAL_SHARE, (m_ScriptModulesDict["NativeBridge"] as NativeBridge).Share);

            NotifCenter.GetNotice.AddEventListener(NotifEventKey.EVERYPLAY_RECORDING_START, (m_ScriptModulesDict["EveryplayModule"] as EveryplayModule).OnRecordingStart);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.EVERYPLAY_RECORDING_STOP, (m_ScriptModulesDict["EveryplayModule"] as EveryplayModule).OnRecordingStop);

            NotifCenter.GetNotice.AddEventListener(NotifEventKey.ASSETS_ABINSTANCE, (m_ScriptModulesDict["AssetbundleLoaderModule"] as AssetbundleLoaderModule).InstaniateAB);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.ASSETS_VIDEOPLAY, (m_ScriptModulesDict["Video360LoaderModule"] as Video360LoaderModule).PlayVideo);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.ASSETS_IMAGE360, (m_ScriptModulesDict["Image360LoaderModule"] as Image360LoaderModule).PlayImage360);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.ASSETS_SETUP, m_AssetsManager.SetupContent);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.ASSETS_REMOVEALL, m_AssetsManager.RemoveABSource);

            NotifCenter.GetNotice.AddEventListener(NotifEventKey.OPERATER_PLACECONTENT, m_AssetsManager.PlaceContent);

            NotifCenter.GetNotice.AddEventListener(NotifEventKey.UI_SHOWHIDELOADING, m_UIManager.ShowHideLoading);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.UI_SHOWCALLBTN, m_UIManager.ShowHideCallBtn);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.UI_HIDECALLBTN, m_UIManager.ShowHideCallBtn);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.UI_GOTOARSCENE, m_UIManager.GoToARScene);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.UI_REFRESHDATAS, m_LoopListViewHelper.RefreshDatas);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.UI_SETHINTSTATES, m_UIManager.SetHintStates);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.UI_CREATETALK, (m_ScriptModulesDict["TutorialModule"] as TutorialModule).CreateTalk);

            //(m_ScriptModulesDict["TutorialModule"] as TutorialModule).m_WasNoFirstTimeCallBack = () => { StartCoroutine(GetDatas()); };
            // NotifCenter.GetNotice.AddEventListener(NotifEventKey.UI_SHOWCACHESIZE, m_UIManager.ShowCacheSize);
        }
        private void Start()
        {
            //NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.UI_CREATETALK);
            StartCoroutine(GetDatas());
        }
        private IEnumerator GetDatas()
        {
            yield return new WaitForSeconds(0.25f);
            HttpGetDataHelper tmp_HttpGetDataHelper = new HttpGetDataHelper();
            tmp_HttpGetDataHelper.m_Finished = null;
            tmp_HttpGetDataHelper.m_PageIndex = Configs.GetConfigs.ContentPageNum;
            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.HTTP_GETPAGEDATAS, tmp_HttpGetDataHelper);
            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.UI_SHOWHIDELOADING, new UICtrlHelper() { m_State = true });
        }

        private void OnDisable()
        {
            if (m_ScriptModulesDict.Count <= 0 || m_ScriptModulesDict == null)
                m_ScriptModulesDict.Clear();
        }
    }
}