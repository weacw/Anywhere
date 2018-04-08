using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    public bool m_DontDestroyOnLoad = false;
    public static bool m_IsInit=false;
    //线程锁
    private static readonly object syslock = new object();
    private static T _Instance;
    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                lock (syslock)
                {
                    _Instance = FindObjectOfType<T>();
                    if (_Instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.hideFlags = HideFlags.HideAndDontSave;
                        _Instance = obj.AddComponent<T>();              
                    }
                    m_IsInit = true;
                }
            }
            return _Instance;
        }
    }

    public virtual void Awake()
    {
        if (m_DontDestroyOnLoad)
            DontDestroyOnLoad(this.gameObject);
    }
}
