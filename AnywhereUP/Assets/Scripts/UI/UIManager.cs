﻿/*
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
            m_Inputfield.onEndEdit.AddListener(OnInputFiledEndEdit);
        }

        #endregion


        private void OnInputFiledEndEdit(string _inputstr)
        {
            Debug.Log(_inputstr);
            m_Horizontalscorll.JumpByLocation(_inputstr);
        }



    }
}