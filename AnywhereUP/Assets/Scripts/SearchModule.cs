using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Anywhere.Net;
using Anywhere.UI;
using SuperScrollView;
using UnityEngine;

namespace Anywhere
{
	[CreateAssetMenu(menuName="Anywhere/Search")]
    public class SearchModule : BaseModule
    {
        public LoopListView m_Looplistview;
        public void Search(string _keyword)
        {
            //本地检索			
            PageItem tmp_Item = null;
            int index = 0;
            tmp_Item = DatasourceMgr.Instance.GetItemDataByPlace(_keyword, out index);
            if (tmp_Item != null)
            {
                SearchResultsHelper resultsHelper = new SearchResultsHelper() { m_Index = index };
                NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.OPERATER_SEARCH, resultsHelper);
            }
            else //本地没有 网络检索
            {
                Loom.RunAsync(() =>
                {
                    new System.Threading.Thread(() =>
                    {
                        HttpRequestHelper tmp_HttpRequestHelper = new HttpRequestHelper();
                        tmp_HttpRequestHelper.m_URI = Configs.GetConfigs.m_SearchInfoHost + _keyword;
                        tmp_HttpRequestHelper.m_TimeOut = 30000;
                        // Debug.Log(tmp_HttpRequestHelper.m_URI);
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
                                NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.NET_SEARCHPAGE);
                            };
                            if (tmp_SaveDataHelper.m_PageItemArray == null || tmp_SaveDataHelper.m_PageItemArray.Length == 0) return;
                            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.HTTP_GETPAGEITEM, tmp_SaveDataHelper);
                        };
                        NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.HTTP_GETREQUEST, tmp_HttpRequestHelper);
                    }).Start();
                });
            }
        }
    }
}