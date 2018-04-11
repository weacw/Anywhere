using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anywhere
{
    //#用户转载传送门与ARKit相机间的所有操作
    [CreateAssetMenu(menuName = "Anywhere/Action builder")]
    public class ActionBuilder : ScriptableObject
    {
        //需要Update的操作
        public List<BaseAction> m_TickList = new List<BaseAction>();

        //通过触发调用的操作
        public List<BaseAction> m_TriggerList = new List<BaseAction>();

    }
}