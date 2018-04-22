using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Anywhere
{
    public class EveryplayManager : MonoBehaviour
    {
        private bool m_isRecording;
        private bool m_isRecordingFinished;

        void Awake()
        {
            m_isRecording = false;
            m_isRecordingFinished = false;
            Everyplay.RecordingStarted += RecordingStarted;
            Everyplay.RecordingStopped += RecordingStopped;
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.EVERYPLAY_RECORDING_START, OnRecordingStart);
            NotifCenter.GetNotice.AddEventListener(NotifEventKey.EVERYPLAY_RECORDING_STOP,OnRecordingStop);
        }

        void OnDestroy()
        {
            Everyplay.RecordingStarted -= RecordingStarted;
            Everyplay.RecordingStopped -= RecordingStopped;
            NotifCenter.GetNotice.RemoveEventListener(NotifEventKey.EVERYPLAY_RECORDING_START, OnRecordingStart);
            NotifCenter.GetNotice.RemoveEventListener(NotifEventKey.EVERYPLAY_RECORDING_STOP, OnRecordingStop);
        }

        // start recording
        private void OnRecordingStart(Notification _notif)
        {
            if (m_isRecording)
            {
                return;
            }
            Everyplay.StartRecording();
            m_isRecording = true;
            m_isRecordingFinished = false;
        }

        //stop recording
        private void OnRecordingStop(Notification _notif)
        {
            if (!m_isRecording)
            {
                return;
            }
            Everyplay.StopRecording();
            m_isRecording = false;
            m_isRecordingFinished = true;
        }

        private void RecordingStarted() { }

        private void RecordingStopped()
        {
            StartCoroutine(EveryplayLocalSaveAsync());
        }

        IEnumerator EveryplayLocalSaveAsync()
        {
            //TODO:先存放到手机上
            yield return EveryplayLocalSave.SaveToAsync(Path.Combine(Application.persistentDataPath, "share.mp4"));
        }
    }
}

