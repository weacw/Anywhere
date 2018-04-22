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
    [Serializable]
    public class PageItem
    {
        public int id;//索引
        public string place;//位置
        public string type;//资源种类
        public string descript;//描述
        public string version;//版本
        public string assetName;//资源名称
        public string thumbnailName;//缩列图名称
        public PageItem()
        {
            id = 0;
            place = "";
            type = "";
            descript = "";
            version = "";
            assetName = "";
            thumbnailName = "";
        }
    }

    public class NetHttp : Singleton<NetHttp>
    {
        private int m_PageNum = 1;

        #region Get
        //获取分页信息
        public void GetPageInfo()
        {
            StartCoroutine(Getpageinfo());
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
            getUrl.Append(UIConst.Host).Append("getinfo.php?page=" + m_PageNum);
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
                DatasourceMgr.Instance.SaveData(tmp_Itemarray);
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
            getUrl.Append(UIConst.Host).Append("searchinfo.php?search=").Append(_place);
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
                    DatasourceMgr.Instance.AddDatas(tmp_Itemarray);
                }
            }
        }

        #endregion
    }
}