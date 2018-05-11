using UnityEngine;
using System;
public class VideoPlayerHelper : EventArgs
{
    public string m_Videoname = null;
    public Action m_BeginInstance;
    public Action m_EndIntance;
}
