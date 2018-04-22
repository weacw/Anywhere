/*
*	Function:
*		UI管理
*		
*	Author:
*		Jeno
*		
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Anywhere.Net;
using Aliyun.OSS;
using SuperScrollView;

namespace Anywhere.UI
{

    public class UIManager : Singleton<UIManager>
    {
        //Main Scene
        private HorizontalScorll m_Horizontalscorll;
        [SerializeField]
        private Transform m_Mainuiroot;//主界面UI
        private InputField m_Inputfield;
        private Button m_CallBtn;

        private void Start()
        {
            m_Horizontalscorll = transform.GetComponent<HorizontalScorll>();
            m_Inputfield = m_Mainuiroot.Find("SearchBar/SearchbarField").GetComponent<InputField>();
            m_CallBtn = m_Aruiroot.Find("CallPortalBtn").GetComponent<Button>();
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.UI_SHOWCALLBTN, ShowCallBtn);
            Init();
        }


        //AR Scene
        [SerializeField]
        private Transform m_Aruiroot;//AR界面UI
        private GameObject m_ReturnToMainButton;
        private GameObject m_RecordButton;
        private Text m_Tiptoptext;//上方提示文字


        #region 生命周期
        void Awake()
        {
            m_Horizontalscorll = transform.GetComponent<HorizontalScorll>();
            m_Inputfield = m_Mainuiroot.Find("SearchBar/SearchbarField").GetComponent<InputField>();
            m_ReturnToMainButton = m_Aruiroot.Find("BackButton").gameObject;
            m_RecordButton = m_Aruiroot.Find("RecordButton").gameObject;
            m_Tiptoptext = m_Aruiroot.Find("TipTop/Text").GetComponent<Text>();
        }

        void Update()
        {
            DownLoadListItemAB();
        }

        void Init()
        {
            Debug.Log("start");
            m_Inputfield.onEndEdit.AddListener(OnInputFiledEndEdit);
            NetHttp.Instance.GetPageInfo();
            ClickEventListener tmp_Listener = ClickEventListener.Get(m_ReturnToMainButton.gameObject);
            tmp_Listener.SetClickEventHandler(OnBackToMainButtonClick);
        }

        private void ShowCallBtn(Notification _notif)
        {
            if (!m_CallBtn.gameObject.activeSelf)
            {
                m_CallBtn.gameObject.SetActive(true);
            }
        }


        #endregion

        #region 控件响应
        //输入框输入结束
        private void OnInputFiledEndEdit(string _inputstr)
        {
            //从本地和服务器检索地名
            //Debug.Log(_inputstr);
            m_Horizontalscorll.JumpByLocation(_inputstr);
        }

        public void Return()
        {
            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.ASSETS_REMOVEALL);
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
                if (GetObject.GetDownLoadState() == DownLoadState.DOWNLOADING)
                {
                    if (m_Downloadabitem == null)
                        return;
                    m_Downloadabitem.OnABDownloading();
                }
                if (GetObject.GetDownLoadState() == DownLoadState.DOWNLOADCOMPLETE)
                {
                    if (m_Downloadabitem == null)
                        return;
                    m_Downloadabitem.OnABDownloadComplete();
                    m_Downloadabitem = null;
                    m_Isdownitemab = false;
                }
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
        }
        #endregion
    }

}