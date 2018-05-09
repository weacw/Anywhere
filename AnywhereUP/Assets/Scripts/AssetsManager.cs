using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;
using Aliyun.OSS;
using System.IO;
using System.Collections;
using UnityEngine.XR.iOS;
using UnityEngine.SceneManagement;

namespace Anywhere
{
    /// <summary>
    ///加载视频,图片，AB资源类; 
    /// </summary>
    public class AssetsManager : Singleton<AssetsManager>
    {
        public GameObject m_Content;
        public override void Awake()
        {
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.ASSETS_REMOVEALL, RemoveABSource);
                        NotifCenter.GetNotice.AddEventListener(NotifEventKey.ASSETS_VIDEOPLAY, PlayVideo);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.OPERATER_PLACECONTENT, PlaceContent);
        }


        private void PlaceContent(Notification _notif)
        {
            ContentPlaceHelper cph = _notif.param as ContentPlaceHelper;
            m_Content.transform.position = cph.m_ContentPos;
            m_Content.transform.rotation = cph.m_ContentRot;
            m_Content.SetActive(true);
        }


       
        private Texture LoadTexture(string _texturename, int _t2dwith, int _t2dheight)
        {

            byte[] m_T2dbyts = File.ReadAllBytes(Configs.GetConfigs.m_CachePath+"/Texture/" + _texturename + ".png");
            Texture2D m_T2d = new Texture2D(_t2dwith, _t2dheight);
            m_T2d.LoadImage(m_T2dbyts);
            return m_T2d;
        }

        /// <summary>
        /// 移除当前所有场景内AB资源
        /// </summary>
        private void RemoveABSource(Notification _notif)
        {
            if (m_Content == null) return;
            m_Content.SetActive(false);
            DestroyImmediate(m_Content);
        }

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
            m_Vp.url = "file:///" + Configs.GetConfigs.m_CachePath+"/Video/" + tmp_videoPlayerHelper.m_Videoname + ".mp4";
            m_Vp.renderMode = VideoRenderMode.MaterialOverride;
            m_Vp.targetMaterialRenderer = tmp_videoPlayerHelper.m_Videorender.GetComponent<Renderer>();
            tmp_videoPlayerHelper.m_Videorender.GetComponent<Renderer>().material.shader = Shader.Find("Custom/Video360");
            m_Vp.Play();
            m_Vp.isLooping = tmp_videoPlayerHelper.m_Isloop;
        }      
    }
}