using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ContentPlaceHelper : EventArgs
{
    public Vector3 m_ContentPos;
    public Quaternion m_ContentRot;
}

public class ContentSetupHelper : EventArgs
{
    public GameObject m_Content;
}