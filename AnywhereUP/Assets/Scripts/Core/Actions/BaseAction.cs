using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anywhere
{
    public abstract class BaseAction : ScriptableObject
    {
		public abstract void Execute(PortalActionManager _hook);
    }
}