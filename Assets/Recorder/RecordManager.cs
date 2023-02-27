using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Moein.TimeRecorder
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
        Delay,
        Manual,
        ManualAndDelay
    }

    public class RecordManager : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private string takeName;
        [SerializeField, Tooltip("Increase value after record")] private int takeNumber;
        [SerializeField] private HandlingType handlingType;
        [SerializeField] private RecordState state;
        [SerializeField] private float capturingInterval = .5f;
        [SerializeField] private float startRecordTime;
        [SerializeField] private float endRecordTime;
        public float delay;

        [Header("Objects"), Space(10)]
        private List<TransformRecordModel> transformRecordModels;
        private List<Transform> transforms;

        [Header("Control Key"), Space(10)]
        public KeyCode recordKey = KeyCode.R;
        public KeyCode pauseKey = KeyCode.E;

        public string DirectoryName => $"{takeName}_{takeNumber}";

        public float RecordDuration => endRecordTime - startRecordTime;

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
                transformRecordModels = new List<TransformRecordModel>(transforms.Count);
                for (int i = 0; i < transforms.Count; i++)
                {
                    transformRecordModels[i] = new TransformRecordModel();
                    transformRecordModels[i].SetTargetComponent(transforms[i]);
                    transformRecordModels[i].fileName = GetFileName(transforms[i]);
                    transforms[i].name = $"{transformRecordModels[i].fileName} ({transforms[i].name})";
                }
            }
        }

        private string GetFileName(Transform t)
        {
            if (t == transform) return "0";
            return GetFileName(transform.parent) + "_" + transform.GetSiblingIndex();
        }

        private void SetRecordConfig()
        {
            // set recordModels to record
        }

        #endregion

        #region Recording

        // timers
        private float capturingTimer = 0;
        private float recordingTimer = 0;
        private float delayTimer = 0;

        // capturing data per inteval 
        private void FixedUpdate()
        {
            if (state == RecordState.Recording)
            {
                capturingTimer += Time.fixedDeltaTime;
                if (capturingTimer > capturingInterval) Capture();
            }
        }

        private void Capture()
        {
            for (int i = 0; i < transformRecordModels.Count; i++)
            {
                transformRecordModels[i].CaptureData();
            }
        }

        public void SetStata(RecordState newState)
        {
            state = newState;
        }

        public void StartRecording()
        {
            SetStata(RecordState.Recording);
        }

        public void StopRecording()
        {
            takeNumber++;
            SetStata(RecordState.Stop);
            Save();

            // load tape for ready to play
            // LoadFile();
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
    }
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
