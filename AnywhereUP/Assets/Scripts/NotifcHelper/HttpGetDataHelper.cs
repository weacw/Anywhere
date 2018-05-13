using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anywhere
{
    public class HttpGetDataHelper :System.EventArgs
    {
        public int m_PageIndex = 0;
        public System.Action m_Finished;
    }
}
