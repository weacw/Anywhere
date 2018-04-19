/*
*	Function:
*		列表显示数据管理
*		
*	Author:
*		Jeno
*		
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperScrollView;

namespace Anywhere.UI
{

    public class ItemData
    {
        public int m_Id;//Data索引
        public string m_Spritename;//图片名称
        public string m_Location;//位置
        public string m_Des;//描述
    }

    public class DatasourceMgr : MonoBehaviour
    {

        List<ItemData> m_Itemdatalist = new List<ItemData>();
        static DatasourceMgr m_INSTANCE = null;

        public static DatasourceMgr Get
        {
            get
            {
                if (m_INSTANCE == null)
                {
                    m_INSTANCE = Object.FindObjectOfType<DatasourceMgr>();
                }
                return m_INSTANCE;
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

        void Update()
        {

        }

        /// <summary>
        /// 根据item索引取item
        /// </summary>
        /// <param name="_index"></param>
        /// <returns></returns>
        public ItemData GetItemDataByIndex(int _index)
        {
            if (_index < 0 || _index >= m_Itemdatalist.Count)
            {
                Debug.LogError("can't GetItemDataByIndex index is:" + _index);
                return null;
            }
            return m_Itemdatalist[_index];
        }

        /// <summary>
        /// 根据ItemData的Id取item
        /// </summary>
        /// <param name="_itemid"></param>
        /// <returns></returns>
        public ItemData GetItemDataById(int _itemid)
        {
            int tmp_Count = m_Itemdatalist.Count;
            for (int i = 0; i < tmp_Count; ++i)
            {
                if (m_Itemdatalist[i].m_Id == _itemid)
                {
                    return m_Itemdatalist[i];
                }
            }
            return null;
        }

        
        public int m_Totalitemcount
        {
            get
            {
                return m_Itemdatalist.Count;
            }
        }

        #region 测试功能

        int m_Datacount = 10;//数据条数 （测试）
        /// <summary>
        /// 获取数据(测试）
        /// </summary>
        void DoRefreshDataSource()
        {
            m_Itemdatalist.Clear();
            for (int i = 0; i < m_Datacount; i++)
            {
                ItemData tmp_Itemdata = new ItemData();
                tmp_Itemdata.m_Id = i;
                tmp_Itemdata.m_Des = "这是描述：随机数" + Random.Range(0, 20);
                tmp_Itemdata.m_Location = "位置：" + Random.Range(0, 20);
                m_Itemdatalist.Add(tmp_Itemdata);
            }
        }

        #endregion


    }

}
