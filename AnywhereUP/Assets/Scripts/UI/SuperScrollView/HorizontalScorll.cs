/*
*	Function:
*		
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

    public class HorizontalScorll : MonoBehaviour
    {
        public LoopListView mLoopListView;

        // Use this for initialization
        void Start()
        {
            mLoopListView.InitListView(-1, OnGetItemByIndex);
        }

        LoopListViewItem OnGetItemByIndex(LoopListView listView, int index)
        {
     
            //get a new item. Every item can use a different prefab, the parameter of the NewListViewItem is the prefab’name. 
            //And all the prefabs should be listed in ItemPrefabList in LoopListView2 Inspector Setting
            LoopListViewItem item = listView.NewListViewItem("Element");
            ListItem itemScript = item.GetComponent<ListItem>();
            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
                itemScript.Init();
            }

            index = index % DatasourceMgr.Get.TotalItemCount;
            if (index < 0)
            {
                index = DatasourceMgr.Get.TotalItemCount + index;
            }

            ItemData itemData = DatasourceMgr.Get.GetItemDataByIndex(index);
            itemScript.SetItemData(itemData, index);
            return item;
        }


        void LateUpdate()
        {
            mLoopListView.UpdateAllShownItemSnapData();

            //切换缩放效果
            int count = mLoopListView.ShownItemCount;
            for (int i = 0; i < count; ++i)
            {
                LoopListViewItem item = mLoopListView.GetShownItemByIndex(i);
                ListItem itemScript = item.GetComponent<ListItem>();
                float scale = 1 - Mathf.Abs(item.DistanceWithViewPortSnapCenter) / 700f;
                scale = Mathf.Clamp(scale, 0.8f, 1);
                itemScript.mContentRootObj.GetComponent<CanvasGroup>().alpha = scale;
                itemScript.mContentRootObj.transform.localScale = new Vector3(scale, scale, 1);
            }
        }

        public void OnLoopListViewFinished(LoopListView listView, LoopListViewItem item)
        {
            LoopListViewItem item0 = mLoopListView.GetShownItemByIndex(0);
            ListItem itemScript = item0.GetComponent<ListItem>();
            //int index = itemScript.Value - 1;
            mLoopListView.RefreshAllShownItemWithFirstIndex(0);
        }

    }

}

