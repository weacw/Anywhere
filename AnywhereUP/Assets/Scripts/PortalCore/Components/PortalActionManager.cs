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

        //进入传送门
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("MainCamera"))
                return;
            Trigger("CameraStatusAction");
            m_WasInFront = GetIsInFront();
            m_IsColliding = true;
        }

        //退出传送门
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

        //触发ARKit相机与传送们的操作
        //_methodKey：要执行的方法名称
        internal void Trigger(string _methodKey)
        {
            m_ActionBuilder.m_TriggerList.Find((action) => action.name == _methodKey).Execute(this);
        }

        //判断当前ARKit相机是否处于传送门的前方
        //True:处于前方
        //False:处于后方
        internal bool GetIsInFront()
        {
            return m_CameraPos.z >= 0 ? true : false;
        }
    }
}