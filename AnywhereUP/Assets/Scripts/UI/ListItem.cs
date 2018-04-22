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
using Anywhere.Net;
using Aliyun.OSS;

namespace Anywhere.UI
{

    public class ListItem : MonoBehaviour
    {
        public GameObject m_Contentrootobj;

        public Text m_Destext;//描述
        public Text m_Loactiontext;//位置
        public Image m_Icon;//图片
        public Button m_Downloadbtn;//下载按钮

        private Text m_Downloadbtntext;//按钮文字
        private Image m_Progressimg;//进度
        private PageItem m_Pageitem;//自身属性
        private bool m_Assetisdownloading;
        private bool m_Assetisdownloaded;

        public void Init()
        {
            ClickEventListener tmp_Listener = ClickEventListener.Get(m_Downloadbtn.gameObject);
            tmp_Listener.SetClickEventHandler(OnDownloadBtnClick);
            m_Downloadbtntext = m_Downloadbtn.transform.Find("Text").GetComponent<Text>();
            m_Progressimg = m_Downloadbtn.transform.parent.Find("Progress").GetComponent<Image>();
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
            m_Destext.text = _itemdata.descript + "\n" + _itemdata.place;
            m_Destext.text.Replace("\\n", "\n");
            m_Progressimg.fillAmount = 0;
            m_Progressimg.gameObject.SetActive(false);

            //TODO  判断是否已下载
            m_Assetisdownloaded = false;
            if (m_Assetisdownloaded)
            {
                m_Downloadbtntext.text = "打开";
            }
            else
            {
                m_Downloadbtntext.text = "下载";
            }
        }

        void OnDownloadBtnClick(GameObject _btn)
        {
            Debug.Log("开始下载");
            if (m_Assetisdownloaded)
            {
                //进入场景
                UIManager.Instance.JumpToARScene();
            }
            else
            {
                Debug.Log("下载AB包：" + m_Pageitem.assetName + ".assetbundle");
                UIManager.Instance.StartListItemABDownload(this);
                //GetObject.AsyncGetObject("anywhere-v-1", "assets.assetbundle");
                GetObject.AsyncGetObject("anywhere-v-1", m_Pageitem.assetName + ".assetbundle");
            }
        }

        //下载中
        public void OnABDownloading()
        {
            if (m_Assetisdownloading)
                return;
            m_Assetisdownloading = true;
            m_Downloadbtntext.text = "下载中";
            m_Progressimg.gameObject.SetActive(true);
            m_Progressimg.fillAmount = GetObject.GetDownLoadProgress();
            //Debug.Log("进度"+GetObject.GetDownLoadProgress());
        }

        //下载结束
        public void OnABDownloadComplete()
        {
            m_Assetisdownloaded = true;
            m_Assetisdownloading = false;
            m_Downloadbtntext.text = "打开";
            m_Progressimg.gameObject.SetActive(false);
        }

    }
}