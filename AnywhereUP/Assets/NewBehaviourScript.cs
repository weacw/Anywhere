using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Configs m_Configs;
    public Anywhere.HttpRequest m_HttpRequset;
    public void Start()
    {
        // HttpRequset rq = new HttpRequset();
        Anywhere.NotifCenter.GetNotice.AddEventListener(Anywhere.NotifEventKey.HTTP_GETREQUEST, m_HttpRequset.GetHttpResponse);
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Request"))
        {
            HttpRequestHelper helper = new HttpRequestHelper()
            {
                m_TimeOut = 30000,
                m_URI = m_Configs.m_GetInfoHost + 1,
                m_Succeed = (x) =>
                {
                    // Debug.Log("Succeed:\n" + x);
                    // Debug.Log(FindObjectOfType<Camera>().name);

                    Loom.QueueOnMainThread((parm) =>
                    {
                        Debug.Log("Succeed:\n" + x);
                        Debug.Log(FindObjectOfType<Camera>().name);
                    }, null);
                }
            };
            // Anywhere.NotifCenter.GetNotice.PostDispatchEvent(Anywhere.NotifEventKey.HTTP_GETREQUEST, helper);
            Loom.RunAsync(() =>
            {
                Anywhere.NotifCenter.GetNotice.PostDispatchEvent(Anywhere.NotifEventKey.HTTP_GETREQUEST, helper);
            });
        }
    }
}
