using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;
using Aliyun.OSS;
using System.IO;
using System.Collections;
using UnityEngine.XR.iOS;

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
            m_OutPath = Config.GetCachePath();
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.ASSETS_REMOVEALL, RemoveABSource);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.ASSETS_ABINSTANCE, InstaniateAB);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.ASSETS_VIDEOPLAY, PlayVideo);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.ASSETS_IMAGELOAD, LoadTexture);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.OPERATER_PLACECONTENT, PlaceContent);
            // NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.AB_INSTANCE, new ABInstaniateHelper() { m_ABName = "testscene" });
        }


        private void PlaceContent(Notification _notif)
        {
            ContentPlaceHelper cph = _notif.param as ContentPlaceHelper;
            m_Content.transform.position = cph.m_ContentPos;
            m_Content.transform.rotation = cph.m_ContentRot;
            m_Content.SetActive(true);
            //cph.m_FocusGameObject.SetActive(false);
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

        private void LoadTexture(Notification _notif)
        {
            Image360Helper tmp_image360Helper = _notif.param as Image360Helper;            
            byte[] m_T2dbyts = File.ReadAllBytes(Config.DirToDownload + "/" + tmp_image360Helper.m_ImageName + ".jpg");
            Texture2D m_T2d = new Texture2D(tmp_image360Helper.m_Width, tmp_image360Helper.m_Height);
            m_T2d.LoadImage(m_T2dbyts);          
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
            GameObject tmp_videoPlayer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Renderer tmp_rd = tmp_videoPlayer.GetComponent<Renderer>();
            tmp_rd.material = Resources.Load<Material>("Video360_Mat");
            VideoPlayer m_Vp = tmp_videoPlayer.AddComponent<VideoPlayer>();
            m_Vp.source = VideoSource.Url;
#if UNITY_EDITOR
            m_Vp.url = Config.DirToDownload + "/" + tmp_videoPlayerHelper.m_Videoname + ".mp4";
#else
            m_Vp.url = "file://"+ Config.DirToDownload +"/"+ tmp_videoPlayerHelper.m_Videoname + ".mp4";
#endif
            m_Vp.renderMode = VideoRenderMode.MaterialOverride;
            m_Vp.targetMaterialRenderer = tmp_videoPlayer.GetComponent<Renderer>();
            //tmp_videoPlayerHelper.m_Videorender.GetComponent<Renderer>().material.shader = Shader.Find("Custom/Video360");
            m_Vp.Play();
            m_Vp.isLooping = tmp_videoPlayerHelper.m_Isloop;
        }




        /// <summary>
        /// 异步将Assetbundle文件从缓存中加载到内存
        /// </summary>
        /// <param name="_name"></param>
        /// <returns></returns>
        private IEnumerator SyncLoadABFromFile(string _name)
        {
            yield return null;
            AssetBundleCreateRequest abcr = null;
            abcr = AssetBundle.LoadFromFileAsync(Path.Combine(m_OutPath, _name + ".assetbundle"));
            abcr.completed += (x) =>
            {
                StartCoroutine(SyncABLoad(abcr));
            };
        }

        /// <summary>
        /// 异步将内存中的Assetbundle实例化到场景中
        /// </summary>
        /// <param name="_request"></param>
        /// <returns></returns>
        private IEnumerator SyncABLoad(AssetBundleCreateRequest _request)
        {
            yield return null;
            m_AssetBundle = _request.assetBundle;
            AssetBundleRequest r = m_AssetBundle.LoadAllAssetsAsync<GameObject>();
            if (r == null) yield return null; ;
            m_Content = Instantiate(r.asset as GameObject);
            m_Content.SetActive(false);
            m_AssetBundle.Unload(false);
        }
    }
}