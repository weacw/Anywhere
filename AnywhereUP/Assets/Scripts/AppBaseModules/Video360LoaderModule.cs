using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

namespace Anywhere
{
    [CreateAssetMenu(menuName = "Anywhere/AppModules/Video360LoaderModule")]
    public class Video360LoaderModule : BaseModule
    {
        public GameObject m_Video360Prefab;
        /// <summary>
        ///播放路径下的视频； 
        /// </summary>
        /// <param name="_videoimage">播放视频到指定rawimage.</param>
        /// <param name="_videopath">视频名称.</param>
        /// <param name="_isloop">视频是否循环.</param>
        internal void PlayVideo(Notification _notif)
        {
            VideoPlayerHelper tmp_videoPlayerHelper = _notif.param as VideoPlayerHelper;
            if (tmp_videoPlayerHelper.m_BeginInstance != null) tmp_videoPlayerHelper.m_BeginInstance.Invoke();
            GameObject tmp_Sphere = Instantiate<GameObject>(m_Video360Prefab);
            tmp_Sphere.name = "TMP_VIDEOPLAYER";
            Transform tmp_Transform = tmp_Sphere.transform;
            tmp_Transform.position = Vector3.zero;
            tmp_Transform.localScale = Vector3.one * 2;
            tmp_Transform.rotation = Quaternion.identity;
            tmp_Sphere.SetActive(false);

            NormalCameraSync.Instance.m_SyncPosition = false;

            GameObject tmp_Renderer = tmp_Sphere.transform.Find("Renderer").gameObject;


            VideoPlayer tmp_VideoPlayer = tmp_Renderer.AddComponent<VideoPlayer>();
            tmp_VideoPlayer.playOnAwake = false;
            tmp_VideoPlayer.source = VideoSource.Url;
            tmp_VideoPlayer.url = Path.Combine(Configs.GetConfigs.m_CachePath, tmp_videoPlayerHelper.m_Videoname + ".mp4");
            tmp_VideoPlayer.renderMode = VideoRenderMode.MaterialOverride;
            tmp_VideoPlayer.targetMaterialRenderer = tmp_Renderer.GetComponent<Renderer>();
            tmp_VideoPlayer.isLooping = true;
            tmp_VideoPlayer.Play();
            tmp_VideoPlayer.Pause();

            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.ASSETS_SETUP, new ContentSetupHelper() { m_Content = tmp_Sphere });
            if (tmp_videoPlayerHelper.m_EndIntance != null) tmp_videoPlayerHelper.m_EndIntance.Invoke();
        }
    }
}