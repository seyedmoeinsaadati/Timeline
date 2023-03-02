using UnityEditor;
using UnityEngine;

namespace Moein.TimeSystem
{
    public class FileTimeline : TimelineBase
    {
        [SerializeField] private string fileName;

        public string FileName
        {
            get => fileName;
            set => fileName = value;
        }

        protected override void Init()
        {
            pointer = -1;
            base.Init();
        }

        public override void Progress(float timeScale)
        {
            CalculateLerping(timeScale);
            ApplyComponents();
        }

        public override void Rewind(float timeScale)
        {
            CalculateLerping(timeScale);
            ApplyComponents();
        }

        protected override void CalculateLerping(float timeScale)
        {
            timelineTime += Time.fixedDeltaTime * timeScale;
            timelineTime = Mathf.Clamp(timelineTime, 0, maxTimelineCaptureCount * captureInterval);
            pointer = (int) (timelineTime / captureInterval);
            t = (timelineTime - pointer * captureInterval) / captureInterval;
        }

        public override void Capture()
        {
            CaptureComponents();
        }

        #region TimelineComponents

        protected override void InitComponents()
        {
            transformTimeline = new TransformComponent(transform);
        }

        protected override void CaptureComponents()
        {
            transformTimeline.CaptureSnapshot();
        }

        protected override void ApplyComponents()
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