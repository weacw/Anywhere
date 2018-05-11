/*
*	Function:
*		列表单元初始化、属性设置
*		
*	Author:
*		Jeno
*		
*/
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;

namespace Anywhere.UI
{

    public class ListItem : MonoBehaviour
    {
        public GameObject m_Contentrootobj;
        public Transform m_DownloadArea;
        public Text m_Destext;//描述
        public Text m_Loactiontext;//位置
        public Image m_Icon;//图片
        public Button m_Downloadbtn;//下载按钮

        private Text m_Downloadbtntext;//按钮文字
        private Transform m_Downloadprogress;//进度父对象
        private Image m_Progressimg;//进度圈
        private PageItem m_Pageitem;//自身属性
        private bool m_Assetisdownloading;
        private bool m_Assetisdownloaded;
        private string m_CurData;
        public void Init()
        {
            ClickEventListener tmp_Listener = ClickEventListener.Get(m_Downloadbtn.gameObject);
            tmp_Listener.SetClickEventHandler(OnDownloadBtnClick);
            m_Downloadbtntext = m_Downloadbtn.transform.Find("Text").GetComponent<Text>();
            m_Progressimg = m_DownloadArea.Find("DownloadProgress/ProgressValue").GetComponent<Image>();
            m_Downloadprogress = m_DownloadArea.Find("DownloadProgress");
        }

        public void SetItemData(PageItem _itemdata, int _itemindex)
        {
            if (_itemdata == null)
            {
                Debug.LogError("itemData is null ! pls check");
                return;
            }

            m_Destext.text = _itemdata.descript;
            m_Loactiontext.text = _itemdata.place;
            m_Pageitem = _itemdata;
            m_Destext.text = _itemdata.descript;
            m_Progressimg.fillAmount = 0;
            m_Progressimg.transform.parent.gameObject.SetActive(false);
            m_Progressimg.gameObject.SetActive(false);
            m_CurData = _itemdata.assetName;


            //TODO  判断是否已下载
            m_Assetisdownloaded = false;
            string path = Configs.GetConfigs.m_CachePath + _itemdata.assetName + "." + _itemdata.type;

            m_Assetisdownloaded = CacheMachine.IsCache(path);
            m_Downloadbtntext.text = m_Assetisdownloaded ? "打开" : "下载";
            Sprite tmp_Sprite = DataSource.Instance.GetItemBackgroundById(m_Pageitem.id);
            m_Icon.sprite = tmp_Sprite ? tmp_Sprite : null;
        }

        public void UpdateThumbnail(Sprite _sprite)
        {
            m_Icon.sprite = _sprite;
        }

        void OnDownloadBtnClick(GameObject _btn)
        {
            if (m_Assetisdownloaded)
            {
                NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.ARKIT_PLAY);
                string tmp_Type = m_Pageitem.type.ToLower();
                if (tmp_Type.CompareTo("assetbundle") == 0)
                {
                    NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.ASSETS_ABINSTANCE, new ABInstaniateHelper()
                    {
                        m_ABName = m_CurData,
                        m_BeginInstance = () => NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.UI_SHOWHIDELOADING, new UICtrlHelper() { m_State = true }),
                        m_EndIntance = () => NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.UI_SHOWHIDELOADING, new UICtrlHelper() { m_State = false })
                    });
                }
                else if (tmp_Type.CompareTo("mp4") == 0)
                {
                    NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.ASSETS_VIDEOPLAY, new VideoPlayerHelper()
                    {
                        m_Videoname = m_Pageitem.assetName,
                        m_BeginInstance = () => NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.UI_SHOWHIDELOADING, new UICtrlHelper() { m_State = true }),
                        m_EndIntance = () => NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.UI_SHOWHIDELOADING, new UICtrlHelper() { m_State = false })
                    });
                }
                else if (tmp_Type.CompareTo("jpg") == 0 || tmp_Type.CompareTo("png") == 0)
                {
                    NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.ASSETS_IMAGE360, new Image360Helper()
                    {
                        m_ImageName = m_Pageitem.assetName,
                        m_BeginInstance = () => NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.UI_SHOWHIDELOADING, new UICtrlHelper() { m_State = true }),
                        m_EndIntance = () => NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.UI_SHOWHIDELOADING, new UICtrlHelper() { m_State = false })
                    });
                }
                //进入场景
                NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.UI_GOTOARSCENE);
            }
            else
            {
                Debug.Log("开始下载");
                HttpRequestHelper tmp_HttpRequestHelper = new HttpRequestHelper();
                tmp_HttpRequestHelper.m_LocalPath = Configs.GetConfigs.m_CachePath;
                Loom.RunAsync(() =>
                {
                    tmp_HttpRequestHelper.m_URI = Configs.GetConfigs.m_OSSURI + m_CurData + "." + m_Pageitem.type.ToLower();
                    tmp_HttpRequestHelper.m_Downloading = (progress) =>
                    {
                        m_Progressimg.fillAmount = progress;
                        if (!m_Assetisdownloading) OnABDownloading();
                    };
                    tmp_HttpRequestHelper.m_Succeed = (json) => OnABDownloadComplete();
                    NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.HTTP_DOWNLOADFILE, tmp_HttpRequestHelper);
                });
            }
        }

        //下载中
        public void OnABDownloading()
        {
            if (m_Assetisdownloading)
                return;
            m_Assetisdownloading = true;
            m_Downloadbtntext.text = "下载中";
            m_Downloadbtn.gameObject.SetActive(false);
            m_Downloadprogress.gameObject.SetActive(true);
            m_Progressimg.gameObject.SetActive(true);
        }

        //下载结束
        public void OnABDownloadComplete()
        {
            m_Downloadbtn.gameObject.SetActive(true);
            m_Assetisdownloaded = true;
            m_Assetisdownloading = false;
            m_Downloadbtntext.text = "打开";
            m_Downloadprogress.gameObject.SetActive(false);
            m_Progressimg.gameObject.SetActive(false);
        }
    }
}