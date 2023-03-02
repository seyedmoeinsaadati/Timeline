using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Moein.TimeSystem
{
    public enum HandlingType
    {
        Manual
    }

    public class Recorder : MonoBehaviour
    {
        public enum RecorderState
        {
            Stop,
            Recording,
            Playing
        }

        [HideInInspector] public RecorderState state;


        #region Record Fields

        [Header("Record Fields")] [SerializeField]
        private string takeName = "TakeName";

        [SerializeField, Min(1), Tooltip("Increase value after record")]
        private int takeNumber = 1;

        [SerializeField] private float captureInterval = .5f;
        // [SerializeField] private HandlingType handlingType;

        [Header("Control Recording Key"), Space(5)]
        public KeyCode startRecordKey = KeyCode.R;

        public KeyCode stopKey = KeyCode.E;

        #endregion

        #region Player Fields

        [Header("Player Fields")] [SerializeField]
        private KeyCode forwardPlayKey = KeyCode.W;

        [SerializeField] private KeyCode backwardPlayKey = KeyCode.Q;
        public float timeScale = 1;

        #endregion

        private FileTimeline[] timelines = null;
        private float startRecordTime;
        private int captureCount;

        public string DirectoryName => $"{takeName}_{takeNumber}";
        public float RecordingTime => Time.time - startRecordTime;

        public int CaptureCount => captureCount;

        #region BeforeRecording

        private void Start()
        {
            FindObjects();
            SetRecordConfig();

            Load();
        }

        private void FindObjects()
        {
            timelines = GetComponentsInChildren<FileTimeline>(false);

            for (int i = 0; i < timelines.Length; i++)
            {
                timelines[i].FileName = GetFileName(timelines[i].transform);
                timelines[i].transform.name = $"{timelines[i].FileName} ({timelines[i].transform.name})";
            }
        }

        private string GetFileName(Transform t)
        {
            if (t == transform) return "0";
            return GetFileName(t.parent) + "_" + t.GetSiblingIndex();
        }

        private void SetRecordConfig()
        {
            // set recordModels to record
        }

        #endregion

        #region Recording

        public void StartRecording()
        {
            state = RecorderState.Recording;
            startRecordTime = Time.time;
        }

        public void StopRecording()
        {
            state = RecorderState.Stop;

            Save();

#if UNITY_EDITOR
            Debug.Log($"Record Finished. Record Time: {RecordingTime}");
            startRecordTime = 0;
#endif

            Load();

            takeNumber++;
        }

        private float capturingTimer = 0;

        private void FixedUpdate()
        {
            if (state == RecorderState.Recording)
            {
                capturingTimer += Time.fixedDeltaTime;
                if (capturingTimer > captureInterval)
                {
                    Capture();
                    capturingTimer = 0;
                }
            }
            else if (state == RecorderState.Playing)
            {
                if (timeScale >= 0)
                {
                    ProgressTimelines();
                }
                else
                {
                    RewindTimelines();
                }
            }
        }

        private void Capture()
        {
            for (int i = 0; i < timelines.Length; i++)
            {
                timelines[i].Capture();
            }

            captureCount++;
        }

        #endregion

        #region Playing

        public void StopPlaying()
        {
            state = RecorderState.Stop;
            timeScale = 0;
        }

        public void ForwardPlaying()
        {
            state = RecorderState.Playing;
            timeScale = 1;
        }

        public void BackwardPlaying()
        {
            state = RecorderState.Playing;
            timeScale = -1;
        }

        private void ProgressTimelines()
        {
            for (int i = 0; i < timelines.Length; i++)
            {
                timelines[i].Progress(timeScale);
            }
        }

        private void RewindTimelines()
        {
            for (int i = 0; i < timelines.Length; i++)
            {
                timelines[i].Rewind(timeScale);
            }
        }

        #endregion

        #region Save/Load

        private void Save()
        {
            for (int i = 0; i < timelines.Length; i++)
            {
                timelines[i].SaveComponents(DirectoryName);
            }
        }

        private void Load()
        {
            for (int i = 0; i < timelines.Length; i++)
            {
                timelines[i].LoadComponents(DirectoryName, captureInterval);
            }
        }

        #endregion

        private void Update()
        {
            if (Input.GetKeyDown(startRecordKey))
                StartRecording();
            else if (Input.GetKeyDown(stopKey))
            {
                if (state == RecorderState.Recording)
                {
                    StopRecording();
                }
                else if (state == RecorderState.Playing)
                {
                    StopPlaying();
                }
            }
            else if (Input.GetKeyDown(forwardPlayKey))
            {
                ForwardPlaying();
            }
            else if (Input.GetKeyDown(backwardPlayKey))
            {
                BackwardPlaying();
            }
        }

        private void OnGUI()
        {
            GUILayout.Space(10);

            GUILayout.Label($"Recording State: {state}");

            if (state == RecorderState.Recording)
            {
                GUILayout.Label($"Recording Time: {RecordingTime}");
                GUILayout.Label($"Capturing Count: {CaptureCount}");
            }
            else if (state == RecorderState.Playing)
            {
                GUILayout.Label($"Time Scale: {timeScale}");
            }
        }
    }

#if UNITY_EDITOR

    // [CustomEditor(typeof(Recorder))]
    // public class RecorderEditor : Editor
    // {
    //     private Recorder recorder;
    //
    //     private void OnEnable()
    //     {
    //         recorder = target as Recorder;
    //     }
    //
    //     public override void OnInspectorGUI()
    //     {
    //         base.OnInspectorGUI();
    //
    //         // if (Application.isPlaying)
    //         // {
    //
    //         GUILayout.Space(10);
    //         GUILayout.BeginHorizontal();
    //         if (GUILayout.Button("<"))
    //         {
    //             recorder.state = Recorder.RecorderState.Playing;
    //             recorder.timeScale = -1;
    //         }
    //
    //         if (GUILayout.Button("R"))
    //         {
    //             recorder.state = Recorder.RecorderState.Recording;
    //             recorder.timeScale = -1;
    //         }
    //
    //         if (GUILayout.Button("P"))
    //         {
    //         }
    //
    //         if (GUILayout.Button(">"))
    //         {
    //             recorder.state = Recorder.RecorderState.Playing;
    //             recorder.timeScale = 1;
    //         }
    //
    //
    //         GUILayout.EndHorizontal();
    //         // }
    //     }
    // }

#endif
}