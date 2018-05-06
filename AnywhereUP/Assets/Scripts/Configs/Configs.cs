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
    public string m_Host = "https://aw.weacw.com";

    /// <summary>
    /// 获取相关信息的服务器地址
    /// </summary>
    public string m_GetInfoHost = "https://aw.weacw.com/anywhere/getinfo.php?";

    /// <summary>
    /// 搜索相关信息的服务器地址
    /// </summary>
    public string m_SearchInfoHost = "https://aw.weacw.com/anywhere/searchinfo.php?";
}
