using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anywhere
{
    public class AppManager : Singleton<AppManager>
    {
        public List<BaseModule> m_ScriptModules = new List<BaseModule>();

        private Dictionary<string, BaseModule> m_ScriptModulesDict = new Dictionary<string, BaseModule>();
        public override void Awake()
        {
            foreach (var module in m_ScriptModules)
            {
                if (!m_ScriptModulesDict.ContainsKey(module.name))
                    m_ScriptModulesDict.Add(module.name, module);
            }
        }
    }
}