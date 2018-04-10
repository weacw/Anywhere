using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anywhere
{
    public class AssignTransform : MonoBehaviour
    {
        public TransformVariable variable;
        private void Awake()
        {
            variable.m_Transform = this.transform;
        }
    }
}