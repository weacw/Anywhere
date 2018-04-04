using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
namespace Anywhere
{
    [CreateAssetMenu(menuName = "Anywhere/Actions/Portal Action")]
    public class PortalAction : BaseAction
    {
        public TransformVariable m_Device;
        public override void Execute(PortalActionManager _hook)
        {
            if (!_hook.m_IsColliding) return;
            _hook.Trigger("CameraStatusAction");
            _hook.m_IsInFront = _hook.GetIsInFront();
            if ((_hook.m_IsInFront && !_hook.m_WasInFront) || (_hook.m_WasInFront && !_hook.m_IsInFront))
            {
                _hook.m_InOtherWorld = !_hook.m_InOtherWorld;
                _hook.m_Fullrenderer = _hook.m_InOtherWorld;
                _hook.Trigger("ShaderAction");
            }
            _hook.m_WasInFront = _hook.m_IsInFront;
        }
    }
}