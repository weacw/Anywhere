using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anywhere
{
    //#ARKit相机位置计算
    [CreateAssetMenu(menuName = "Anywhere/Actions/CameraStatus Action")]
    public class CameraStatusAction : BaseAction
    {
        private Transform m_Device;
        private void Awake()
        {
//#if !UNITY_EDITOR
            m_Device = GameObject.FindGameObjectWithTag("MainCamera").transform;
//#endif
        }
        public override void Execute(PortalActionManager _hook)
        {
            Vector3 worldPos = m_Device.position + m_Device.forward * Camera.main.nearClipPlane;
            Vector3 pos = _hook.transform.InverseTransformPoint(worldPos);
            _hook.m_CameraPos = pos;
        }
    }
}
