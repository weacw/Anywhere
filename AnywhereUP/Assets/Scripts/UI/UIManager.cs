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

    public class UIManager : MonoBehaviour
    {

        public Transform m_Mainuiroot;
        public Transform m_Aruiroot;

        public Button m_SettingBtn;
        public Button m_RefreshBtn;

        public Button m_RecordButton;
        public Button m_ReturnToMainButton;
        public Button m_CallBtn;


        public InputField m_Inputfield;

        public Text m_Tiptoptext;

        //Loading
        public GameObject m_LoadingScreen;
        public GameObject m_RecordGroup;
        /// <summary>
        /// 初始化
        /// </summary>
        private void Start()
        {
            m_CallBtn.onClick.AddListener(CallToPortal);
            m_Inputfield.onEndEdit.AddListener(OnInputFiledEndEdit);
            m_RecordButton.onClick.AddListener(Record);
            m_ReturnToMainButton.onClick.AddListener(OnBackToMainButtonClick);


            //ClickEventListener tmp_Listener = ClickEventListener.Get(m_ReturnToMainButton.gameObject);
            //tmp_Listener.SetClickEventHandler(OnBackToMainButtonClick);

            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.HTTP_GETPAGEDATAS, new HttpGetDataHelper() { m_PageIndex = 0 });
        }

        /// <summary>
        /// 显示召唤按钮
        /// </summary>
        /// <param name="_notif"></param>
        internal void ShowHideCallBtn(Notification _notif)
        {
            UICtrlHelper tmp_UICtrlHelper = _notif.param as UICtrlHelper;
            m_CallBtn.gameObject.SetActive(tmp_UICtrlHelper.m_State);
        }



        /// <summary>
        /// 输入框输入结束
        /// </summary>
        /// <param name="_inputstr"></param>
        private void OnInputFiledEndEdit(string _inputstr)
        {
            if (string.IsNullOrEmpty(_inputstr)) return;
            SearchHelper m_SearchHelper = new SearchHelper() { m_Keywords = _inputstr };
            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.SEARCH_GETRESULT, m_SearchHelper);
        }


        /// <summary>
        /// 拉取下一页数据
        /// </summary>
        private void Refresh()
        {
        }

        /// <summary>
        /// 从ARScene返回到Home
        /// </summary>
        private void OnBackToMainButtonClick()
        {
            SetHintsText(null);
            //从ARScene进入Home
            m_Mainuiroot.gameObject.SetActive(true);
            m_Aruiroot.gameObject.SetActive(false);
            //TODO 重置场景
            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.ARKIT_PAUSE);            
        }

        /// <summary>
        /// 召唤传送门
        /// </summary>
        private void CallToPortal()
        {
            SetHintsText("穿越传送门");
            m_CallBtn.gameObject.SetActive(false);
            m_RecordGroup.SetActive(true);
            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.OPERATER_SETFOCUSPOSTOCONTENT);
        }
        /// <summary>
        /// 录像
        /// </summary>
        private void Record()
        {
            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.EVERYPLAY_RECORDING_START);
        }

        /// <summary>
        /// 设置提示文案
        /// </summary>
        /// <param name="_text"></param>
        private void SetHintsText(string _text)
        {
            m_Tiptoptext.text = _text;
        }




        /// <summary>
        /// 从Home进入ARScene
        /// </summary>
        internal void GoToARScene(Notification _notif)
        {
            SetHintsText("寻找放置传送门的位置");
            m_Mainuiroot.gameObject.SetActive(false);
            m_Aruiroot.gameObject.SetActive(true);
        }


        /// <summary>
        /// 显示隐藏加载界面
        /// </summary>
        /// <param name="_notif"></param>
        internal void ShowHideLoading(Notification _notif)
        {
            UICtrlHelper tmp_UICtrlHelper = _notif.param as UICtrlHelper;
            m_LoadingScreen.SetActive(tmp_UICtrlHelper.m_State);
        }
    }

}