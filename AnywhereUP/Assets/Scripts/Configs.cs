using UnityEngine;

public class Configs
{

    private static Configs m_Instance;
    public static Configs GetConfigs
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new Configs();
            return m_Instance;
        }
    }



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
            return "https://aw.weacw.com/anywhere";
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
#if !UNITY_EDITOR
            return Application.persistentDataPath + "/cache/";
#elif UNITY_EDITOR
            return Application.streamingAssetsPath + "/cache/";
#endif
        }
    }

    /// <summary>
    /// oss资源地址
    /// </summary>
    public string m_OSSURI
    {
        get
        {
            return "https://oss.weacw.com/";
        }
    }

}
