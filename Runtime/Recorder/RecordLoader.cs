using UnityEngine;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Moein.TimeSystem
{

    public class RecordLoader : MonoBehaviour
    {
        [Serializable]
        public class TakeInfo
        {
            public string takeName;
            public string takeNumber;
            public bool reverse;
        }

        [SerializeField] private float captureInterval = .5f;

        [SerializeField] private bool autoLoad;
        [SerializeField] private bool loadOnHead;
        [SerializeField] private bool renameTimelines;
        [SerializeField] private List<TakeInfo> takes = new List<TakeInfo>(0);

        private FileTimeline[] timelines = null;

        public TakeInfo MainTake => takes[0];

        public string DirectoryName => $"{MainTake.takeName}_{MainTake.takeNumber}";

        private void Start()
        {
            FindObjects();
            if (autoLoad) Load(loadOnHead);
        }

        private void FindObjects()
        {
            timelines = GetComponentsInChildren<FileTimeline>(false);

            for (int i = 0; i < timelines.Length; i++)
            {
                timelines[i].FileName = GetFileName(timelines[i].transform);
            }

            if (renameTimelines)
            {
                for (int i = 0; i < timelines.Length; i++)
                {
                    timelines[i].transform.name = $"{timelines[i].FileName} ({timelines[i].transform.name})";
                }
            }
        }

        public void Load(bool loadOnHead)
        {
            for (int j = 0; j < takes.Count; j++)
            {
                string directroy = $"{takes[j].takeName}_{takes[j].takeNumber}";
                for (int i = 0; i < timelines.Length; i++)
                {
                    timelines[i].AddTape(directroy, takes[j].reverse);
                }
            }
        }

        private string GetFileName(Transform t)
        {
            if (t == transform) return "0";
            return GetFileName(t.parent) + "_" + t.GetSiblingIndex();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(RecordLoader)), CanEditMultipleObjects]
    public class RecordLoaderEditor : Editor { }
#endif

}