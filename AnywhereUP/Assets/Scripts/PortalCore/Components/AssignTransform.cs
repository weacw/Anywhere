using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anywhere
{
    //#用于访问Transform的组建
    public class AssignTransform : MonoBehaviour
    {
        //Transform 容器
        public TransformVariable variable;
        private void Awake()
        {
            variable.m_Transform = this.transform;
        }
    }
}