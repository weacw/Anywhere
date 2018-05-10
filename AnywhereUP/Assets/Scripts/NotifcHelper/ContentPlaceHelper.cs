using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using UnityEngine.SceneManagement;

public class ContentPlaceHelper : EventArgs
{
    public Vector3 m_ContentPos;
    public Quaternion m_ContentRot;
}

public class ContentSetupHelper : EventArgs
{
    public GameObject m_Content;
    public bool m_IsSceneStream;
    public Scene m_Scene;
}