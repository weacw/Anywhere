using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;
using Aliyun.OSS;
using System.IO;
using System.Collections;

namespace Anywhere
{
    /// <summary>
    ///加载视频,图片，AB资源类; 
    /// </summary>
    public class AssetsManager : MonoBehaviour
    {
        public GameObject m_Content { get; private set; }
        private string m_OutPath = null;
        private AssetBundle m_AssetBundle = null;
        private void Awake()
        {
            m_OutPath = Application.streamingAssetsPath + "/AssetBundle/";
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.ASSETS_REMOVEALL, RemoveABSource);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.AB_INSTANCE, InstaniateAB);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.ASSETS_VIDEOPLAY, PlayVideo);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.OPERATER_PLACECONTENT, PlaceContent);

            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.AB_INSTANCE, new ABInstaniateHelper() { m_ABName = "testscene" });
        }


        private void PlaceContent(Notification _notif)
        {
            ContentPlaceHelper cph = _notif.param as ContentPlaceHelper;
            m_Content.transform.localPosition = cph.m_ContentPos;
            m_Content.SetActive(true);
        }


        /// <summary>
        ///_AssetName=null或空时，实例化该AB包所有资源
        /// </summary>
        /// <returns>The A.</returns>
        /// <param name="_ABname">A bname.</param>
        /// <param name="_AssetName">Asset name.</param>
        private void InstaniateAB(Notification _notif)
        {
            ABInstaniateHelper abhelper = _notif.param as ABInstaniateHelper;
            StartCoroutine(SyncLoadABFromFile(abhelper.m_ABName));
        }

        private Texture LoadTexture(string _texturename, int _t2dwith, int _t2dheight)
        {

            byte[] m_T2dbyts = File.ReadAllBytes(Config.DirToDownload + "/Download/Texture/" + _texturename + ".png");
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
            m_Vp.url = "file:///" + Config.DirToDownload + "/Download/Video/" + tmp_videoPlayerHelper.m_Videoname + ".mp4";
            m_Vp.renderMode = VideoRenderMode.MaterialOverride;
            m_Vp.targetMaterialRenderer = tmp_videoPlayerHelper.m_Videorender.GetComponent<Renderer>();
            tmp_videoPlayerHelper.m_Videorender.GetComponent<Renderer>().material.shader = Shader.Find("Custom/Video360");
            m_Vp.Play();
            m_Vp.isLooping = tmp_videoPlayerHelper.m_Isloop;
        }





        private IEnumerator SyncLoadABFromFile(string _name)
        {
            yield return null;
            AssetBundleCreateRequest abcr = null;
            abcr = AssetBundle.LoadFromFileAsync(m_OutPath + _name + ".assetbundle");
            abcr.completed += (x) =>
            {
                StartCoroutine(SyncABLoad(abcr));
            };
        }
        private IEnumerator SyncABLoad(AssetBundleCreateRequest _r)
        {
            yield return null;
            m_AssetBundle = _r.assetBundle;
            AssetBundleRequest r = m_AssetBundle.LoadAllAssetsAsync<GameObject>();
            if (r == null) yield return null; ;
            m_Content = Instantiate(r.asset as GameObject);
            m_Content.SetActive(false);
            m_AssetBundle.Unload(false);
        }
    }
}