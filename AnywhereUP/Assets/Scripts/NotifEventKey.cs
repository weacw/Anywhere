namespace Anywhere
{
    public enum NotifEventKey
    {
        /// <summary>
        /// 录屏开始
        /// </summary>
        EVERYPLAY_RECORDING_START,
        /// <summary>
        /// 录屏结束
        /// </summary>
        EVERYPLAY_RECORDING_STOP,
        /// <summary>
        /// 删除内容
        /// </summary>
        ASSETS_REMOVEALL,
        /// <summary>
        /// 加载ab
        /// </summary>
        ASSETS_LOAD,
        /// <summary>
        /// 实例化ab
        /// </summary>
        AB_INSTANCE,
        /// <summary>
        /// 播放视频
        /// </summary>
        ASSETS_VIDEOPLAY,
        /// <summary>
        /// 显示召唤按钮
        /// </summary>
        UI_SHOWCALLBTN,
        /// <summary>
        /// 隐藏召唤按钮
        /// </summary>
        UI_HIDECALLBTN,
        /// <summary>
        /// 设置提示
        /// </summary>
        UI_SETHINT,
        /// <summary>
        /// 放置内容
        /// </summary>
        OPERATER_PLACECONTENT,
        /// <summary>
        /// 设置内容的位置与focus位置相同
        /// </summary>
        OPERATER_SETFOCUSPOSTOCONTENT,
        /// <summary>
        /// 从服务器获取分页信息
        /// </summary>
        NET_GETALLPAGEINFO,
        /// <summary>
        /// 从服务检索完毕
        /// </summary>
        NET_SEARCHPAGE,
        /// <summary>
        /// AB下载中
        /// </summary>
        NET_ABDOWNLOADING,
        /// <summary>
        /// AB下载完成
        /// </summary>
        NET_ABDOWNLOADCOMPLETE,
        /// <summary>
        /// 关闭ARKit   
        /// </summary>
        ARKIT_PAUSE,
        /// <summary>
        /// 开启ARKit
        /// </summary>
        ARKIT_PLAY,
        /// <summary>
        /// 开启Focus
        /// </summary>
        ARKIT_FOCUS_ON,
        /// <summary>
        /// 关闭Focus
        /// </summary>        
        ARKIT_FOCUS_OFF,
        /// <summary>
        /// 删除锚点
        /// </summary>        
        ARKIT_DESTORYARANCHOR,
        /// <summary>
        /// 创建锚点
        /// </summary>        
        ARKIT_CREATEARANCHOR,
        /// <summary>
        ///分享
        /// </summary> 
        SOCIAL_SHARE,
        /// <summary>
        ///Http 请求
        /// </summary> 
        HTTP_GETREQUEST,
        /// <summary>
        ///http 下载
        /// </summary> 
        HTTP_DOWNLOADFILE,
        /// <summary>
        ///存储数据
        /// </summary> 
        HTTP_SAVEDATA
    }
}