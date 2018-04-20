/*
*	Function:
*		列表单元初始化、属性设置
*		
*	Author:
*		Jeno
*		
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;

namespace Anywhere.UI
{

    public class ListItem : MonoBehaviour
    {

        public GameObject m_Contentrootobj;

        public Text m_Indextext;//索引
        public Text m_Destext;//描述
        //public Text m_Loactiontext;//位置
        public Image m_Icon;//图片

        public Button m_Downloadbtn;//下载按钮

        public void Init()
        {
            ClickEventListener tmp_Listener = ClickEventListener.Get(m_Downloadbtn.gameObject);
            tmp_Listener.SetClickEventHandler(OnDownloadBtnClick);
        }

        public void SetItemData(ItemData _itemdata, int _itemindex)
        {
            if (_itemdata == null)
            {
                Debug.LogError("itemData is null ! pls check");
                return;
            }
            m_Indextext.text = _itemindex.ToString();
            m_Destext.text = _itemdata.m_Des + "\n" + _itemdata.m_Location;
            m_Destext.text.Replace("\\n", "\n");
        }

        void OnDownloadBtnClick(GameObject _btn)
        {
            Debug.Log("开始下载");
            //下载中--环形进度条、显示进度百分比数字
            //下载完成--文字改成打开--点击进入AR场景
        }

    }
}