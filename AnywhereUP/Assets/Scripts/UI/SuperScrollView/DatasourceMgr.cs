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

namespace Anywhere.UI
{

    public class ItemData
    {
        public int mId;//Data索引
        public string mSpriteName;//图片名称
        public string mLocation;//位置
        public string mDes;//描述
    }

    public class DatasourceMgr : MonoBehaviour
    {

        List<ItemData> mItemDataList = new List<ItemData>();
        static DatasourceMgr instance = null;


        public static DatasourceMgr Get
        {
            get
            {
                if (instance == null)
                {
                    instance = Object.FindObjectOfType<DatasourceMgr>();
                }
                return instance;
            }
        }

        void Awake()
        {
            Init();
        }

        public void Init()
        {
            DoRefreshDataSource();
        }

        /// <summary>
        /// 根据item索引取item
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ItemData GetItemDataByIndex(int index)
        {
            if (index < 0 || index >= mItemDataList.Count)
            {
                Debug.LogError("can't GetItemDataByIndex index is:" + index);
                return null;
            }
            return mItemDataList[index];
        }

        /// <summary>
        /// 根据ItemData的Id取item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public ItemData GetItemDataById(int itemId)
        {
            int count = mItemDataList.Count;
            for (int i = 0; i < count; ++i)
            {
                if (mItemDataList[i].mId == itemId)
                {
                    return mItemDataList[i];
                }
            }
            return null;
        }

        
        public int TotalItemCount
        {
            get
            {
                return mItemDataList.Count;
            }
        }

        //public void SetDataTotalCount(int count)
        //{
        //    mTotalDataCount = count;
        //    DoRefreshDataSource();
        //}


        int DataCount = 10;//数据条数 （测试）
        /// <summary>
       /// 获取数据(测试）
       /// </summary>
        void DoRefreshDataSource()
        {
            mItemDataList.Clear();
            for (int i = 0; i < DataCount; i++)
            {
                ItemData itemData = new ItemData();
                itemData.mId = i;
                itemData.mDes = "这是描述：随机数" + Random.Range(0, 20);
                itemData.mLocation = "位置：" + Random.Range(0, 20);
                mItemDataList.Add(itemData);
            }
        }

    }

}
