using Moein.TimeSystem;
using UnityEditor;
using UnityEngine;


namespace Moein.TimeSystem
{
    public class Timeline : MonoBehaviour
    {
        [SerializeField] public bool withMemory;
        [SerializeField] public float captureInterval = .5f;
        // private Timeline[] children;

        [HideInInspector] public float time;
        [HideInInspector] public int timePointer;
        [HideInInspector] public int headIndex;
        private float lerpT;
        [HideInInspector] public float lastCapturingTime;

        private void Start()
        {
            timePointer = headIndex = -1;
            InitComponents();
            // children = GetComponentsInChildren<Timeline>();
        }

        public void Progress(float timeScale)
        {
            time += Time.fixedDeltaTime * timeScale;
            timePointer = (int) (time / captureInterval);

            if (withMemory)
            {
                if (timePointer < headIndex)
                {
                    // forwarding
                    lerpT = (time - timePointer * captureInterval) / captureInterval;
                    transformTimeline.ApplySnapshot(transformTimeline.LerpSnapshot(timePointer, timePointer + 1,
                        lerpT));

                    return;
                }
            }

            if (timePointer < headIndex) headIndex = timePointer;
            Capture();
        }

        public void Rewind(float timeScale)
        {
            time += Time.fixedDeltaTime * timeScale;
            time = Mathf.Max(0, time);

            timePointer = (int) (time / captureInterval);
            lerpT = (time - timePointer * captureInterval) / captureInterval;

            if (timePointer == headIndex)
            {
                transformTimeline.ApplySnapshot(transformTimeline.LerpSnapshot(transformTimeline.HeadSnapshot,
                    transformTimeline.CurrentSnapshot, lerpT));
                return;
            }

            transformTimeline.ApplySnapshot(transformTimeline.LerpSnapshot(timePointer, timePointer + 1, lerpT));
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
            transformTimeline.CaptureSnapshot();
        }

        #endregion


        #region TimelineComponents

        private TransformComponent transformTimeline;

        private void InitComponents()
        {
            transformTimeline = new TransformComponent(transform);
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
            GUILayout.Label($"Last Capturing Time: {timeline.lastCapturingTime}");
            GUILayout.Label($"Time: {timeline.time}");
            GUILayout.Label($"Head Index: {timeline.headIndex}");
            GUILayout.Label($"Current Pointer: {timeline.timePointer}");
        }
    }

#endif
}