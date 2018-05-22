using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Anywhere;

public class TutorialLogics : MonoBehaviour
{
    public List<GameObject> m_TalkList;
    private int m_Index = 0;
    private bool m_WasClicked = false;
    private void Start()
    {
        // m_TalkList = new List<GameObject>();
        InvokeRepeating("ShowNextTalk", 0.5f, 1);
    }

    private void ShowNextTalk()
    {
        string str = m_TalkList[m_Index].GetComponentInChildren<UnityEngine.UI.Text>().text;
        int id = m_TalkList.Count - 1;

        if (m_Index == 2)
        {
            m_TalkList[id].GetComponentInChildren<UnityEngine.UI.Text>().text = str;
            m_TalkList[id].SetActive(true);
            return;
        }
        else if (m_Index == 5)
        {
            m_TalkList[id].GetComponentInChildren<UnityEngine.UI.Text>().text = str;
            m_TalkList[id].SetActive(true);
            return;
        }
        else if (m_Index == 9)
        {
            m_TalkList[id].GetComponentInChildren<UnityEngine.UI.Text>().text = str;
            m_TalkList[id].SetActive(true);
            return;
        }


        if (m_Index >= m_TalkList.Count - 1)
        {
            CancelInvoke("ShowNextTalk");
            m_Index = m_TalkList.Count - 2;
            return;
        }
        m_WasClicked = false;
        m_TalkList[m_Index].SetActive(true);
        m_TalkList[m_Index].transform.DOLocalMoveY(m_TalkList[m_Index].transform.localPosition.y + 15, 0.5f);
      m_TalkList[m_Index].GetComponent<CanvasGroup>().DOFade(1, 1f);
        m_Index++;
    }

    public void Talk()
    {
        int id = m_TalkList.Count - 1;
        m_TalkList[id].GetComponentInChildren<UnityEngine.UI.Text>().text = null;
        m_TalkList[id].SetActive(false);
        m_WasClicked = true;

        m_TalkList[m_Index].SetActive(true);
        m_TalkList[m_Index].transform.DOLocalMoveY(m_TalkList[m_Index].transform.localPosition.y + 20, 0.5f);
        Tweener tw = m_TalkList[m_Index].GetComponent<CanvasGroup>().DOFade(1, 1f);
        m_Index++;

        tw.OnComplete(() =>
        {
            if (m_Index == 9)
            {
                this.gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(() => DestroyImmediate(this.gameObject));
                HttpGetDataHelper tmp_HttpGetDataHelper = new HttpGetDataHelper();
                tmp_HttpGetDataHelper.m_Finished = null;
                tmp_HttpGetDataHelper.m_PageIndex = Configs.GetConfigs.ContentPageNum;
                NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.HTTP_GETPAGEDATAS, tmp_HttpGetDataHelper);
                NotifCenter.GetNotice.PostDispatchEvent(NotifEventKey.UI_SHOWHIDELOADING, new UICtrlHelper() { m_State = true });
            }
        });
    }
}
