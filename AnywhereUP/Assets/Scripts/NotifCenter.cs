using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anywhere
{
    public class NotifCenter
    {
        /// <summary>
        /// 单例，提供对外调用接口
        /// </summary>
        private static NotifCenter m_Instance = null;

        /// <summary>
        /// 调用NotifCenter方法
        /// </summary>
        /// <returns></returns>
        public static NotifCenter GetNotice
        {
            get
            {
                if (m_Instance != null) return m_Instance;
                m_Instance = new NotifCenter();
                return m_Instance;
            }
        }

        /// <summary>
        /// 监听列表
        /// </summary>
        private Dictionary<NotifEventKey, Action> m_EventListener = new Dictionary<NotifEventKey, Action>();


        /// <summary>
        /// 添加监听
        /// </summary>
        /// <param name="_eventKey"></param>
        /// <param name="_listener"></param>
        public void AddEventListener(NotifEventKey _eventKey, Action _listener)
        {
            if (!HasEventListener(_eventKey))
            {
                m_EventListener.Add(_eventKey, _listener);
            }
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="_eventKey"></param>
        /// <param name="_listener"></param>
        public void RemoveEventListener(NotifEventKey _eventKey, Action _listener)
        {
            if (!HasEventListener(_eventKey)) return;
            m_EventListener[_eventKey] -= _listener;
            if (m_EventListener[_eventKey] == null)
            {
                m_EventListener.Remove(_eventKey);
            }
        }


        /// <summary>
        /// 分发事件，须知消息状况
        /// </summary>
        /// <param name="_eventKey"></param>        
        public void PostDispatchEvent(NotifEventKey _eventKey)
        {
            if (!HasEventListener(_eventKey)) return;
            m_EventListener[_eventKey].Invoke();
        }

        /// <summary>
        /// 查询_eventKey 存留与 eventListener列表中
        /// </summary>
        /// <param name="_eventKey"></param>
        /// <returns></returns>
        private bool HasEventListener(NotifEventKey _eventKey)
        {
            Debug.LogError(string.Format("eventListener do not has eventkey{0}", _eventKey));
            return m_EventListener.ContainsKey(_eventKey);
        }
    }

}


