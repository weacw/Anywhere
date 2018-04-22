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

namespace Anywhere.UI
{

    public class UIManager : Singleton<UIManager>
    {
        private HorizontalScorll m_Horizontalscorll;
        [SerializeField]
        private Transform m_Mainuiroot;
        [SerializeField]
        private Transform m_Aruiroot;

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
        
        private void Init()
        {
            Debug.Log("start");
            m_Inputfield.onEndEdit.AddListener(OnInputFiledEndEdit);
            NetHttp.Instance.GetPageInfo();
        }
     
        private void ShowCallBtn(Notification _notif)
        {
            if (!m_CallBtn.gameObject.activeSelf)
            {
                m_CallBtn.gameObject.SetActive(true);
            }
        }

        private void OnInputFiledEndEdit(string _inputstr)
        {
            //从本地和服务器检索地名
            Debug.Log(_inputstr);
            m_Horizontalscorll.JumpByLocation(_inputstr);
        }

        public void Return()
        {
            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.ASSETS_REMOVEALL);
        }

        private void Refresh()
        {

        }

        public void CallToPortal()
        {
            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.OPERATER_SETFOCUSPOSTOCONTENT);            
        }
    }
}