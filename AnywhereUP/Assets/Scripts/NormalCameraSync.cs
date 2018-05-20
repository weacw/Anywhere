using UnityEngine;
using UnityEngine.XR.iOS;

namespace Anywhere
{
    public class NormalCameraSync : Singleton<NormalCameraSync>
    {

        public Camera m_NormalCamera;
        public Camera m_ARCamera;
        public Vector3 m_ScaledObjectOrigin = Vector3.one;
        public bool m_SyncPosition = false;
        public bool m_SyncRotation = true;
        private float m_CameraScale = 1.0f;


        void LateUpdate()
        {

            Matrix4x4 matrix = UnityARSessionNativeInterface.GetARSessionNativeInterface().GetCameraPose();
            float invScale = 1.0f / m_CameraScale;
            Vector3 cameraPos = UnityARMatrixOps.GetPosition(matrix);
            Vector3 vecAnchorToCamera = cameraPos - m_ScaledObjectOrigin;
            m_NormalCamera.transform.localPosition = m_ScaledObjectOrigin + (vecAnchorToCamera * invScale);
            m_NormalCamera.transform.localRotation = UnityARMatrixOps.GetRotation(matrix);


            //this needs to be adjusted for near/far
            m_NormalCamera.projectionMatrix = UnityARSessionNativeInterface.GetARSessionNativeInterface().GetCameraProjection();
        }

    }
}