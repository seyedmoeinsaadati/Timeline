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
            transformTimeline = new TransformComponent(transform);
            initialized = true;
        }

        public void Jump(float time)
        {
            timelineTime = time;
            CalculatingTime();
            Apply();
        }

        public void Lerp(float t)
        {
            timelineTime = Mathf.Lerp(0, maxTimelineCaptureCount * captureInterval, t);
            CalculatingTime();
            Apply();
        }

        public override void Progress(float timeScale)
        {
            timelineTime += Time.fixedDeltaTime * timeScale;
            CalculatingTime();
            Apply();
        }

        public override void Rewind(float timeScale)
        {
            timelineTime += Time.fixedDeltaTime * timeScale;
            CalculatingTime();
            Apply();
        }

        protected override void CalculatingTime()
        {
            timelineTime = Mathf.Clamp(timelineTime, 0, maxTimelineCaptureCount * captureInterval);
            pointer = (int) (timelineTime / captureInterval);
            t = (timelineTime - pointer * captureInterval) / captureInterval;
        }

        public override void Capture()
        {
            transformTimeline.CaptureSnapshot();
        }

        protected override void Apply()
        {
            if (pointer < maxTimelineCaptureCount - 1)
            {
                transformTimeline.ApplySnapshot(transformTimeline.LerpSnapshot(pointer, pointer + 1, t));
            }
        }

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

        public AnimationCurve[] GetAnimationCurve()
        {
            return TransformComponent.ToAnimationCurve(transformTimeline, captureInterval);
        }

#if UNITY_EDITOR
        public float MaxTime => maxTimelineCaptureCount * captureInterval;
#endif
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(FileTimeline)), CanEditMultipleObjects]
    public class FileTimelineEditor : Editor
    {
        private FileTimeline timeline;
        private float sliderTime;

        private void OnEnable()
        {
            timeline = target as FileTimeline;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            var timelineTime = serializedObject.FindProperty("timelineTime");

            if (Application.isPlaying)
            {
                GUILayout.Space(5);
                GUILayout.Label($"Timeline Time: {timelineTime.floatValue}");

                sliderTime = EditorGUILayout.Slider("timeline: ", sliderTime, 0, timeline.MaxTime);
                timeline.Jump(sliderTime);
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