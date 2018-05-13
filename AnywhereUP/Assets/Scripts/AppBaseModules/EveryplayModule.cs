using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Anywhere
{
    [CreateAssetMenu(menuName = "Anywhere/AppModules/EveryplayModule")]
    public class EveryplayModule : BaseModule
    {
        private bool m_isRecording;

        private bool m_isRecordingFinished;
        
        private void Awake()
        {
            m_isRecording = false;
            m_isRecordingFinished = false;
            Everyplay.RecordingStarted += RecordingStarted;
            Everyplay.RecordingStopped += RecordingStopped;
        }

        private void OnDestroy()
        {
            Everyplay.RecordingStarted -= RecordingStarted;
            Everyplay.RecordingStopped -= RecordingStopped;
            NotifCenter.GetNotice.RemoveEventListener(NotifEventKey.EVERYPLAY_RECORDING_START, OnRecordingStart);
            NotifCenter.GetNotice.RemoveEventListener(NotifEventKey.EVERYPLAY_RECORDING_STOP, OnRecordingStop);
        }
        private void OnDisable()
        {
            m_isRecording = false;
        }

        // start recording
        internal void OnRecordingStart(Notification _notif)
        {
            if (m_isRecording)
            {
                OnRecordingStop(null);
                return;
            }
            Everyplay.StartRecording();
            m_isRecording = true;
            m_isRecordingFinished = false;
        }

        //stop recording
        internal void OnRecordingStop(Notification _notif)
        {
            if (!m_isRecording)
            {
                return;
            }
            Everyplay.StopRecording();
            m_isRecording = false;
            m_isRecordingFinished = true;
        }

        private void RecordingStarted()
        {
            Debug.Log("Started recording");
        }

        private void RecordingStopped()
        {
            AppManager.Instance.StartCoroutine(EveryplayLocalSaveAsync());
        }

        IEnumerator EveryplayLocalSaveAsync()
        {
            //TODO:先存放到手机上
            yield return EveryplayLocalSave.SaveToAsync(Path.Combine(Application.persistentDataPath, "share.mp4"));
        }
    }
}

