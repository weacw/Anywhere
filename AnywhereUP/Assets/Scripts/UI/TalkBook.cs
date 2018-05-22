using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Anywhere
{
    [CreateAssetMenu(menuName = "Anywhere/TalkBook")]
    public class TalkBook : ScriptableObject
    {
        public List<TalkField> m_TalkList = new List<TalkField>();
    }
    [System.Serializable]
    public class TalkField
    {
        public string m_Role;
        public string m_TalkValue;
        public bool m_Interaction;
    }
}
