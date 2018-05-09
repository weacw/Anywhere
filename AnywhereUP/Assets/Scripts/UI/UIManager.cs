/*
*	Function:
*		UI管理
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

    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private Transform m_Mainuiroot;
        [SerializeField] private Transform m_Aruiroot;

        //Main Scene
        //private LoopListViewHelper m_Horizontalscorll;
        private InputField m_Inputfield;
        private Button m_CallBtn;

        //AR Scene
        private GameObject m_ReturnToMainButton;
        private GameObject m_RecordButton;

        //上方提示文字
        private Text m_Tiptoptext;
        public GameObject m_LoadingScreen;
        #region 生命周期
        private void Start()
        {
            //m_Horizontalscorll = transform.GetComponent<LoopListViewHelper>();
            m_Inputfield = m_Mainuiroot.Find("SearchBar/SearchbarField").GetComponent<InputField>();
            m_ReturnToMainButton = m_Aruiroot.Find("BackButton").gameObject;
            m_RecordButton = m_Aruiroot.Find("Recording/RecordingBtn").gameObject;
            m_Tiptoptext = m_Aruiroot.Find("Hint/Hintbackground/Hinttext").GetComponent<Text>();
            m_CallBtn = m_Aruiroot.Find("CallPortalBtn").GetComponent<Button>();
            m_LoadingScreen = m_Mainuiroot.Find("Loading").gameObject;
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.UI_SHOWCALLBTN, ShowCallBtn);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.UI_HIDECALLBTN, HideCallBtn);


            m_Inputfield.onEndEdit.AddListener(OnInputFiledEndEdit);
            ClickEventListener tmp_Listener = ClickEventListener.Get(m_ReturnToMainButton.gameObject);
            tmp_Listener.SetClickEventHandler(OnBackToMainButtonClick);
            m_RecordButton.GetComponent<Button>().onClick.AddListener(() => NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.EVERYPLAY_RECORDING_START));

            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.HTTP_GETPAGEDATAS, new HttpGetDataHelper() { m_PageIndex = 0 });
        }
        void Update()
        {
            DownLoadListItemAB();
        }

        private void ShowCallBtn(Notification _notif)
        {
            if (!m_CallBtn.gameObject.activeSelf)
            {
                m_CallBtn.gameObject.SetActive(true);
            }
        }
        private void HideCallBtn(Notification _notif)
        {
            if (m_CallBtn.gameObject.activeSelf)
            {
                m_CallBtn.gameObject.SetActive(false);
            }
        }

        #endregion

        #region 控件响应
        //输入框输入结束
        private void OnInputFiledEndEdit(string _inputstr)
        {
            //从本地和服务器检索地名
            //Debug.Log(_inputstr);
            if (string.IsNullOrEmpty(_inputstr)) return;
            SearchHelper m_SearchHelper = new SearchHelper() { m_Keywords = _inputstr };
            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.SEARCH_GETRESULT, m_SearchHelper);
        }

        public void Return()
        {
            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.ARKIT_PAUSE);
        }

        private void Refresh()
        {
        }
        //返回主界面按钮
        private void OnBackToMainButtonClick(GameObject btn)
        {
            //场景跳转
            BackToMainScene();
            //TODO 重置场景
        }

        #endregion


        //场景跳转-AR场景
        public void JumpToARScene()
        {
            m_Mainuiroot.gameObject.SetActive(false);
            m_Aruiroot.gameObject.SetActive(true);
        }

        //场景跳转-主场景
        public void BackToMainScene()
        {
            m_Mainuiroot.gameObject.SetActive(true);
            m_Aruiroot.gameObject.SetActive(false);
        }

        #region 列表单元下载AB

        private ListItem m_Downloadabitem;//在下载AB的ListItem
        private bool m_Isdownitemab;//是否在下载AB

        /// <summary>
        /// 实时更新下载状态
        /// </summary>
        private void DownLoadListItemAB()
        {
            if (m_Isdownitemab)
            {
                //if (GetObject.GetDownLoadState() == DownLoadState.DOWNLOADING)
                //{
                //    if (m_Downloadabitem == null)
                //        return;
                //    m_Downloadabitem.OnABDownloading();
                //}

                //if (GetObject.GetDownLoadState() == DownLoadState.DOWNLOADCOMPLETE)
                //{
                //    if (m_Downloadabitem == null)
                //        return;
                //    m_Downloadabitem.OnABDownloadComplete();
                //    m_Downloadabitem = null;
                //    m_Isdownitemab = false;
                //}
            }
        }

        /// <summary>
        /// 开始下载ListItem对应的AB
        /// </summary>
        /// <param name="item"></param>
        public void StartListItemABDownload(ListItem item)
        {
            m_Isdownitemab = true;
            m_Downloadabitem = item;
        }

        public void CallToPortal()
        {
            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.OPERATER_SETFOCUSPOSTOCONTENT);
            m_CallBtn.gameObject.SetActive(false);
        }



        #endregion
    }

}