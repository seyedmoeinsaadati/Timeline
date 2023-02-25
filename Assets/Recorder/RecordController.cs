using UnityEngine;
using System.Collections;

namespace Moein.Recorder
{
    [RequireComponent(typeof(Record))]
    public class RecordController : MonoBehaviour
    {
        public KeyCode recordKey = KeyCode.M;
        public KeyCode pauseKey = KeyCode.N;
        public KeyCode forwardPlayKey = KeyCode.B;
        public KeyCode backwardPlayKey = KeyCode.V;

        Record target;

        void Awake()
        {
            target = GetComponent<Record>();
        }

        void Update()
        {
            Control();
        }

        void Control()
        {
            if (Input.GetKeyDown(recordKey))
            {
                target.DoRecord();
            }

            if (Input.GetKeyDown(pauseKey))
            {
                target.DoPause();
            }

            if (Input.GetKeyDown(forwardPlayKey))
            {
                target.DoPlay();
            }

            if (Input.GetKeyDown(backwardPlayKey))
            {
                target.DoPlay(true);
            }
        }
    }
}