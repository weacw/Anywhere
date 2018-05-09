using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Anywhere.Net;
using Anywhere.UI;
using SuperScrollView;
using UnityEngine;

namespace Anywhere
{
    [CreateAssetMenu(menuName = "Anywhere/AppModules/Search")]
    public class SearchModule : BaseModule
    {
        public LoopListView m_Looplistview;
        public void Search(Notification _notif)
        {
            if (m_Looplistview == null) m_Looplistview = FindObjectOfType<LoopListView>();


            PageItem tmp_Item = null;
            int index = 0;

            SearchHelper tmp_SearchHelper = _notif.param as SearchHelper;
            //本地检索			        
            tmp_Item = DataSource.Instance.GetItemDataByPlace(tmp_SearchHelper.m_Keywords, out index);
            if (tmp_Item != null)
            {
                GoToSearchResultById(index);
            }
            else
            {
                //网络检索
                Loom.RunAsync(() =>
                {
                    new Thread(() =>
                    {
                        HttpRequestHelper tmp_HttpRequestHelper = new HttpRequestHelper();
                        tmp_HttpRequestHelper.m_URI = Configs.GetConfigs.m_SearchInfoHost + tmp_SearchHelper.m_Keywords;
                        tmp_HttpRequestHelper.m_TimeOut = 30000;

                        tmp_HttpRequestHelper.m_Succeed = (json) =>
                        {
                            if (json.Contains("null")) return;
                            PageItem[] tmp_Itemarray = JsonHelper.FromJson<PageItem>(json);
                            if (tmp_Itemarray.Length <= 0)
                            {
                                Debug.Log("Not found");
                                return;
                            }
                            HttpSaveDataHelper tmp_SaveDataHelper = new HttpSaveDataHelper();
                            tmp_SaveDataHelper.m_PageItemArray = tmp_Itemarray;
                            tmp_SaveDataHelper.m_Action = () =>
                            {
                                tmp_Item = DataSource.Instance.GetItemDataByPlace(tmp_SearchHelper.m_Keywords, out index);
                                GoToSearchResultById(index);
                            };
                            if (tmp_SaveDataHelper.m_PageItemArray == null || tmp_SaveDataHelper.m_PageItemArray.Length == 0) return;
                            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.HTTP_GETPAGEITEM, tmp_SaveDataHelper);
                        };
                        NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.HTTP_GETREQUEST, tmp_HttpRequestHelper);
                    }).Start();
                });
            }
        }

        private void GoToSearchResultById(int _index)
        {
            //单元数据id从1开始 数据存储索引从0开始,这里是用索引跳转
            m_Looplistview.MovePanelToItemIndex(_index, 0);
        }
    }
}