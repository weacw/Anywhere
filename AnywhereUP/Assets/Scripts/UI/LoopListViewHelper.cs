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
using Anywhere.Net;
using Anywhere.UI;
namespace Anywhere
{

    public class LoopListViewHelper : Singleton<LoopListViewHelper>
    {
        public LoopListView m_Looplistview;
        #region 生命周期

        private void LateUpdate()
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
                tmp_Itemscript.m_Contentrootobj.transform.localScale = new Vector3(tmp_Scale, tmp_Scale, 1);
            }
        }

        #endregion

        /// <summary>
        /// 创建列表
        /// </summary>
        internal void CreatPages(Notification _notif)
        {
            m_Looplistview.InitListView(-1, OnGetItemByIndex);
            LoadViewHelper tmp_LoadingViewHelper = _notif.param as LoadViewHelper;
            if (tmp_LoadingViewHelper.m_Action != null)
                tmp_LoadingViewHelper.m_Action.Invoke();
        }

        /// <summary>
        /// 通过索引获取列表单元
        /// </summary>
        /// <param name="_listview"></param>
        /// <param name="_index"></param>
        /// <returns></returns>
        private LoopListViewItem OnGetItemByIndex(LoopListView _listview, int _index)
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

            _index = _index % DataSource.Instance.m_Totalitemcount;
            if (_index < 0)
            {
                _index = DataSource.Instance.m_Totalitemcount + _index;
            }

            PageItem tmp_Itemdata = DataSource.Instance.GetItemDataByIndex(_index);
            tmp_itemscript.SetItemData(tmp_Itemdata, _index);
            return tmp_Item;
        }


        private void OnLoopListViewFinished(LoopListView _listview, LoopListViewItem _item)
        {
            m_Looplistview.RefreshAllShownItemWithFirstIndex(0);
        }
    }

}

