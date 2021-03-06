﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class FocusSquare : MonoBehaviour
{

    public enum FocusState
    {
        Initializing,
        Finding,
        Found,
        Putdown
    }

    public GameObject findingSquare;
    public GameObject foundSquare;

    //for editor version
    public float maxRayDistance = 30.0f;
    public LayerMask collisionLayerMask;
    public float findingSquareDist = 0.5f;
    private bool trackingInitialized;
    private bool m_ShowedButton;
    private FocusState squareValue;
    private IEnumerator m_FocusCoroutine;
    private FocusState SquareState
    {
        get
        {
            return squareValue;
        }
        set
        {
            squareValue = value;
            foundSquare.SetActive(squareValue == FocusState.Found);
            findingSquare.SetActive(squareValue != FocusState.Found);
        }
    }


    /// <summary>
    /// 初始化
    /// </summary>
    private void Start()
    {
        Anywhere.NotifCenter.GetNotice.AddEventListener(Anywhere.NotifEventKey.OPERATER_SETFOCUSPOSTOCONTENT, SyncFocusPosToContent);
        Anywhere.NotifCenter.GetNotice.AddEventListener(Anywhere.NotifEventKey.ARKIT_FOCUS_ON, TurnOnFocus);
        Anywhere.NotifCenter.GetNotice.AddEventListener(Anywhere.NotifEventKey.ARKIT_FOCUS_OFF, TurnOffFocus);

        SquareState = FocusState.Initializing;
        trackingInitialized = true;

        m_FocusCoroutine = CheckingPlane();
    }

    /// <summary>
    /// 检测到平面
    /// </summary>
    /// <param name="point"></param>
    /// <param name="resultTypes"></param>
    /// <returns></returns>
    private bool HitTestWithResultType(ARPoint point, ARHitTestResultType resultTypes)
    {
        List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
        if (hitResults.Count > 0)
        {
            foreach (var hitResult in hitResults)
            {
                foundSquare.transform.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                foundSquare.transform.rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
                return true;
            }
        }
        return false;
    }

    private void UpdateFocus()
    {
        if (SquareState == FocusState.Putdown) return;
        //use center of screen for focusing
        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, findingSquareDist);

#if UNITY_EDITOR
        Ray ray = Camera.main.ScreenPointToRay(center);
        RaycastHit hit;

        //we'll try to hit one of the plane collider gameobjects that were generated by the plugin
        //effectively similar to calling HitTest with ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent
        if (Physics.Raycast(ray, out hit, maxRayDistance, collisionLayerMask))
        {
            //we're going to get the position from the contact point
            foundSquare.transform.position = hit.point;
            //Debug.Log(string.Format("x:{0:0.######} y:{1:0.######} z:{2:0.######}", foundSquare.transform.position.x, foundSquare.transform.position.y, foundSquare.transform.position.z));

            //and the rotation from the transform of the plane collider
            SquareState = FocusState.Found;
            foundSquare.transform.rotation = hit.transform.rotation;
            if (!m_ShowedButton)
            {
                Anywhere.NotifCenter.GetNotice.PostDispatchEvent(Anywhere.NotifEventKey.UI_SHOWCALLBTN);
                m_ShowedButton = true;
            }
            return;
        }

        //#endif
#else
        var screenPosition = Camera.main.ScreenToViewportPoint(center);
        ARPoint point = new ARPoint
        {
            x = screenPosition.x,
            y = screenPosition.y
        };

        // prioritize reults types
        ARHitTestResultType[] resultTypes = {
            ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
			// if you want to use infinite planes use this:
			//ARHitTestResultType.ARHitTestResultTypeExistingPlane,
			//ARHitTestResultType.ARHitTestResultTypeHorizontalPlane, 
			//ARHitTestResultType.ARHitTestResultTypeFeaturePoint
		};

        foreach (ARHitTestResultType resultType in resultTypes)
        {
            if (HitTestWithResultType(point, resultType))
            {
                SquareState = FocusState.Found;
                if (!m_ShowedButton)
                {
                    m_ShowedButton = true;
                    Anywhere.NotifCenter.GetNotice.PostDispatchEvent(Anywhere.NotifEventKey.UI_SHOWCALLBTN);
                }
                return;
            }
        }
#endif
        //if you got here, we have not found a plane, so if camera is facing below horizon, display the focus "finding" square
        if (trackingInitialized)
        {
            SquareState = FocusState.Finding;
            m_ShowedButton = false;
            Anywhere.NotifCenter.GetNotice.PostDispatchEvent(Anywhere.NotifEventKey.UI_HIDECALLBTN);
            //check camera forward is facing downward
            if (Vector3.Dot(Camera.main.transform.forward, Vector3.down) > 0)
            {

                //position the focus finding square a distance from camera and facing up
                findingSquare.transform.position = Camera.main.ScreenToWorldPoint(center);

                //vector from camera to focussquare
                Vector3 vecToCamera = findingSquare.transform.position - Camera.main.transform.position;

                //find vector that is orthogonal to camera vector and up vector
                Vector3 vecOrthogonal = Vector3.Cross(vecToCamera, Vector3.up);

                //find vector orthogonal to both above and up vector to find the forward vector in basis function
                Vector3 vecForward = Vector3.Cross(vecOrthogonal, Vector3.up);


                findingSquare.transform.rotation = Quaternion.LookRotation(vecForward, Vector3.up);

            }
            else
            {
                //we will not display finding square if camera is not facing below horizon
                findingSquare.SetActive(false);
            }

        }
    }

    /// <summary>
    /// 将内容的位置与focus的位置进行同步
    /// </summary>
    /// <param name="_notif"></param>
    private void SyncFocusPosToContent(Anywhere.Notification _notif)
    {
        //Vector3 focusPos = foundSquare.transform.position;
        Vector3 dir = Camera.main.transform.position - foundSquare.transform.position;
        Quaternion newQuat = Quaternion.LookRotation(dir);
        newQuat.x = newQuat.z = 0;
        ContentPlaceHelper cph = new ContentPlaceHelper
        {
            //Camera.main.transform.rotation
            m_ContentPos = foundSquare.transform.localPosition,
            m_ContentRot = newQuat,
            m_FocusGameObject = this.gameObject
        };
        Anywhere.NotifCenter.GetNotice.PostDispatchEvent(Anywhere.NotifEventKey.OPERATER_PLACECONTENT, cph);
        Anywhere.NotifCenter.GetNotice.PostDispatchEvent(Anywhere.NotifEventKey.ARKIT_FOCUS_OFF);
    }

    /// <summary>
    /// 开启focus
    /// </summary>
    /// <param name="_notif"></param>
    private void TurnOnFocus(Anywhere.Notification _notif)
    {
        m_ShowedButton = false;
        trackingInitialized = true;
        SquareState = FocusState.Initializing;

        this.gameObject.SetActive(true);
        StartCoroutine(m_FocusCoroutine);
    }

    /// <summary>
    /// 关闭focus
    /// </summary>
    /// <param name="_notif"></param>
    private void TurnOffFocus(Anywhere.Notification _notif)
    {
        trackingInitialized = false;
        SquareState = FocusState.Putdown;
        this.gameObject.SetActive(false);
        StopCoroutine(m_FocusCoroutine);
    }

    /// <summary>
    /// 开启focus检测
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckingPlane()
    {
        while (SquareState == FocusState.Found || SquareState == FocusState.Finding || SquareState == FocusState.Initializing)
        {
            yield return null;
            UpdateFocus();
        }
    }
}
