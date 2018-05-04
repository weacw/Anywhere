using System;
using System.Collections.Generic;

namespace UnityEngine.XR.iOS
{
    public class UnityARGeneratePlane : MonoBehaviour
    {
        public GameObject planePrefab;
        private UnityARAnchorManager unityARAnchorManager;

        // Use this for initialization
        void Start()
        {
            Anywhere.NotifCenter.GetNotice.AddEventListener(Anywhere.NotifEventKey.ARKIT_DESTROYARANCHOR, DestroyARAnchor);
            Anywhere.NotifCenter.GetNotice.AddEventListener(Anywhere.NotifEventKey.ARKIT_CREATARANCHOR, CreateARAnchor);
        }

        void OnDestroy()
        {
            if (unityARAnchorManager != null)
                unityARAnchorManager.Destroy();
        }

        // void OnGUI()
        // {
        //    List<ARPlaneAnchorGameObject> arpags = unityARAnchorManager.GetCurrentPlaneAnchors ();
        //    //if (arpags.Count >= 1) {
        //        //ARPlaneAnchor ap = arpags [0].planeAnchor;
        //        //GUI.Box (new Rect (100, 100, 800, 60), string.Format ("Center: x:{0}, y:{1}, z:{2}", ap.center.x, ap.center.y, ap.center.z));
        //        //GUI.Box(new Rect(100, 200, 800, 60), string.Format ("Extent: x:{0}, y:{1}, z:{2}", ap.extent.x, ap.extent.y, ap.extent.z));
        //    //}
        // }

        private void DestroyARAnchor(Anywhere.Notification _notif)
        {
            if (unityARAnchorManager != null)
                unityARAnchorManager.Destroy();
        }
        private void CreateARAnchor(Anywhere.Notification _notif)
        {
            unityARAnchorManager = new UnityARAnchorManager();
            UnityARUtility.InitializePlanePrefab(planePrefab);
        }
    }
}

