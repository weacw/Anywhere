/*
*	Function:
*		UI面板服务器数据交互
 *		    20180417目前只涉及到面板信息
*		
*	Author:
*		Jeno
*{"id":"3",
 *"place":"beijing",
 *"type":"ab",
 *"descript":"bj",
 *"version":"11",
 *"assetName":"assets",
 *"thumbnailName":"PortalNormal"}
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
    }

     [Serializable]
     public class Test1
     {
         public int id;
     }



    public class Net : MonoBehaviour
    {

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                StartCoroutine(GetPageInfo());
            }
        }

        #region Get

        //分页信息（拉取10条）
        IEnumerator GetPageInfo()
        {
            StringBuilder getUrl = new StringBuilder();
            getUrl.Append(UIConst.Host).Append("getinfo.php");
            WWW www = new WWW(getUrl.ToString());
            yield return www;
            if (www.error != null)
            {
                Debug.LogError("Error is :" + www.error);
            }
            else
            {
                Debug.Log("<color=green> Page </color> Request:" + www.text);

                //string tmp_str1 = "{\"id\":\"1\"},{\"id\":\"2\"}";
                //string newJson = "{ \"Items\": [" + tmp_str1 + "]}";
                //Test1[] tmp_Itemarray = JsonHelper.FromJson<Test1>(newJson);
                //Debug.Log(tmp_Itemarray[0].id + "  " + tmp_Itemarray[1].id);
                
                //string newJson=www.text.Replace("}","},");
                PageItem[] tmp_Itemarray = JsonHelper.FromJson<PageItem>(www.text);
                foreach (PageItem item in tmp_Itemarray)
                {
                    Debug.Log(item.id + item.thumbnailName);
                }

            }
        }


        //查询信息（根据地址）


        #endregion
    }
}