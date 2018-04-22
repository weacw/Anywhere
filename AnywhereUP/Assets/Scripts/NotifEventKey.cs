﻿namespace Anywhere
{
    public enum NotifEventKey
    {
        EVERYPLAY_RECORDING_START,
        EVERYPLAY_RECORDING_STOP,     
        ASSETS_REMOVEALL,
        ASSETS_LOAD,
        AB_INSTANCE,
        ASSETS_VIDEOPLAY,
        UI_SHOWCALLBTN,
        UI_SETHINT,
        OPERATER_PLACECONTENT,
        OPERATER_SETFOCUSPOSTOCONTENT,
        NET_GETALLPAGEINFO,   //从服务器获取分页信息
        NET_SEARCHPAGE,       //从服务检索完毕
        NET_ABDOWNLOADING,    //AB下载中
        NET_ABDOWNLOADCOMPLETE      //AB下载完成
    }
}