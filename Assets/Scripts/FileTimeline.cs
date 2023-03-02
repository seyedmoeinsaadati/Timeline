using UnityEditor;
using UnityEngine;

namespace Moein.TimeSystem
{
    public class FileTimeline : MonoBehaviour
    {
        [SerializeField] private string fileName;
        [SerializeField] private float captureInterval = .5f;

        private float t;
        [HideInInspector, SerializeField] private float timelineTime; // between 0, recordingTime
        [SerializeField] private int pointer;
        [SerializeField] private int maxTimelineCaptureCount;


        public string FileName
        {
            get => fileName;
            set => fileName = value;
        }

        private void Start()
        {
            pointer = -1;
            InitComponents();
            // children = GetComponentsInChildren<Timeline>();
        }

        public void Progress(float timeScale)
        {
            CalculateTiming(timeScale);

            ApplyComponents();
        }

        public void Rewind(float timeScale)
        {
            CalculateTiming(timeScale);

            // transformTimeline.ApplySnapshot(transformTimeline.LerpSnapshot(transformTimeline.HeadSnapshot,
            //     transformTimeline.CurrentSnapshot, t));
            // return;


            ApplyComponents();
        }

        private void CalculateTiming(float timeScale)
        {
            timelineTime += Time.fixedDeltaTime * timeScale;
            timelineTime = Mathf.Clamp(timelineTime, 0, maxTimelineCaptureCount * captureInterval);
            pointer = (int) (timelineTime / captureInterval);
            t = (timelineTime - pointer * captureInterval) / captureInterval;
        }

        #region TimelineComponents

        private TransformComponent transformTimeline;

        private void InitComponents()
        {
            transformTimeline = new TransformComponent(transform);
        }

        public void CaptureComponents()
        {
            transformTimeline.CaptureSnapshot();
        }

        private void ApplyComponents()
        {
            if (pointer < maxTimelineCaptureCount - 1)
            {
                transformTimeline.ApplySnapshot(transformTimeline.LerpSnapshot(pointer, pointer + 1, t));
            }
        }

        #endregion

        public void SaveComponents(string directory)
        {
            TimeRecorderFileHandler.Save(directory, fileName, transformTimeline.Tape);
        }

        public void LoadComponents(string directory, float interval)
        {
            captureInterval = interval;
            pointer = -1;

            transformTimeline.Tape = TimeRecorderFileHandler.Load<TransformSnapshot>(directory, fileName);
            maxTimelineCaptureCount = transformTimeline.CaptureCount;
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(FileTimeline)), CanEditMultipleObjects]
    public class FileTimelineEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            if (Application.isPlaying)
            {
                GUILayout.Space(5);
                var timelineTime = serializedObject.FindProperty("timelineTime");
                GUILayout.Label($"Timeline Time: {timelineTime.floatValue}");
            }

            // read-only mode
            // if (Application.isPlaying == false)
            // {
            //     DrawDefaultInspector();
            // }
            // else
            // {
            //     serializedObject.Update();
            //
            //     var saveMemory = serializedObject.FindProperty("saveMemory");
            //     var captureInterval = serializedObject.FindProperty("captureInterval");
            //     var recordingTime = serializedObject.FindProperty("recordingTime");
            //     var timelineTime = serializedObject.FindProperty("timelineTime");
            //
            //     GUILayout.Label($"Save Memory: {saveMemory}");
            //     GUILayout.Label($"Capturing Interval: {captureInterval.floatValue}");
            //     GUILayout.Label($"Recording Time: {recordingTime.floatValue}");
            //     GUILayout.Space(5);
            //     GUILayout.Label($"Timeline Time: {timelineTime.floatValue}");
            // }
        }
    }

#endif
}