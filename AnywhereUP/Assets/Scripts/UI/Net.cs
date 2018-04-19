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



namespace Anywhere.UI
{
     [System.Serializable]
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
                PageItem item = JsonUtility.FromJson<PageItem>(www.text);
                //Debug.Log("<color=green> Page </color> Request:" + www.text);
                Debug.Log(item.id);
            }
        }


        //查询信息（根据地址）


        #endregion
    }
}