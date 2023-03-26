using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

        public Vector3 DefaultPosition => transform.parent.TransformPoint(transformTimeline.Tape[0].position);

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
            pointer = (int)(timelineTime / captureInterval);
            t = (timelineTime - pointer * captureInterval) / captureInterval;
        }

        public override void Capture()
        {
            transformTimeline.CaptureSnapshot();
        }

        protected override void Apply()
        {
            if (pointer < maxTimelineCaptureCount - 1)
                transformTimeline.ApplySnapshot(transformTimeline.LerpSnapshot(pointer, pointer + 1, t));
            else
                transformTimeline.ApplySnapshot(transformTimeline.HeadSnapshot);
        }

        public void SaveComponents(string directory)
        {
            TimeRecorderFileHandler.Save(directory, fileName, transformTimeline.Tape);
        }

        public void LoadComponents(float interval, bool loadOnHead, params TakeInfo[] takes)
        {
            captureInterval = interval;
            pointer = -1;

            for (int j = 0; j < takes.Length; j++)
            {
                AddTape(takes[j].Directory, takes[j].reverse);
            }

            maxTimelineCaptureCount = transformTimeline.CaptureCount;

            if (loadOnHead) 
                Jump(maxTimelineCaptureCount * interval);
        }

        public void AddTape(string directory, bool reverseLoad = false)
        {
            var newTape = TimeRecorderFileHandler.Load<TransformSnapshot>(directory, fileName);
            if (reverseLoad) newTape.Reverse();

            transformTimeline.Tape.AddRange(newTape);
        }

        public AnimationCurve[] GetAnimationCurve()
        {
            return TransformComponent.ToAnimationCurve(transformTimeline, captureInterval);
        }

#if UNITY_EDITOR
        public float MaxTime => maxTimelineCaptureCount * captureInterval;

        //private void OnDrawGizmos()
        //{
        //    if (EditorApplication.isPlaying)
        //    {
        //        Gizmos.color = Color.red;
        //        Gizmos.DrawSphere(DefaultPosition, .5f);
        //    }
        //}
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

            if (EditorApplication.isPlaying)
            {
                GUILayout.Space(5);
                GUILayout.Label($"Timeline Time: {timelineTime.floatValue}");

                EditorGUI.BeginChangeCheck();
                sliderTime = EditorGUILayout.Slider("timeline: ", sliderTime, 0, timeline.MaxTime);

                if (EditorGUI.EndChangeCheck())
                {
                    timeline.Jump(sliderTime);
                }
            }

            // read-only mode
            // if (EditorApplication.isPlaying == false)
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