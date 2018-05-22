namespace Anywhere
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Anywhere/TutorialModule")]
    public class TutorialModule : BaseModule
    {
        public GameObject m_TalkPrefab;
        public GameObject m_ToturialPrefab;

        private GameObject m_InstancedTutorial;
        private List<GameObject> m_TalkList;

        private bool m_FirstTimeEntry = true;
        private bool m_WasInit = false;
        private Transform m_TalkContentRoot;
#if UNITY_EDITOR
        public bool m_AutoDel;
#endif
        private void Init()
        {
            // Transform m_UIRoot = GameObject.Find("AnywhereUICanvas").transform;

            // m_TalkList = new List<GameObject>();
            // string state = PlayerPrefs.GetString("FirstTimeEntry");
            // Debug.Log(state);
            // if (!string.IsNullOrEmpty(state))
            //     bool.TryParse(state, out m_FirstTimeEntry);
            // if (!m_FirstTimeEntry) return;

            // Debug.Log(m_UIRoot.name);
            // m_InstancedTutorial = Instantiate(m_ToturialPrefab, m_UIRoot);
            // m_InstancedTutorial.transform.localScale = Vector3.one;
            // m_InstancedTutorial.transform.localPosition = Vector3.zero;
            // m_InstancedTutorial.transform.localRotation = Quaternion.identity;
            // PlayerPrefs.SetString("FirstTimeEntry", "false");
            // m_TalkContentRoot = m_InstancedTutorial.transform.Find("Content");
            // m_WasInit = true;
        }

        private int CreateTalk(Transform _root)
        {
            m_TalkList.Add(Instantiate(m_TalkPrefab, _root));
            return m_TalkList.Count - 1;
        }
#if UNITY_EDITOR
        private void OnDisable()
        {
            if (m_AutoDel)
                PlayerPrefs.DeleteKey("FirstTimeEntry");
        }
#endif

        private IEnumerator WaitToInstance(float _time, Transform _root)
        {
            yield return new WaitForSeconds(_time);
            CreateTalk(_root);
        }

        internal void CreateTalk(Notification _notif)
        {
            if (!m_WasInit)
            {
                Init();
                return;
            }

            UITextHelper tmp_UICtrlHelper = _notif.param as UITextHelper;
            UnityEngine.UI.Text tmp_TextComp = m_TalkList[CreateTalk(m_TalkContentRoot)].GetComponentInChildren<UnityEngine.UI.Text>();
            tmp_TextComp.text = tmp_UICtrlHelper.m_TextValue;
            tmp_TextComp.alignment = tmp_UICtrlHelper.m_TextHorAnchor;
        }
    }
}
