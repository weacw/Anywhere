﻿/*
*	Function:
*		UI管理
*		
*	Author:
*		Jeno
*		
*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Anywhere.UI
{

    public class UIManager : MonoBehaviour
    {

        public Transform m_MainUIRoot;
        public Transform m_ARUIRoot;
        public Transform m_SettingRoot;

        public Button m_SettingBtn;
        public Button m_RefreshBtn;

        public Button m_RecordButton;
        public Button m_ReturnToMainButton;
        public Button m_CallBtn;

        public Button m_CleanCache;
        public Button m_About;
        public Button m_SettingReturnHome;

        public InputField m_Inputfield;
        public Text m_Tiptoptext;
        public Text m_CacheSizetext;

        public Image m_RecordProgress;

        //Loading
        public GameObject m_LoadingScreen;
        public GameObject m_SearchNotFoundScreen;
        public GameObject m_LoadingWaittingScreen;
        public GameObject m_RecordGroup;

        private float m_CurTime;
        private IEnumerator m_RecordCoroutine;

        /// <summary>
        /// 初始化
        /// </summary>
        private void Start()
        {
            m_CallBtn.onClick.AddListener(CallToPortal);
            m_Inputfield.onEndEdit.AddListener(OnInputFiledEndEdit);
            m_RecordButton.onClick.AddListener(Record);
            m_ReturnToMainButton.onClick.AddListener(OnBackToMainButtonClick);
            m_CleanCache.onClick.AddListener(CleanCache);
            m_SettingBtn.onClick.AddListener(ShowSettingPanel);
            m_SettingReturnHome.onClick.AddListener(SettingReturnHome);
            m_About.onClick.AddListener(() => { Application.OpenURL("https://aw.weacw.com/anywhere/about/"); });
            m_RefreshBtn.onClick.AddListener(Refresh);

            m_RecordCoroutine = RecordTimeCoroutine();
        }

        private void ShowSettingPanel()
        {
            m_SettingRoot.gameObject.SetActive(true);
            ShowCacheSize();
        }
        private void CleanCache()
        {
            CacheMachine.CleanCache();
            ShowCacheSize();
            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.UI_REFRESHDATAS);
        }
        private void SettingReturnHome()
        {
            m_SettingRoot.gameObject.SetActive(false);
            m_MainUIRoot.gameObject.SetActive(true);
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
            DataSource.Instance.RefreshDatas();
            Loom.RunAsync(() =>
            {
                new System.Threading.Thread(() =>
                {
                    HttpGetDataHelper tmp_HttpGetDataHelper = new HttpGetDataHelper();
                    tmp_HttpGetDataHelper.m_Finished = () => NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.UI_REFRESHDATAS);
                    tmp_HttpGetDataHelper.m_PageIndex = Configs.GetConfigs.ContentPageNum;
                    NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.HTTP_GETPAGEDATAS, tmp_HttpGetDataHelper);
                }).Start();
            });

        }

        /// <summary>
        /// 从ARScene返回到Home
        /// </summary>
        private void OnBackToMainButtonClick()
        {
            SetHintsText(null);
            //从ARScene进入Home
            m_MainUIRoot.gameObject.SetActive(true);
            m_ARUIRoot.gameObject.SetActive(false);
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
            StartCoroutine(m_RecordCoroutine);
        }

        /// <summary>
        /// 设置提示文案
        /// </summary>
        /// <param name="_text"></param>
        private void SetHintsText(string _text)
        {
            m_Tiptoptext.text = _text;
        }


        private void ShowCacheSize()
        {
            m_CacheSizetext.text = CacheMachine.GetCacheSize().ToString("0.0") + " M";
        }

        private IEnumerator RecordTimeCoroutine()
        {
            while (m_CurTime < Configs.GetConfigs.m_MaxRecordTime)
            {
                yield return null;
                m_CurTime += Time.deltaTime;
                m_RecordProgress.fillAmount = (m_CurTime / Configs.GetConfigs.m_MaxRecordTime);
            }
            yield return null;

            m_CurTime = 0;
            m_RecordProgress.fillAmount = m_CurTime;
            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.EVERYPLAY_RECORDING_STOP);
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
        /// 从Home进入ARScene
        /// </summary>
        internal void GoToARScene(Notification _notif)
        {
            SetHintsText("寻找放置传送门的位置");
            m_MainUIRoot.gameObject.SetActive(false);
            m_ARUIRoot.gameObject.SetActive(true);
        }


        /// <summary>
        /// 显示隐藏加载界面
        /// </summary>
        /// <param name="_notif"></param>
        internal void ShowHideLoading(Notification _notif)
        {
            UICtrlHelper tmp_UICtrlHelper = _notif.param as UICtrlHelper;
            Loom.QueueOnMainThread((parm) =>
            {
                m_LoadingScreen.SetActive(tmp_UICtrlHelper.m_State);

                if (tmp_UICtrlHelper.m_ResultType == "SearchNotFound")
                {
                    m_SearchNotFoundScreen.SetActive(tmp_UICtrlHelper.m_State);
                    m_LoadingWaittingScreen.SetActive(!tmp_UICtrlHelper.m_State);
                }
                else
                {
                    m_SearchNotFoundScreen.SetActive(!tmp_UICtrlHelper.m_State);
                    m_LoadingWaittingScreen.SetActive(tmp_UICtrlHelper.m_State);
                }
            }, null);
        }

    }

}