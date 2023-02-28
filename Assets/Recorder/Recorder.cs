using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Moein.Timeline
{
    public enum RecordState
    {
        Stop,
        Recording,
        ForwardPlaying,
        BackwardPlaying,
        Pause
    }

    public enum HandlingType
    {
        Manual
    }

    public class Recorder : MonoBehaviour
    {
        [Header("Config")] [SerializeField] private RecordState state;

        [SerializeField] private string takeName = "TakeName";

        [SerializeField, Min(1), Tooltip("Increase value after record")]
        private int takeNumber = 1;

        [SerializeField] private HandlingType handlingType;
        [SerializeField] private float capturingInterval = .5f;
        [SerializeField] private float delay;

        [Header("Objects"), Space(5)] [SerializeField]
        private List<Transform> transforms;

        private List<TransformRecordModel> transformRecordModels;

        [Header("Control Key"), Space(5)] public KeyCode startRecordKey = KeyCode.R;
        public KeyCode stopRecordKey = KeyCode.E;

        private float startRecordTime;

        public string DirectoryName => $"{takeName}_{takeNumber}";

        #region BeforeRecording

        private void Start()
        {
            FindObjects();
            SetRecordConfig();
        }

        private void FindObjects()
        {
            // find objects
            transforms = new List<Transform>();
            transforms.AddRange(transform.GetComponentsInChildren<Transform>(false));

            if (transforms.Count > 0)
            {
                // create recordModels from Objects
                transformRecordModels = new List<TransformRecordModel>();
                for (int i = 0; i < transforms.Count; i++)
                {
                    var model = new TransformRecordModel();
                    model.SetTargetComponent(transforms[i]);
                    model.fileName = GetFileName(transforms[i]);
                    transforms[i].name = $"{model.fileName} ({transforms[i].name})";
                    transformRecordModels.Add(model);
                }
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
            state = RecordState.Recording;
            startRecordTime = Time.time;
        }

        public void StopRecording()
        {
            state = RecordState.Stop;
            Save();
            takeNumber++;

            Debug.Log($"Record Finished. Record Time{Time.time - startRecordTime}");
            startRecordTime = 0;
            // load tape for ready to play
            // LoadFile();
        }

        // timers
        private float capturingTimer = 0;
        private float recordingTimer = 0;
        private float delayTimer = 0;

        private void Update()
        {
            if (Input.GetKeyDown(startRecordKey))
                StartRecording();
            else if (Input.GetKeyDown(stopRecordKey))
                StopRecording();
        }

        // capturing data per inteval 
        private void FixedUpdate()
        {
            if (state == RecordState.Recording)
            {
                capturingTimer += Time.fixedDeltaTime;
                if (capturingTimer > capturingInterval)
                {
                    Capture();
                    capturingTimer = 0;
                }
            }
        }

        private void Capture()
        {
            for (int i = 0; i < transformRecordModels.Count; i++)
            {
                transformRecordModels[i].CaptureData();
            }
        }

        #endregion

        #region Save/Load

        private void Save()
        {
            for (int i = 0; i < transformRecordModels.Count; i++)
            {
                transformRecordModels[i].Save(DirectoryName);
            }
        }

        private void Load()
        {
            for (int i = 0; i < transformRecordModels.Count; i++)
            {
                transformRecordModels[i].Load(DirectoryName);
            }
        }

        #endregion

        void OnGUI()
        {
            GUILayout.Label(state.ToString());
        }

        private void OnValidate()
        {
            if (handlingType == HandlingType.Manual)
                delay = -1;
        }

        // lerp =>
        // a: prevTapeIndex
        // b: nextTapeIndex
        // t: capturingTime/capturingInterval
    }

#if UNITY_EDITOR

    // [CustomEditor(typeof(Recorder))]
    // public class RecorderEditor : Editor
    // {
    //     public override void OnInspectorGUI()
    //     {
    //         base.OnInspectorGUI();
    //     }
    // }

#endif
}


//    case RecordState.ForwardPlaying:
//                    if (index > tape.Count - 1)
//{
//    // end of tape
//    ChangeState(RecordState.Stop);
//    OnFinishPlay?.Invoke();
//    break;
//}
//// Load and Update() new Transform
//ti = tape[index++];
//UpdateTransform(ti);
//break;
//                case RecordState.BackwardPlaying:
//                    if (index < 0)
//{
//    // end of tape
//    ChangeState(RecordState.Stop);
//    OnFinishPlay?.Invoke();
//    break;
//}
//// Load and Update() new Transform
//ti = tape[index--];
//UpdateTransform(ti);
//break;
//                case RecordState.Stop:
//                    break;

//public void DoPlay(bool isInverse = false)
//{
//    if (state == RecordState.Recording)
//    {
//        return;
//    }

//    isBackward = isInverse;

//    if (isBackward)
//    {
//        index = tape.Count - 1;
//        ChangeState(RecordState.BackwardPlaying);
//    }
//    else
//    {
//        index = 0;
//        ChangeState(RecordState.ForwardPlaying);
//    }
//    OnPlay?.Invoke();

//}

//public void DoPause()
//{
//    if (state == RecordState.Recording)
//    {
//        return;
//    }
//    if (state == RecordState.ForwardPlaying)
//    {
//        ChangeState(RecordState.Pause);
//    }
//    if (state == RecordState.BackwardPlaying)
//    {
//        ChangeState(RecordState.Pause);
//    }
//    OnPause?.Invoke();
//}