using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public Slider m_Slider;
    public Configs m_Configs;
    public Anywhere.HttpDownload m_HttpRequset;
    public void Start()
    {
        // HttpRequset rq = new HttpRequset();
        //Anywhere.NotifCenter.GetNotice.AddEventListener(Anywhere.NotifEventKey.HTTP_DOWNLOADFILE, m_HttpRequset.HttpDownloadFile);
        Anywhere.NotifCenter.GetNotice.AddEventListener(Anywhere.NotifEventKey.HTTP_DOWNLOADFILE, m_HttpRequset.HttpDownloadFile);
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Request"))
        {

            Loom.RunAsync(() =>
            {
                HttpRequestHelper h = new HttpRequestHelper()
                {
                    m_TimeOut = 30000,
                    m_URI = "https://oss.weacw.com/home.png",
                    m_LocalPath = @"G:\Unity\20180415Anywhere\AnywhereUP",
                    m_Downloading = ShowProgress

                };
                Anywhere.NotifCenter.GetNotice.PostDispatchEvent(Anywhere.NotifEventKey.HTTP_DOWNLOADFILE, h);
            });
            //HttpRequestHelper helper = new HttpRequestHelper()
            //{
            //    m_TimeOut = 30000,
            //    m_URI = m_Configs.m_GetInfoHost + 1,
            //    m_Succeed = (x) =>
            //    {
            //        // Debug.Log("Succeed:\n" + x);
            //        // Debug.Log(FindObjectOfType<Camera>().name);

            //        Loom.QueueOnMainThread((parm) =>
            //        {
            //            Debug.Log("Succeed:\n" + x);
            //            Debug.Log(FindObjectOfType<Camera>().name);
            //        }, null);
            //    }
            //};
            //// Anywhere.NotifCenter.GetNotice.PostDispatchEvent(Anywhere.NotifEventKey.HTTP_GETREQUEST, helper);
            //Loom.RunAsync(() =>
            //{
            //    Anywhere.NotifCenter.GetNotice.PostDispatchEvent(Anywhere.NotifEventKey.HTTP_GETREQUEST, helper);
            //});
        }
    }

    public void ShowProgress(float _pf)
    {
        m_Slider.value = _pf;
    }
}
