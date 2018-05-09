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
using System.IO;

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
            string path = Application.streamingAssetsPath + "/ResCache/" + _itemdata.assetName + "." + _itemdata.type;

            if (File.Exists(path))
            {
                m_Assetisdownloaded = true;
            }

            if (m_Assetisdownloaded)
            {
                m_Downloadbtntext.text = "打开";
            }
            else
            {
                m_Downloadbtntext.text = "下载";
            }

            if (DatasourceMgr.Instance.GetItemBackgroundById(m_Pageitem.id) != null)
            {
                m_Icon.sprite = DatasourceMgr.Instance.GetItemBackgroundById(m_Pageitem.id);
            }
        }

        public void UpdateThumbnail(Sprite _sprite)
        {
            m_Icon.sprite = _sprite;
        }

        void OnDownloadBtnClick(GameObject _btn)
        {
            Debug.Log("开始下载");
            if (m_Assetisdownloaded)
            {
                NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.ARKIT_PLAY);
                NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.AB_INSTANCE, new ABInstaniateHelper() { m_ABName = m_CurData });
                //进入场景
                UIManager.Instance.JumpToARScene();
            }
            else
            {
                Debug.Log("下载AB包：" + m_Pageitem.assetName + "." + m_Pageitem.type.ToLower());
                UIManager.Instance.StartListItemABDownload(this);
                GetObject.AsyncGetObject("anywhere-v-1", m_Pageitem.assetName + "." + m_Pageitem.type.ToLower());
            }
        }

        //下载中
        public void OnABDownloading()
        {
            m_Progressimg.fillAmount = GetObject.GetDownLoadProgress();
            if (m_Assetisdownloading)
                return;
            m_Assetisdownloading = true;
            m_Downloadbtntext.text = "下载中";
            m_Downloadbtn.gameObject.SetActive(false);
            m_Downloadprogress.gameObject.SetActive(true);
            m_Progressimg.gameObject.SetActive(true);
            //Debug.Log("进度"+GetObject.GetDownLoadProgress());
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