﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Anywhere
{
    [CreateAssetMenu(menuName = "Anywhere/AppModules/AssetbundleLoaderModule")]
    public class AssetbundleLoaderModule : BaseModule
    {
        /// <summary>
        ///_AssetName=null或空时，实例化该AB包所有资源
        /// </summary>
        /// <returns>The A.</returns>
        /// <param name="_ABname">A bname.</param>
        /// <param name="_AssetName">Asset name.</param>
        public void InstaniateAB(Notification _notif)
        {
            ABInstaniateHelper abhelper = _notif.param as ABInstaniateHelper;
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
            if (m_AssetBundle.isStreamedSceneAssetBundle)
            {
                string[] scenePaths = m_AssetBundle.GetAllScenePaths();
                string sceneName = Path.GetFileNameWithoutExtension(scenePaths[0]);
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                SceneManager.sceneLoaded += (name, mode) =>
                {
                    AssetsManager.Instance.m_Content = GameObject.Find("ContentRoot");
                    loaded = true;
                };
                while (!loaded)
                    yield return null;

            }
            else
            {
                AssetBundleRequest r = m_AssetBundle.LoadAllAssetsAsync<GameObject>();
                if (r == null) yield return null; ;
                AssetsManager.Instance.m_Content = Instantiate(r.asset as GameObject);
            }

            AssetsManager.Instance.m_Content.SetActive(false);
            m_AssetBundle.Unload(false);

        }
    }
}
