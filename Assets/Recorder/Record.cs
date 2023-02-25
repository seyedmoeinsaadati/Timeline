using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Moein.Recorder
{
    public enum RecordState
    {
        Stop,
        Recording,
        ForwardPlaying,
        BackwardPlaying,
        Pause
    }

    public class Record : MonoBehaviour
    {
        [SerializeField]
        GameObject playObject;
        public string fileName;
        public string directoryName;

        public RecordState state;
        List<TransformInfo> tape;
        public int index;

        public bool recordOnAwake;
        public bool isBackward;

        // initial position
        TransformInfo offset;

        public UnityEvent OnPlay, OnFinishPlay, OnRecordStart, OnRecordStop, OnPause;

        #region UnityMethods

        void Awake()
        {
            if (playObject == null)
            {
                playObject = gameObject;
            }
            tape = new List<TransformInfo>();
        }

        void Start()
        {
            offset = new TransformInfo(transform.localPosition, transform.localRotation);
            if (recordOnAwake)
            {
                StartRecord();
            }
        }

        void FixedUpdate()
        {
            TransformInfo ti;
            switch (state)
            {
                case RecordState.Recording:

                    Quaternion _localRotation = transform.localRotation;
                    _localRotation.eulerAngles = transform.localRotation.eulerAngles;// - offset.GetEularAngles ();

                    ti = new TransformInfo(transform.localPosition - offset.GetPosition(), _localRotation);

                    // store tansform data with forward form
                    tape.Add(ti);

                    break;
                case RecordState.Pause:
                    break;
                case RecordState.ForwardPlaying:
                    if (index > tape.Count - 1)
                    {
                        // end of tape
                        ChangeState(RecordState.Stop);
                        OnFinishPlay?.Invoke();
                        break;
                    }
                    // Load and Update() new Transform
                    ti = tape[index++];
                    UpdateTransform(ti);
                    break;
                case RecordState.BackwardPlaying:
                    if (index < 0)
                    {
                        // end of tape
                        ChangeState(RecordState.Stop);
                        OnFinishPlay?.Invoke();
                        break;
                    }
                    // Load and Update() new Transform
                    ti = tape[index--];
                    UpdateTransform(ti);
                    break;
                case RecordState.Stop:
                    break;
            }
        }

        void UpdateTransform(TransformInfo newTransformInfo)
        {
            Quaternion myRotation = transform.localRotation;
            myRotation.eulerAngles = newTransformInfo.GetEularAngles();// + offset.GetEularAngles ();

            playObject.transform.localPosition = newTransformInfo.GetPosition() + offset.GetPosition();
            playObject.transform.localRotation = myRotation;
        }

        void OnGUI()
        {
            GUILayout.Label(state.ToString());
        }
        void OnDrawGizmos()
        {
            switch (state)
            {
                case RecordState.Recording:
                    Gizmos.color = Color.red;
                    break;
                case RecordState.Pause:
                    Gizmos.color = Color.blue;
                    break;
                case RecordState.ForwardPlaying:
                    Gizmos.color = Color.green;
                    break;
                case RecordState.BackwardPlaying:
                    Gizmos.color = Color.cyan;
                    break;
                case RecordState.Stop:
                    Gizmos.color = Color.black;
                    break;
            }
            Gizmos.DrawWireCube(transform.position, Vector3.one * 2);
        }

        #endregion

        #region RecordMethods

        public void Init()
        {
            LoadFile();
        }

        public void StartRecord()
        {
            tape = new List<TransformInfo>();
            ChangeState(RecordState.Recording);
        }

        public void StopRecord(float delay = 0)
        {
            ChangeState(RecordState.Stop);
            OnRecordStop?.Invoke();
            SaveToFile();

            // Trash code
            if (GetComponent<Rigidbody>() != null)
            {
                GetComponent<Rigidbody>().isKinematic = true;
            }
            //Destroy (GetComponent<Rigidbody> ());

            // load tape for ready to play
            LoadFile();
        }

        public void DoRecord()
        {
            if (state == RecordState.Recording)
            {
                ChangeState(RecordState.Stop);
                StopRecord();
            }
            else
            {
                ChangeState(RecordState.Recording);
                OnRecordStart?.Invoke();
            }

        }

        public void DoPlay(bool isInverse = false)
        {
            if (state == RecordState.Recording)
            {
                return;
            }

            isBackward = isInverse;

            if (isBackward)
            {
                index = tape.Count - 1;
                ChangeState(RecordState.BackwardPlaying);
            }
            else
            {
                index = 0;
                ChangeState(RecordState.ForwardPlaying);
            }
            OnPlay?.Invoke();

        }

        public void DoPause()
        {
            if (state == RecordState.Recording)
            {
                return;
            }
            if (state == RecordState.ForwardPlaying)
            {
                ChangeState(RecordState.Pause);
            }
            if (state == RecordState.BackwardPlaying)
            {
                ChangeState(RecordState.Pause);
            }
            OnPause?.Invoke();
        }

        public void ChangeState(RecordState newState)
        {
            state = newState;
        }

        #endregion

        #region File

        void SaveToFile()
        {
            FileHandler.Save<TransformInfo>(directoryName, fileName, tape);
        }

        void LoadFile()
        {
            //tape = FileHandler.Load<TransformInfo> (fileName);
            tape = UnityFileHandler.Instance.Load<TransformInfo>(directoryName, fileName);
        }

        #endregion
    }
}