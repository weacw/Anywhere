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

    public class UIManager : MonoBehaviour
    {
        private HorizontalScorll m_Horizontalscorll;
        [SerializeField]
        private Transform m_Mainuiroot;
        [SerializeField]
        private Transform m_Aruiroot;

        private InputField m_Inputfield;


        #region 生命周期
        void Awake()
        {
            m_Horizontalscorll = transform.GetComponent<HorizontalScorll>();
            m_Inputfield = m_Mainuiroot.Find("SearchBar/SearchbarField").GetComponent<InputField>();
        }

        void Start()
        {
            Init();
        }

        void Update()
        {

        }

        void Init()
        {
            Debug.Log("start");
            m_Inputfield.onEndEdit.AddListener(OnInputFiledEndEdit);
            NetHttp.Instance.GetPageInfo();
        }

        #endregion


        private void OnInputFiledEndEdit(string _inputstr)
        {
            //从本地和服务器检索地名
            Debug.Log(_inputstr);
            m_Horizontalscorll.JumpByLocation(_inputstr);
        }



    }
}