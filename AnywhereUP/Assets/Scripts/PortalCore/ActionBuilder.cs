using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anywhere
{
    [CreateAssetMenu(menuName = "Anywhere/Action builder")]
    public class ActionBuilder : ScriptableObject
    {
        public List<BaseAction> m_TickList = new List<BaseAction>();
        public List<BaseAction> m_TriggerList = new List<BaseAction>();

    }
}