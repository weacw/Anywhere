/*
*	Function:
*		列表显示数据管理
*		
*	Author:
*		Jeno
*		
*/

using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;

namespace Anywhere.UI
{

    public class DataSource : Singleton<DataSource>
    {
        private int index = 0;
        [SerializeField] private List<PageItem> m_Itemdatalist;
        public Dictionary<int, Sprite> ItemBackgroundDic;//<id , 背景图>
        private bool wasCreated = false;
        private string path = null;
        private Thread thread;

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

        void Start()
        {
            m_Itemdatalist = new List<PageItem>();
            ItemBackgroundDic = new Dictionary<int, Sprite>();

            path = Configs.GetConfigs.m_CachePath;

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
            try
            {
                PageItem tmp_PageItem = m_Itemdatalist.Find((item) => item.place.Equals(_place));
                int i = m_Itemdatalist.FindIndex((item) => item.place.Equals(_place));
                index = i;
                return tmp_PageItem;
            }
            catch (System.Exception ex)
            {
                index = -1;
                Debug.LogError(ex.Message);
                return null;
            }
        }

        public void RefreshDatas()
        {
            index=0;
            m_Itemdatalist.Clear();
            ItemBackgroundDic.Clear();
        }


        /// <summary>
        /// 创建item
        /// </summary>
        /// <param name="_notif"></param>
        public void GetPageItem(Notification _notif)
        {
            HttpSaveDataHelper helper = _notif.param as HttpSaveDataHelper;
            //存储服务器返回的数据
            m_Itemdatalist.AddRange(helper.m_PageItemArray);
            for (var i = index; i < m_Itemdatalist.Count; i++)
            {
                MultThreadSetupThumbnials(i, helper.m_Action);
                index++;
            }
        }

        /// <summary>
        /// 多线程下载Thumbnail
        /// </summary>
        /// <param name="index"></param>
        private void MultThreadSetupThumbnials(int index, System.Action _callback)
        {
            HttpRequestHelper helpr = new HttpRequestHelper()
            {
                m_URI = Configs.GetConfigs.m_OSSURI + m_Itemdatalist[index].thumbnailName + ".png",
                m_LocalPath = path,
                m_Succeed = (json) =>
                 {
                     Texture2D tmp_tex2d = GetIcon(m_Itemdatalist[index], 10, 10);
                     Sprite tmp_Sprite = Sprite.Create(tmp_tex2d, new Rect(0, 0, tmp_tex2d.width, tmp_tex2d.height), new Vector2(0, 0));
                     tmp_Sprite.name = m_Itemdatalist[index].thumbnailName;
                     if (!ItemBackgroundDic.ContainsKey(m_Itemdatalist[index].thumbnailName.GetHashCode()))
                         ItemBackgroundDic.Add(m_Itemdatalist[index].thumbnailName.GetHashCode(), tmp_Sprite);

                     if (_callback != null) _callback.Invoke();

                     //下载到列表最后一个时才进行生成
                     if (index == m_Itemdatalist.Count - 1 && !wasCreated)
                     {
                         NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.HTTP_GETALLPAGEINFO, new LoadViewHelper()
                         {
                             //加载完毕，关闭Loading界面
                             m_Action = () => NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.UI_SHOWHIDELOADING, new UICtrlHelper() { m_State = false })
                         });
                         wasCreated = true;
                     }
                 },
                m_TimeOut = 30000
            };
            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.HTTP_DOWNLOADFILE, helpr);
        }

        private Texture2D GetIcon(PageItem _item, int _t2dwith, int _t2dheight)
        {
            byte[] m_T2dbyts = File.ReadAllBytes(Path.Combine(Configs.GetConfigs.m_CachePath, _item.thumbnailName + ".png"));
            Texture2D m_T2d = new Texture2D(_t2dwith, _t2dheight);
            m_T2d.Compress(false);
            m_T2d.Apply(false);
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
