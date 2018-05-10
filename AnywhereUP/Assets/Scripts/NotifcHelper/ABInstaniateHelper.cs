using System;
public class ABInstaniateHelper : EventArgs
{
    public string m_ABName;
    public string m_AssetName;
    public Action m_BeginInstance;
    public Action m_EndIntance;
}