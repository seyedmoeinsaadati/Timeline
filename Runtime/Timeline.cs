using UnityEditor;
using UnityEngine;

namespace Moein.TimeSystem
{
    public class Timeline : TimelineBase
    {
        [SerializeField, Min(1)] protected float recordingTime = 30f;
        [SerializeField] public bool saveMemory;
        protected int headIndex;
        private float timescale;

        protected override void Init()
        {
            pointer = headIndex = -1;
            maxTimelineCaptureCount = Mathf.RoundToInt(recordingTime / captureInterval);

            transformTimeline = new TransformComponent(transform, maxTimelineCaptureCount);
            var animator = GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animatorComponent = new AnimatorComponent(animator)
                {
                    RecordedFrames = (int) (recordingTime * 30)
                };
            }

            initialized = true;
        }

        public override void Progress(float timescale)
        {
            this.timescale = timescale;
            timelineTime += Time.fixedDeltaTime * timescale;
            CalculatingTime();

            if (pointer < headIndex)
            {
                if (saveMemory)
                {
                    // forwarding
                    Apply();
                    return;
                }

                headIndex = pointer;
            }

            Capture();
        }

        public override void Rewind(float timescale)
        {
            this.timescale = timescale;
            timelineTime += Time.fixedDeltaTime * timescale;
            CalculatingTime();
            if (pointer == headIndex)
            {
                transformTimeline.ApplySnapshot(transformTimeline.LerpSnapshot(transformTimeline.HeadSnapshot,
                    transformTimeline.CurrentSnapshot, t));
                return;
            }

            Apply();
        }

        protected override void CalculatingTime()
        {
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

                transformTimeline.CaptureSnapshot(headIndex);
            }

            animatorComponent?.CaptureSnapshot(timescale);
        }

        protected override void Apply()
        {
            transformTimeline.ApplySnapshot(transformTimeline.LerpSnapshot(pointer, pointer + 1, t));

            animatorComponent?.ApplySnapshot(timescale);
        }

        #region TimelineComponents

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(Timeline)), CanEditMultipleObjects]
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