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
using Anywhere.Net;
using Aliyun.OSS;
using System.IO;

namespace Anywhere.UI
{

    public class DatasourceMgr : Singleton<DatasourceMgr>
    {
        List<PageItem> m_Itemdatalist;
        Dictionary<int, Sprite> ItemBackgroundDic;//<id , 背景图>

        void Start()
        {
            Init();
        }

        public void Init()
        {
            m_Itemdatalist = new List<PageItem>();
            ItemBackgroundDic = new Dictionary<int, Sprite>();
        }

        /// <summary>
        /// 根据item索引取item
        /// </summary>
        /// <param name="_index"></param>
        /// <returns></returns>
        public PageItem GetItemDataByIndex(int _index)
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
        public PageItem GetItemDataById(int _itemid)
        {
            int tmp_Count = m_Itemdatalist.Count;
            for (int i = 0; i < tmp_Count; i++)
            {
                if (m_Itemdatalist[i].id == _itemid)
                {
                    return m_Itemdatalist[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 根据ItemData的place取item
        /// </summary>
        /// <param name="_itemid"></param>
        /// <returns></returns>
        public PageItem GetItemDataByPlace(string _place, out int index)
        {
            int tmp_Count = m_Itemdatalist.Count;
            for (int i = 0; i < tmp_Count; i++)
            {
                if (m_Itemdatalist[i].place == _place)
                {
                    index = i;
                    return m_Itemdatalist[i];
                }
            }
            index = 0;
            return null;
        }

        /// <summary>
        /// 数据个数
        /// </summary>
        public int m_Totalitemcount
        {
            get
            {
                return m_Itemdatalist.Count;
            }
        }

        /// <summary>
        /// 存储分页数据
        /// </summary>
        /// <param name="_itemarray"></param>
        public void SaveData(PageItem[] _itemarray)
        {
            m_Itemdatalist = new List<PageItem>(_itemarray);
            DownItemBackground();
        }

        public void AddDatas(PageItem[] _items)
        {
            m_Itemdatalist.AddRange(_items);
            DownItemBackground();
            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.NET_SEARCHPAGE);
        }

        /// <summary>
        /// 下载分页背景
        /// </summary>
        private void DownItemBackground()
        {
            Loom.RunAsync(() =>
            {
                bool wasCreated = false;
                foreach (PageItem _item in m_Itemdatalist)
                {
                    //GetObject.SyncGetObject(UIConst.m_BUCKETNAME, _item.thumbnailName + ".png");
                    //Texture2D tmp_tex2d = GetIcon(_item, 10, 10);
                    //Sprite tmp_Sprite = Sprite.Create(tmp_tex2d, new Rect(0, 0, tmp_tex2d.width, tmp_tex2d.height), new Vector2(0, 0));
                    //tmp_Sprite.name = _item.thumbnailName;
                    //ItemBackgroundDic.Add(_item.id, tmp_Sprite);



                    GetObject.SyncGetObject(UIConst.m_BUCKETNAME, _item.thumbnailName + ".png");
                    Loom.QueueOnMainThread((parm) =>
                    {
                        Texture2D tmp_tex2d = GetIcon(_item, 10, 10);
                        Sprite tmp_Sprite = Sprite.Create(tmp_tex2d, new Rect(0, 0, tmp_tex2d.width, tmp_tex2d.height), new Vector2(0, 0));
                        tmp_Sprite.name = _item.thumbnailName;
                        ItemBackgroundDic.Add(_item.id, tmp_Sprite);
                        if (!wasCreated)
                            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.NET_GETALLPAGEINFO);
                        wasCreated = true;
                    }, null);
                }
            });
        }

        private Texture2D GetIcon(PageItem _item, int _t2dwith, int _t2dheight)
        {
            byte[] m_T2dbyts = File.ReadAllBytes(Path.Combine(Config.DirToDownload, _item.thumbnailName + ".png"));
            Texture2D m_T2d = new Texture2D(_t2dwith, _t2dheight);
            m_T2d.LoadImage(m_T2dbyts);
            return m_T2d;
        }

        public Sprite GetItemBackgroundById(int _id)
        {
            if (ItemBackgroundDic.ContainsKey(_id))
            {
                return ItemBackgroundDic[_id];
            }
            return null;
        }


        #region 测试功能

        /// <summary>
        /// 获取数据(测试）
        /// </summary>
        void DoRefreshDataSource()
        {
            m_Itemdatalist.Clear();
            for (int i = 0; i < 10; i++)
            {
                PageItem tmp_Pageitem = new PageItem();
                tmp_Pageitem.id = i;
                tmp_Pageitem.descript = "这是描述：随机数" + Random.Range(0, 20);
                tmp_Pageitem.place = "位置" + Random.Range(0, 20);
                m_Itemdatalist.Add(tmp_Pageitem);
            }
        }

        #endregion


    }

}
