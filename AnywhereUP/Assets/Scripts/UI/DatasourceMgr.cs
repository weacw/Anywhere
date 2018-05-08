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
using System.Threading;

namespace Anywhere.UI
{

    public class DatasourceMgr : Singleton<DatasourceMgr>
    {
        private List<PageItem> m_Itemdatalist;
        private Dictionary<int, Sprite> ItemBackgroundDic;//<id , 背景图>
        private bool wasCreated = false;
        private string path = null;
        private Thread thread;
        void Start()
        {
            m_Itemdatalist = new List<PageItem>();
            ItemBackgroundDic = new Dictionary<int, Sprite>();

            path = Configs.GetConfigs.m_CachePath;

            NotifCenter.GetNotice.AddEventListener(NotifEventKey.HTTP_SAVEDATA, SaveData);
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
            return m_Itemdatalist.Find((item) => item.id.Equals(_itemid));
        }

        /// <summary>
        /// 根据ItemData的place取item
        /// </summary>
        /// <param name="_itemid"></param>
        /// <returns></returns>
        public PageItem GetItemDataByPlace(string _place, out int index)
        {
            PageItem tmp_PageItem = m_Itemdatalist.Find((item) => item.place.Equals(_place));
            int i = m_Itemdatalist.FindIndex((item) => item.place.Equals(_place));
            index = i;
            return tmp_PageItem;
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
        public void SaveData(Notification _notif)
        {
            HttpSaveDataHelper helper = _notif.param as HttpSaveDataHelper;
            m_Itemdatalist.AddRange(helper.m_PageItemArray);
            DownItemBackground();
            if (helper.m_Action != null)
                helper.m_Action.Invoke();
        }

        /// <summary>
        /// 下载分页背景
        /// </summary>
        private void DownItemBackground()
        {
            Loom.RunAsync(() =>
            {
                thread = new Thread(RunThread);
                thread.Start();
            });
        }
        private void OnDisable()
        {
            if (thread != null)
                thread.Abort();
        }

        private void RunThread()
        {
            if (wasCreated) return;
            foreach (PageItem _item in m_Itemdatalist)
            {
                HttpRequestHelper helpr = new HttpRequestHelper()
                {
                    m_URI = Configs.GetConfigs.m_OSSURI + _item.thumbnailName + ".png",
                    m_LocalPath = path,
                    m_Succeed = (json) =>
                     {
                         Texture2D tmp_tex2d = GetIcon(_item, 10, 10);
                         Sprite tmp_Sprite = Sprite.Create(tmp_tex2d, new Rect(0, 0, tmp_tex2d.width, tmp_tex2d.height), new Vector2(0, 0));
                         tmp_Sprite.name = _item.thumbnailName;
                         ItemBackgroundDic.Add(_item.id, tmp_Sprite);
                         if (!wasCreated && _item.id == m_Itemdatalist.Count)
                         {
                             NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.NET_GETALLPAGEINFO);
                             wasCreated = true;
                             if (thread != null) thread.Abort();
                             UIManager.Instance.m_LoadingScreen.SetActive(false);
                         }
                     },
                    m_TimeOut = 30000
                };
                NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.HTTP_DOWNLOADFILE, helpr);
            }
        }

        private Texture2D GetIcon(PageItem _item, int _t2dwith, int _t2dheight)
        {
            byte[] m_T2dbyts = File.ReadAllBytes(Path.Combine(Configs.GetConfigs.m_CachePath, _item.thumbnailName + ".png"));
            Texture2D m_T2d = new Texture2D(_t2dwith, _t2dheight);
            m_T2d.LoadImage(m_T2dbyts);
            return m_T2d;
        }

        public Sprite GetItemBackgroundById(int _id)
        {
            if (ItemBackgroundDic.ContainsKey(_id))
                return ItemBackgroundDic[_id];
            return null;
        }
    }
}
