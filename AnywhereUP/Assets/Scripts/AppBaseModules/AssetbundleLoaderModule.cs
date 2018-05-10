using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Anywhere
{
    [CreateAssetMenu(menuName = "Anywhere/AppModules/AssetbundleLoaderModule")]
    public class AssetbundleLoaderModule : BaseModule
    {

        private System.Action m_BeginAction;
        private System.Action m_EndAction;
        private GameObject tmp_Content;
        private ContentSetupHelper m_ContentSetupHelper;
        private void OnDisable()
        {
            tmp_Content = null;
            m_ContentSetupHelper = null;
        }

        /// <summary>
        ///_AssetName=null或空时，实例化该AB包所有资源
        /// </summary>
        /// <returns>The A.</returns>
        /// <param name="_ABname">A bname.</param>
        /// <param name="_AssetName">Asset name.</param>
        public void InstaniateAB(Notification _notif)
        {
            ABInstaniateHelper abhelper = _notif.param as ABInstaniateHelper;
            m_BeginAction = abhelper.m_BeginInstance;
            m_EndAction = abhelper.m_EndIntance;
            if (m_BeginAction != null) m_BeginAction.Invoke();
            AppManager.Instance.StartCoroutine(SyncLoadABFromFile(abhelper.m_ABName));
        }

        private IEnumerator SyncLoadABFromFile(string _name)
        {
            yield return null;
            AssetBundleCreateRequest abcr = null;
            abcr = AssetBundle.LoadFromFileAsync(Path.Combine(Configs.GetConfigs.m_CachePath, _name + ".assetbundle"));
            abcr.completed += (x) =>
            {
                AppManager.Instance.StartCoroutine(SyncABLoad(abcr));
            };
        }
        private IEnumerator SyncABLoad(AssetBundleCreateRequest _r)
        {
            bool loaded = false;
            yield return null;
            AssetBundle m_AssetBundle = _r.assetBundle;
            if (m_ContentSetupHelper == null) m_ContentSetupHelper = new ContentSetupHelper();
            m_ContentSetupHelper.m_IsSceneStream = m_AssetBundle.isStreamedSceneAssetBundle;
            if (m_ContentSetupHelper.m_IsSceneStream)
            {
                string[] scenePaths = m_AssetBundle.GetAllScenePaths();
                string sceneName = Path.GetFileNameWithoutExtension(scenePaths[0]);
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                SceneManager.sceneLoaded += (name, mode) =>
                {
                    tmp_Content = GameObject.Find("ContentRoot");
                    m_ContentSetupHelper.m_Content = tmp_Content;
                    m_ContentSetupHelper.m_Scene = name;
                    NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.ASSETS_SETUP, m_ContentSetupHelper);
                    loaded = true;
                };
                while (!loaded)
                    yield return null;

            }
            else
            {
                AssetBundleRequest r = m_AssetBundle.LoadAllAssetsAsync<GameObject>();
                if (r == null) yield return null;
                GameObject tmp_Content = Instantiate(r.asset as GameObject);
                m_ContentSetupHelper.m_Content = tmp_Content;
                NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.ASSETS_SETUP, m_ContentSetupHelper);
            }
            tmp_Content.SetActive(false);
            m_AssetBundle.Unload(false);
            if (m_EndAction != null) m_EndAction.Invoke();

        }
    }
}
