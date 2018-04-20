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

        #region 生命周期


        void Start()
        {
            m_Looplistview.InitListView(-1, OnGetItemByIndex);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //LoopListViewItem tmp_Item = _listview.l("Element");
            }
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

        #endregion

        /// <summary>
        /// 通过索引获取列表单元
        /// </summary>
        /// <param name="_listview"></param>
        /// <param name="_index"></param>
        /// <returns></returns>
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

            _index = _index % DatasourceMgr.INSTANCE.m_Totalitemcount;
            if (_index < 0)
            {
                _index = DatasourceMgr.INSTANCE.m_Totalitemcount + _index;
            }

            ItemData tmp_Itemdata = DatasourceMgr.INSTANCE.GetItemDataByIndex(_index);
            tmp_itemscript.SetItemData(tmp_Itemdata, _index);
            return tmp_Item;
        }


        public void OnLoopListViewFinished(LoopListView _listview, LoopListViewItem _item)
        {
            LoopListViewItem tmp_Item0 = m_Looplistview.GetShownItemByIndex(0);
            ListItem itemScript = tmp_Item0.GetComponent<ListItem>();
            m_Looplistview.RefreshAllShownItemWithFirstIndex(0);
        }

        /// <summary>
        /// 根据地点检索
        /// </summary>
        /// <param name="_place"></param>
        public void JumpByLocation(string _place)
        {
            ItemData tmp_Item = DatasourceMgr.INSTANCE.GetItemDataByPlace(_place);
            if (tmp_Item == null)
            {
                Debug.LogError("未找到对应地点的数据");
                return;
            }
            m_Looplistview.MovePanelToItemIndex(tmp_Item.m_Id, 0);
        }

    }

}

