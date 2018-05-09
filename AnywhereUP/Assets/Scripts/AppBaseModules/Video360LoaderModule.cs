using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Anywhere
{
    [CreateAssetMenu(menuName = "Anywhere/AppModules/Video360LoaderModule")]
    public class Video360LoaderModule : BaseModule
    {
        /// <summary>
        ///播放路径下的视频； 
        /// </summary>
        /// <param name="_videoimage">播放视频到指定rawimage.</param>
        /// <param name="_videopath">视频名称.</param>
        /// <param name="_isloop">视频是否循环.</param>
        private void PlayVideo(Notification _notif)
        {
            VideoPlayerHelper tmp_videoPlayerHelper = _notif.param as VideoPlayerHelper;
            VideoPlayer m_Vp = tmp_videoPlayerHelper.m_Videorender.GetComponent<VideoPlayer>();
            if (m_Vp != null)
            {
                if (m_Vp.isPlaying)
                    m_Vp.Stop();
            }
            else
                m_Vp = tmp_videoPlayerHelper.m_Videorender.AddComponent<VideoPlayer>();
            m_Vp.source = VideoSource.Url;
            m_Vp.url = "file:///" + Configs.GetConfigs.m_CachePath + "/Video/" + tmp_videoPlayerHelper.m_Videoname + ".mp4";
            m_Vp.renderMode = VideoRenderMode.MaterialOverride;
            m_Vp.targetMaterialRenderer = tmp_videoPlayerHelper.m_Videorender.GetComponent<Renderer>();
            tmp_videoPlayerHelper.m_Videorender.GetComponent<Renderer>().material.shader = Shader.Find("Custom/Video360");
            m_Vp.Play();
            m_Vp.isLooping = tmp_videoPlayerHelper.m_Isloop;
        }
    }
}