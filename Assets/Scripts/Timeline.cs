using Moein.TimeSystem;
using UnityEditor;
using UnityEngine;


namespace Moein.TimeSystem
{
    public class Timeline : MonoBehaviour
    {
        [SerializeField] public float captureInterval = .5f;

        [SerializeField] private StoreType storeType = StoreType.NoMemory;
        // private Timeline[] children;

        [HideInInspector] public float time;
        [HideInInspector] public int timePointer;
        [HideInInspector] public int headIndex;
        private float lerpT;
        [HideInInspector] public float lastCapturingTime;


        private void Start()
        {
            timePointer = headIndex = -1;
            capturingTimer = captureInterval;

            InitComponents();
            // children = GetComponentsInChildren<Timeline>();
        }

        public void Progress(float timeScale)
        {
            time += Time.fixedDeltaTime * timeScale;
            time = Mathf.Max(0, time);
            timePointer = (int) (time / captureInterval);

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

        private float capturingTimer;

        private void Capture()
        {
            capturingTimer += Time.fixedDeltaTime;
            if (capturingTimer >= captureInterval)
            {
                CaptureSnapshots();
                capturingTimer = 0;
                headIndex++;
            }
        }


        private void CaptureSnapshots()
        {
            // if (currentPointer != headIndex) return;
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