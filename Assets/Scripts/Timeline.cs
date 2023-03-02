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
        [HideInInspector] public int headIndex = -1;
        private float lerpT;
        private int currentPointer;

        private void Start()
        {
            currentPointer = headIndex = -1;
            InitComponents();
            // children = GetComponentsInChildren<Timeline>();
        }

        public void Progress(float timeScale)
        {
            time += Time.fixedDeltaTime * timeScale;
            time = Mathf.Max(0, time);

            // currentPointer = (int) (time / captureInterval);
            Capture();

            // if (storeType == StoreType.NoMemory)
            // {
            //     Capture();
            // }
            // else
            // {
            //     lerpT = timeScale == 0 ? lerpT : time - (currentPointer * captureInterval);
            //     transformTimeline.ApplySnapshot(transformTimeline.LerpSnapshot(currentPointer, currentPointer + 1,
            //         lerpT));
            // }
            //
        }

        public void Rewind(float timeScale)
        {
            time += Time.fixedDeltaTime * timeScale;
            time = Mathf.Max(0, time);
            currentPointer = (int) (time / captureInterval);
            lerpT = (time - currentPointer * captureInterval) / captureInterval;

            //Debug.Log($"Current Pointer: {currentPointer}");

            if (currentPointer == headIndex + 1)
            {
                transformTimeline.ApplySnapshot(transformTimeline.LerpSnapshot(currentPointer - 1, currentPointer - 1,
                    lerpT));
                return;
            } 

            transformTimeline.ApplySnapshot(transformTimeline.LerpSnapshot(currentPointer, currentPointer + 1, lerpT));
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
            }
        }

        private void CaptureSnapshots()
        {
            // if (currentPointer != headIndex) return;
            transformTimeline.CaptureSnapshot();
            headIndex++;
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
            GUILayout.Label($"Time: {timeline.time}");
            GUILayout.Label($"Head Index: {timeline.headIndex}");
        }
    }

#endif
}