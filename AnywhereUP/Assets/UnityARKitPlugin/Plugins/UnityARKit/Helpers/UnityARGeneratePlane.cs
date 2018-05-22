using System;
using System.Collections.Generic;

namespace UnityEngine.XR.iOS
{
    public class UnityARGeneratePlane : MonoBehaviour
    {
        public GameObject planePrefab;
        private UnityARAnchorManager unityARAnchorManager;

       
        private void Start()
        {
            Anywhere.NotifCenter.GetNotice.AddEventListener(Anywhere.NotifEventKey.ARKIT_CREATEARANCHOR, CreateARAnchor);
            Anywhere.NotifCenter.GetNotice.AddEventListener(Anywhere.NotifEventKey.ARKIT_DESTORYARANCHOR, DestroyARAnchor);
        }

        private void OnDestroy()
        {
            if (unityARAnchorManager != null)
                unityARAnchorManager.Destroy();
        }

        private void DestroyARAnchor(Anywhere.Notification _notif)
        {
            if (unityARAnchorManager != null)
                unityARAnchorManager.Destroy();
        }
        private void CreateARAnchor(Anywhere.Notification _notif)
        {
            unityARAnchorManager = new UnityARAnchorManager();
            //UnityARUtility.InitializePlanePrefab(planePrefab);
        }
    }
}

