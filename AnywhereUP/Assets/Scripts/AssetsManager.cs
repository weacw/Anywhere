using UnityEngine;
using UnityEngine.Video;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

namespace Anywhere
{
    /// <summary>
    ///加载视频,图片，AB资源类; 
    /// </summary>
    public class AssetsManager : Singleton<AssetsManager>
    {
        public GameObject m_Content;
        private bool m_IsSceneStream;
        private Scene m_Scenes;

        internal void PlaceContent(Notification _notif)
        {
            ContentPlaceHelper cph = _notif.param as ContentPlaceHelper;
            m_Content.transform.position = cph.m_ContentPos;
            m_Content.transform.rotation = cph.m_ContentRot;
            m_Content.SetActive(true);
        }

        /// <summary>
        /// 移除当前所有场景内AB资源
        /// </summary>
        internal void RemoveABSource(Notification _notif)
        {
            if (m_Content == null) return;
            m_Content.SetActive(false);
            DestroyImmediate(m_Content);
            m_Content = null;
            if (m_IsSceneStream)
                SceneManager.UnloadSceneAsync(m_Scenes);
            Resources.UnloadUnusedAssets();

        }

        /// <summary>
        /// 设置实例化出来的资源寄存，以便管理
        /// </summary>
        /// <param name="_notif"></param>
        internal void SetupContent(Notification _notif)
        {
            ContentSetupHelper tmp_ContentSetupHelper = _notif.param as ContentSetupHelper;
            m_IsSceneStream = tmp_ContentSetupHelper.m_IsSceneStream;
            m_Content = tmp_ContentSetupHelper.m_Content;
            m_Scenes = tmp_ContentSetupHelper.m_Scene;
        }
    }
}