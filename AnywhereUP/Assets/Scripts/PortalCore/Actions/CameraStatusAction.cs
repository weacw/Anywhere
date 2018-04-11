using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anywhere
{
    //#ARKit相机位置计算
    [CreateAssetMenu(menuName = "Anywhere/Actions/CameraStatus Action")]
    public class CameraStatusAction : BaseAction
    {
        public TransformVariable m_Device;
        public override void Execute(PortalActionManager _hook)
        {
            Vector3 worldPos = m_Device.m_Transform.position + m_Device.m_Transform.forward * Camera.main.nearClipPlane;
            Vector3 pos = _hook.transform.InverseTransformPoint(worldPos);
            _hook.m_CameraPos = pos;
        }
    }
}
