using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anywhere
{
    //#用于访问Transform的组建
    [CreateAssetMenu(menuName = "Anywhere/Varuables/Transform Variable")]
    public class TransformVariable : ScriptableObject
    {
        //访问的Transform对象容器
        public Transform m_Transform;
    }
}
