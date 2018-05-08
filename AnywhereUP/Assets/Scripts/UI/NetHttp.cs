/*
*	Function:
*		UI面板服务器数据交互
 *		    20180417目前只涉及到面板信息
*		
*	Author:
*		Jeno
*		
*/

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Anywhere.UI;
using System;

namespace Anywhere.Net
{
    public class NetHttp : Singleton<NetHttp>
    {
        private int m_PageNum = 1;

        #region Get
        //获取分页信息
        public void GetPageInfo()
        {
            // StartCoroutine(Getpageinfo());
            HttpRequestHelper helper = new HttpRequestHelper()
            {
                m_URI = Configs.GetConfigs.m_GetInfoHost + m_PageNum,
                m_Succeed = (json) =>
                {
                    Loom.QueueOnMainThread((parm) =>
                    {
                        PageItem[] tmp_Itemarray = JsonHelper.FromJson<PageItem>(json);
                        //DatasourceMgr.Instance.SaveData(tmp_Itemarray);
                        m_PageNum++;
                    }, null);
                },
                m_TimeOut = 30000
            };
            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.HTTP_GETREQUEST, helper);
        }
        //查询信息
        public void GetSerchInfo(string _palce)
        {
            StartCoroutine(Getsearchinfo(_palce));
        }

        //分页信息（拉取10条）
        IEnumerator Getpageinfo()
        {
            StringBuilder getUrl = new StringBuilder();
            getUrl.Append(UIConst.m_HOST).Append("getinfo.php?page=" + m_PageNum);
            WWW www = new WWW(getUrl.ToString());
            yield return www;
            if (www.error != null)
            {
                Debug.LogError("Error is :" + www.error);
            }
            else
            {
                //Debug.Log("<color=green> Page </color> Request:" + www.text);
                if (www.text.Contains("null")) yield return null;
                PageItem[] tmp_Itemarray = JsonHelper.FromJson<PageItem>(www.text);
                //DatasourceMgr.Instance.SaveData(tmp_Itemarray);
                m_PageNum++;
            }
        }


        /// <summary>
        /// 查询信息
        /// </summary>
        /// <param name="_place">地址</param>
        /// <returns></returns>
        IEnumerator Getsearchinfo(string _place)
        {
            StringBuilder getUrl = new StringBuilder();
            getUrl.Append(UIConst.m_HOST).Append("searchinfo.php?search=").Append(_place);
            WWW www = new WWW(getUrl.ToString());
            Debug.Log("<color=green> Search Url </color> Request:" + getUrl.ToString());
            yield return www;
            if (www.error != null)
            {
                Debug.LogError("Error is :" + www.error);
            }
            else
            {
                Debug.Log("<color=green> Search </color> Request:" + www.text);
                PageItem[] tmp_Itemarray = JsonHelper.FromJson<PageItem>(www.text);

                if (string.IsNullOrEmpty(tmp_Itemarray[0].place))//为空时
                {
                    Debug.LogError("can search the place!");
                }
                else//不为空
                {
                    HttpSaveDataHelper tmp_SaveDataHelper = new HttpSaveDataHelper();
                    tmp_SaveDataHelper.m_PageItemArray = tmp_Itemarray;
                    tmp_SaveDataHelper.m_Action = () => NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.NET_SEARCHPAGE);
                    NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.HTTP_SAVEDATA, tmp_SaveDataHelper);
                }
            }
        }

        #endregion
    }
}