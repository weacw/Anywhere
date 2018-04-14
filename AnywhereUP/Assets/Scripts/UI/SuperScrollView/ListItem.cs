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

namespace Anywhere.UI
{

    public class ListItem : MonoBehaviour
    {

        public GameObject mContentRootObj;

        public Text mIndexText;//索引
        public Text mDesText;//描述
        public Text mLoactionText;//位置
        public Image mIcon;//图片

        public Button mDownloadBtn;//下载按钮

        public void Init()
        {
            ClickEventListener listener = ClickEventListener.Get(mDownloadBtn.gameObject);
            listener.SetClickEventHandler(OnDownloadBtnClick);
        }

        public void SetItemData(ItemData itemData, int itemIndex)
        {

            mIndexText.text = itemIndex.ToString();
            mDesText.text = itemData== null ? " " : itemData.mDes;
            mLoactionText.text = itemData== null ? " " : itemData.mLocation;

        }

        void OnDownloadBtnClick(GameObject btn)
        {
            Debug.Log("开始下载");
        }

    }
}