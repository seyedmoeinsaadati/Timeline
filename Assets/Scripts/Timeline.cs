using UnityEditor;
using UnityEngine;

namespace Moein.TimeSystem
{
    public class Timeline : TimelineBase
    {
        // private Timeline[] children;
        [SerializeField] public bool saveMemory;

        private int headIndex;

        protected override void Init()
        {
            pointer = headIndex = -1;
            maxTimelineCaptureCount = Mathf.RoundToInt(recordingTime / captureInterval);
            base.Init();
        }

        public override void Progress(float timeScale)
        {
            CalculateLerping(timeScale);
            if (pointer < headIndex)
            {
                if (saveMemory)
                {
                    // forwarding
                    ApplyComponents();
                    return;
                }

                headIndex = pointer;
            }

            Capture();
        }

        public override void Rewind(float timeScale)
        {
            CalculateLerping(timeScale);
            if (pointer == headIndex)
            {
                transformTimeline.ApplySnapshot(transformTimeline.LerpSnapshot(transformTimeline.HeadSnapshot,
                    transformTimeline.CurrentSnapshot, t));
                return;
            }

            ApplyComponents();
        }

        protected override void CalculateLerping(float timeScale)
        {
            timelineTime += Time.fixedDeltaTime * timeScale;
            timelineTime = Mathf.Clamp(timelineTime, 0, recordingTime);
            pointer = (int) (timelineTime / captureInterval);
            t = (timelineTime - pointer * captureInterval) / captureInterval;
        }

        public override void Capture()
        {
            if (timelineTime >= captureInterval * (headIndex + 1))
            {
                headIndex++;

                if (headIndex >= maxTimelineCaptureCount)
                {
                    transformTimeline.Tape.RemoveAt(0);
                    headIndex = maxTimelineCaptureCount - 1;
                    timelineTime -= captureInterval;
                }

                CaptureComponents();
            }
        }

        #region TimelineComponents

        protected override void CaptureComponents()
        {
            transformTimeline.CaptureSnapshot(headIndex);
        }

        protected override void ApplyComponents()
        {
            transformTimeline.ApplySnapshot(transformTimeline.LerpSnapshot(pointer, pointer + 1, t));
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(Timeline))]
    public class TimelineEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

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