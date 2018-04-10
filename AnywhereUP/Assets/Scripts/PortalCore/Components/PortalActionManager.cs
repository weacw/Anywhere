using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anywhere
{
    public class PortalActionManager : MonoBehaviour
    {
        internal bool m_IsColliding;
        internal bool m_WasInFront;
        internal bool m_IsInFront;
        internal bool m_InOtherWorld;
        internal bool m_Fullrenderer;
        internal Vector3 m_CameraPos;

        public ActionBuilder m_ActionBuilder;

        private void Awake()
        {
            m_Fullrenderer = false;
            Trigger("ShaderAction");
        }
        private void Update()
        {
            if (m_ActionBuilder != null) Tick();
        }
        private void Tick()
        {
            for (var i = 0; i < m_ActionBuilder.m_TickList.Count; i++)
            {
                m_ActionBuilder.m_TickList[i].Execute(this);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("MainCamera"))
                return;
            Trigger("CameraStatusAction");
            m_WasInFront = GetIsInFront();
            m_IsColliding = true;
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("MainCamera"))
                return;
            m_IsColliding = false;
        }
        private void OnDestroy()
        {
            m_Fullrenderer = true;
            Trigger("ShaderAction");
        }

        internal void Trigger(string _methodKey)
        {
            m_ActionBuilder.m_TriggerList.Find((action) => action.name == _methodKey).Execute(this);
        }

        internal bool GetIsInFront()
        {
            return m_CameraPos.z >= 0 ? true : false;
        }
    }
}