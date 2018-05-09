﻿using System.Collections;
using System.Collections.Generic;
using Anywhere.Net;
using Anywhere.UI;
using UnityEngine;

namespace Anywhere
{
    [CreateAssetMenu(menuName = "Anywhere/Http/ThumbnialDownloader")]
    public class ThumbnialDownloader : BaseModule
    {
        //private int m_PageIndex = 0;
        public void ThumbDownload()
        {
            int m_PageIndex = 0;
            HttpRequestHelper helper = new HttpRequestHelper()
            {
                m_URI = Configs.GetConfigs.m_GetInfoHost + m_PageIndex,
                m_Succeed = (json) =>
                {
                    HttpSaveDataHelper tmp_SaveDataHelper = new HttpSaveDataHelper();
                    tmp_SaveDataHelper.m_PageItemArray = (JsonHelper.FromJson<PageItem>(json));
                    NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.HTTP_GETPAGEITEM, tmp_SaveDataHelper);
                    m_PageIndex++;
                },
                m_TimeOut = 30000
            };
            NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.HTTP_GETREQUEST, helper);
        }

    }
}