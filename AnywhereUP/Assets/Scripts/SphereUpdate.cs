using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Anywhere
{
    public class SphereUpdate : MonoBehaviour
    {
        public Transform m_Target;
        private Transform m_Self;

        private void Start()
        {
            m_Self = transform;
        }

        public void Update()
        {
            if (m_Self == null || m_Target == null) return;
            m_Self.position = Vector3.Lerp(m_Self.position, m_Target.position, Time.deltaTime * 50);
        }

    }
}
