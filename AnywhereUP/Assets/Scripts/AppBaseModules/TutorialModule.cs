namespace Anywhere
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Anywhere/TutorialModule")]
    public class TutorialModule : BaseModule
    {
        internal System.Action m_WasNoFirstTimeCallBack;

        public GameObject m_TalkPrefab;
        public GameObject m_ToturialPrefab;

        private GameObject m_InstancedTutorial;
        private List<GameObject> m_TalkList;

        private bool m_FirstTimeEntry = true;
        private bool m_WasInit = false;
        private Transform m_TalkContentRoot;

        private void Init()
        {
            Transform m_UIRoot = GameObject.FindGameObjectWithTag("UICanvas").transform;

            m_TalkList = new List<GameObject>();
            string state = PlayerPrefs.GetString("FirstTimeEntry");
            if (!string.IsNullOrEmpty(state))
                bool.TryParse(state, out m_FirstTimeEntry);
            if (!m_FirstTimeEntry)
            {
                if (m_WasNoFirstTimeCallBack != null) m_WasNoFirstTimeCallBack.Invoke();
                return;
            }

            m_InstancedTutorial = Instantiate(m_ToturialPrefab, m_UIRoot);
            m_InstancedTutorial.transform.localScale = Vector3.one;
            m_InstancedTutorial.transform.localPosition = Vector3.zero;
            m_InstancedTutorial.transform.localRotation = Quaternion.identity;
            PlayerPrefs.SetString("FirstTimeEntry", "false");
            m_TalkContentRoot = m_InstancedTutorial.transform.Find("Scroll View/Viewport/Content");
            m_WasInit = true;
        }

        private int CreateTalk(Transform _root)
        {
            m_TalkList.Add(Instantiate(m_TalkPrefab, _root));
            return m_TalkList.Count - 1;
        }
        private void OnDisable()
        {
            m_TalkContentRoot = null;
            m_WasInit = false;
            m_FirstTimeEntry = true;
            if (m_InstancedTutorial != null) DestroyImmediate(m_InstancedTutorial);
            m_InstancedTutorial = null;
            if (m_TalkList != null)
            {
                foreach (var talk in m_TalkList) DestroyImmediate(talk);
                m_TalkList.Clear();
            }
            PlayerPrefs.DeleteKey("FirstTimeEntry");
        }


        private IEnumerator WaitToInstance(float _time, Transform _root)
        {
            yield return new WaitForSeconds(_time);
            CreateTalk(_root);
        }

        internal void CreateTalk(Notification _notif)
        {
            if (!m_WasInit)
                Init();
        }
    }
}
