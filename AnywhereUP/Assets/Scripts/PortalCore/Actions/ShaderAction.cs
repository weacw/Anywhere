﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
namespace Anywhere
{
    //#用于操作Shader实现传送门渲染功能
    [CreateAssetMenu(menuName = "Anywhere/Actions/Shader Action")]
    public class ShaderAction : BaseAction
    {
        public override void Execute(PortalActionManager _hook)
        {
            var stencilTest = _hook.m_Fullrenderer ? CompareFunction.NotEqual : CompareFunction.Equal;
            Shader.SetGlobalInt("_StencilTest", (int)stencilTest);
        }
    }
}