﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class HttpRequestHelper : EventArgs
{
    public string m_URI;
    public int m_TimeOut;
    public Action<string> m_Succeed;
    public Action m_Failed;
}
