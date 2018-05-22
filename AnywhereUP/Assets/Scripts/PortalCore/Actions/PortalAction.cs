using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
namespace Anywhere
{
    //#每一帧检测相机是否通过传送门，通过传送门开启次世界渲染，未通过则局部渲染
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
                NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.UI_SETHINTSTATES, new UICtrlHelper() { m_State = false });
            }
            _hook.m_WasInFront = _hook.m_IsInFront;
        }
    }
}