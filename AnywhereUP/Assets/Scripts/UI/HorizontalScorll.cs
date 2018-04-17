/*
*	Function:
*		列表功能
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

    public class HorizontalScorll : MonoBehaviour
    {
        public LoopListView m_Looplistview;

        void Start()
        {
            m_Looplistview.InitListView(-1, OnGetItemByIndex);
        }

        LoopListViewItem OnGetItemByIndex(LoopListView _listview, int _index)
        {
     
            //get a new item. Every item can use a different prefab, the parameter of the NewListViewItem is the prefab’name. 
            //And all the prefabs should be listed in ItemPrefabList in LoopListView2 Inspector Setting
            LoopListViewItem tmp_Item = _listview.NewListViewItem("Element");
            ListItem tmp_itemscript = tmp_Item.GetComponent<ListItem>();
            if (tmp_Item.IsInitHandlerCalled == false)
            {
                tmp_Item.IsInitHandlerCalled = true;
                tmp_itemscript.Init();
            }

            _index = _index % DatasourceMgr.Get.m_Totalitemcount;
            if (_index < 0)
            {
                _index = DatasourceMgr.Get.m_Totalitemcount + _index;
            }

            ItemData tmp_Itemdata = DatasourceMgr.Get.GetItemDataByIndex(_index);
            tmp_itemscript.SetItemData(tmp_Itemdata, _index);
            return tmp_Item;
        }


        void LateUpdate()
        {
            m_Looplistview.UpdateAllShownItemSnapData();

            //切换缩放效果
            int tmp_Count = m_Looplistview.ShownItemCount;
            for (int i = 0; i < tmp_Count; ++i)
            {
                LoopListViewItem tmp_Item = m_Looplistview.GetShownItemByIndex(i);
                ListItem tmp_Itemscript = tmp_Item.GetComponent<ListItem>();
                float tmp_Scale = 1 - Mathf.Abs(tmp_Item.DistanceWithViewPortSnapCenter) / 700f;
                tmp_Scale = Mathf.Clamp(tmp_Scale, 0.8f, 1);
                tmp_Itemscript.m_Contentrootobj.GetComponent<CanvasGroup>().alpha = tmp_Scale;
                tmp_Itemscript.m_Contentrootobj.transform.localScale = new Vector3(tmp_Scale, tmp_Scale, 1);
            }
        }

        public void OnLoopListViewFinished(LoopListView _listview, LoopListViewItem _item)
        {
            LoopListViewItem tmp_Item0 = m_Looplistview.GetShownItemByIndex(0);
            ListItem itemScript = tmp_Item0.GetComponent<ListItem>();
            m_Looplistview.RefreshAllShownItemWithFirstIndex(0);
        }


    //    public void CalculateScale()
    //    {
    //        float standard_width = 750f;        //初始宽度  
    //        float standard_height = 1334f;       //初始高度  
    //        float device_width = 0f;                //当前设备宽度  
    //        float device_height = 0f;               //当前设备高度  
    //        float adjustor = 0f;         //屏幕矫正比例  
    //        //获取设备宽高  
    //        device_width = Screen.width;
    //        device_height = Screen.height;
    //        //计算宽高比例  
    //        float standard_aspect = standard_width / standard_height;
    //        float device_aspect = device_width / device_height;
    //        //计算矫正比例  
    //        if (device_aspect < standard_aspect)
    //        {
    //            adjustor = standard_aspect / device_aspect;
    //        }

    //        CanvasScaler canvasScalerTemp = transform.GetComponent<CanvasScaler>();
    //        if (adjustor == 0)
    //        {
    //            canvasScalerTemp.matchWidthOrHeight = 1;
    //        }
    //        else
    //        {
    //            canvasScalerTemp.matchWidthOrHeight = 0;
    //        }  
    //    }
    }

}

