using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBallMoveComponent : MonoBehaviour
{

    private float m_moveSpeed = 1.0f;
    private bool m_bStartMove = false;
    public Vector3 m_targetVec;
    private Action m_moveComplete = null;
    // Use this for initialization
    void Start()
    {

    }

    public void MoveTo(Transform targetTrans, Action callback = null, float moveSpeed = 1.0f)
    {
        MoveTo(targetTrans.transform.position, callback, moveSpeed);
    }

    public void MoveTo(Vector3 targetVec, Action callback = null, float moveSpeed = 1.0f)
    {
        m_targetVec = targetVec;
        m_moveSpeed = moveSpeed;
        m_bStartMove = true;
        m_moveComplete = callback;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            m_bStartMove = true;
        }
        if (m_bStartMove)
        {
            float moveDistance = m_moveSpeed * Math.Min(Time.deltaTime, 1f / 30);

            Vector3 normalized = ((m_targetVec - transform.position).normalized);
            Vector3 nextPos = transform.position + ((m_targetVec - transform.position).normalized) * moveDistance;
            float targetDis = Vector3.Distance(nextPos, m_targetVec);
            if (targetDis < 0.34f)
            {
                transform.position = m_targetVec;
                m_bStartMove = false;
                if (m_moveComplete != null)
                    m_moveComplete();
            }
            else
            {
                transform.position = nextPos;
            }
        }
    }
}
