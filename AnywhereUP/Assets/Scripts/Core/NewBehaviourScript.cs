using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NewBehaviourScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        // Shader.SetGlobalInt("_StencilTest", (int)CompareFunction.NotEqual);
        Shader.SetGlobalInt("_StencilTest",  (int)CompareFunction.NotEqual);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
