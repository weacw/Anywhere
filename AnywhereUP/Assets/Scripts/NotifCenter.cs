using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anywhere
{
    public class Notification
    {
        public EventArgs param;
        public Notification(EventArgs param)
        {
            this.param = param;
        }
        public Notification()
        {

        }
    }
    public delegate void NotificationDelegate(Notification notific);
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
        private Dictionary<NotifEventKey, NotificationDelegate> m_EventListener = new Dictionary<NotifEventKey, NotificationDelegate>();


        /// <summary>
        /// 添加监听
        /// </summary>
        /// <param name="_eventKey"></param>
        /// <param name="_listener"></param>
        public void AddEventListener(NotifEventKey _eventKey, NotificationDelegate listener)
        {
            if (!HasEventListener(_eventKey))
            {
                NotificationDelegate del = null; //定义方法
                m_EventListener[_eventKey] = del;// 给委托变量赋值
            }
            m_EventListener[_eventKey] += listener; //注册接收者的监听
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="_eventKey"></param>
        /// <param name="_listener"></param>
        public void RemoveEventListener(NotifEventKey _eventKey, NotificationDelegate listener)
        {
            if (!HasEventListener(_eventKey))
                return;
            m_EventListener[_eventKey] -= listener;
            if (m_EventListener[_eventKey] == null)
            {
                RemoveEventListener(_eventKey);
            }
        }
        public void RemoveEventListener(NotifEventKey _eventKey)
        {
            m_EventListener.Remove(_eventKey);
        }

        /// <summary>
        /// 分发事件，须知消息状况
        /// </summary>
        /// <param name="_eventKey"></param>        
        public void PostDispatchEvent(NotifEventKey _eventKey, Notification _notif)
        {
            if (!HasEventListener(_eventKey))
                return;
            m_EventListener[_eventKey](_notif);
        }

        public void PostDispatchEvent(NotifEventKey _eventKey)
        {
            if (!HasEventListener(_eventKey))
                return;
            m_EventListener[_eventKey](new Notification());
        }
        public void PostDispatchEvent(NotifEventKey _eventKey, EventArgs param)
        {
            if (!HasEventListener(_eventKey))
                return;
            m_EventListener[_eventKey](new Notification(param));
        }
        /// <summary>
        /// 查询_eventKey 存留与 eventListener列表中
        /// </summary>
        /// <param name="_eventKey"></param>
        /// <returns></returns>
        private bool HasEventListener(NotifEventKey _eventKey)
        {
            //Debug.LogError(string.Format("eventListener do not has eventkey{0}", _eventKey));
            return m_EventListener.ContainsKey(_eventKey);
        }
    }

}


