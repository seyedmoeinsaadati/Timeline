using UnityEngine;
using System;
namespace Moein.Recorder
{

    public class RecordObjectNaming : MonoBehaviour
    {
        public string number, subject, groupname;
        Record[] recordObjs;
        public bool isLoad;
        public event Action<bool> onPlay;

        void Awake()
        {
            recordObjs = GetComponentsInChildren<Record>();
            foreach (var item in recordObjs)
            {
                item.directoryName = string.Format("{0}_{1}", number, subject);
                item.fileName = string.Format("{0}_{1}_{2}", groupname, item.transform.parent.name, item.name);
                if (isLoad)
                {
                    item.Init();
                }
            }
        }

        void Start()
        {
            foreach (var item in recordObjs)
            {
                onPlay += item.DoPlay;
            }
        }

        void OnDestroy()
        {
            foreach (var item in recordObjs)
            {
                onPlay -= item.DoPlay;
            }
        }

        public void Play(bool isInverse)
        {
            for (int i = 0; i < recordObjs.Length; i++)
            {
                recordObjs[i].DoPlay(isInverse);
            }
        }

        public string GetDiretory()
        {
            return string.Format("{0}_{1}", number, subject);
        }

        public string GetName(Transform _transform)
        {
            return string.Format("{0}_{1}_{2}", groupname, _transform.parent.name, _transform.name);
        }
    }
}