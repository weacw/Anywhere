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
        }

        void OnDestroy()
        {
            Everyplay.RecordingStarted -= RecordingStarted;
            Everyplay.RecordingStopped -= RecordingStopped;
        }

        public void OnRecordingStart()
        {
            if (m_isRecording)
            {
                Debug.Log("正在录制视频中");
                return;
            }
            Everyplay.StartRecording();
            m_isRecording = true;
            m_isRecordingFinished = false;
        }

        public void OnRecordingStop()
        {
            if (!m_isRecording)
            {
                Debug.Log("没有在录制视频");
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

