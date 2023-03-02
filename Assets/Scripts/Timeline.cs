using Moein.TimeSystem;
using UnityEditor;
using UnityEngine;


namespace Moein.TimeSystem
{
    public class Timeline : MonoBehaviour
    {
        [SerializeField] public bool saveMemory;
        [SerializeField] public float captureInterval = .5f;

        [SerializeField, Min(1)] public float memoryTime = 30f;
        // private Timeline[] children;

        [HideInInspector] public float time;
        [HideInInspector] public int pointer;
        [HideInInspector] public int headIndex;
        private float lerpT;
        [HideInInspector] public float lastFulltime;
        public int maxTimelineCaptureCount;

        private void Start()
        {
            pointer = headIndex = -1;
            maxTimelineCaptureCount = Mathf.RoundToInt(memoryTime / captureInterval);

            InitComponents();
            // children = GetComponentsInChildren<Timeline>();
        }

        public void Progress(float timeScale)
        {
            time += Time.fixedDeltaTime * timeScale;
            time = Mathf.Clamp(time, 0, memoryTime);
            pointer = (int) (time / captureInterval);

            if (pointer < headIndex)
            {
                if (saveMemory)
                {
                    // forwarding
                    lerpT = (time - pointer * captureInterval) / captureInterval;
                    transformTimeline.ApplySnapshot(transformTimeline.LerpSnapshot(pointer, pointer + 1, lerpT));
                    return;
                }

                headIndex = pointer;
            }

            Capture();
        }

        public void Rewind(float timeScale)
        {
            time += Time.fixedDeltaTime * timeScale;
            time = Mathf.Clamp(time, 0, memoryTime);

            pointer = (int) (time / captureInterval);
            lerpT = (time - pointer * captureInterval) / captureInterval;

            if (pointer == headIndex)
            {
                transformTimeline.ApplySnapshot(transformTimeline.LerpSnapshot(transformTimeline.HeadSnapshot,
                    transformTimeline.CurrentSnapshot, lerpT));
                return;
            }

            transformTimeline.ApplySnapshot(transformTimeline.LerpSnapshot(pointer, pointer + 1, lerpT));
        }

        #region Capturing

        private void Capture()
        {
            if (time >= captureInterval * (headIndex + 1))
            {
                headIndex++;
                CaptureSnapshots();
            }
        }

        private void CaptureSnapshots()
        {
            if (headIndex >= maxTimelineCaptureCount)
            {
                transformTimeline.Tape.RemoveAt(0);
                headIndex = maxTimelineCaptureCount - 1;
                time -= captureInterval;
            }

            transformTimeline.CaptureSnapshot(headIndex);
        }

        #endregion


        #region TimelineComponents

        private TransformComponent transformTimeline;

        private void InitComponents()
        {
            transformTimeline = new TransformComponent(transform, maxTimelineCaptureCount);
            Debug.Log($"transformTimeline {transformTimeline.Tape.Capacity}");
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(Timeline))]
    public class TimelineEditor : Editor
    {
        private Timeline timeline;

        private void OnEnable()
        {
            timeline = target as Timeline;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(5);
            GUILayout.Label($"Last Capturing Time: {timeline.lastFulltime}");
            GUILayout.Label($"Time: {timeline.time}");
            GUILayout.Label($"Head Index: {timeline.headIndex}");
            GUILayout.Label($"Current Pointer: {timeline.pointer}");
        }
    }

#endif
}