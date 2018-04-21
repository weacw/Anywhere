using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
namespace Assets.Scripts.SummonAnywhere
{
    class FindPortalComponent : MonoBehaviour
    {
        [SerializeField]
        FocusSquare m_focusSuare;
        [SerializeField]
        GameObject m_randomRortal;

        Vector3 m_foundFocusPos;

        void OnGUI()
        {
            if (m_focusSuare.SquareState == FocusSquare.FocusState.Found)
            {
                if (GUI.Button(new Rect(10, 200, 100, 30), "召唤传送门"))
                {
#if UNITY_EDITOR
                    UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath("Assets/Artwork/Effects/Anubis_Buff_02.prefab", typeof(GameObject));
#else
#endif
                    GameObject cloneObj = Instantiate<GameObject>((GameObject)obj);
                    cloneObj.transform.position = Camera.main.transform.position;
                    cloneObj.transform.localScale = Vector3.one;
                    MagicBallMoveComponent mmc = cloneObj.AddComponent<MagicBallMoveComponent>();
                    mmc.MoveTo(m_focusSuare.foundSquare.transform.position, () =>
                     {
                         //unload magic ball
                         GameObject.Destroy(cloneObj);

                         m_randomRortal.transform.position = m_focusSuare.foundSquare.transform.position;
                         m_randomRortal.SetActive(true);
                     });

                }
            }
        }
    }
}
