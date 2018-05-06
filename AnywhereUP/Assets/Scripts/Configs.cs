using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Anywhere/Configs")]
public class Configs : ScriptableObject
{
    /// <summary>
    /// ARKit开启点云HUD设置
    /// </summary>
    public bool m_EnabledCloudPoints = true;

    /// <summary>
    /// ARKit开启平面HUD设置
    /// </summary>
    public bool m_EnabledPlanes = true;

    /// <summary>
    /// 服务器地址
    /// </summary>
    public string m_Host
    {
        get
        {
            return "https://aw.weacw.com/anywehre";
        }
    }

    /// <summary>
    /// 获取相关信息的服务器地址
    /// </summary>
    public string m_GetInfoHost
    {
        get
        {
            return m_Host + "/getinfo.php?page=";
        }
    }

    /// <summary>
    /// 搜索相关信息的服务器地址
    /// </summary>
    public string m_SearchInfoHost
    {
        get
        {
            return m_Host + "/searchinfo.php?search=";
        }
    }

    /// <summary>
    /// 缓存地址
    /// </summary>
    public string m_CachePath
    {
        get
        {
            return Application.persistentDataPath + "/cache/";
        }
    }
}
