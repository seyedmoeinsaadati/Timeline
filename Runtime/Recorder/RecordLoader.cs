using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Moein.TimeSystem
{
    [Serializable]
    public class TakeInfo
    {
        public string takeName;
        public string takeNumber;
        public bool reverse;

        public string Directory => $"{takeName}_{takeNumber}";
    }

    public class RecordLoader : MonoBehaviour
    {
        [SerializeField] private float captureInterval = .5f;
        [SerializeField] private bool autoLoad;
        [SerializeField] private bool loadOnHead;
        [SerializeField] private bool renameTimelines;
        [SerializeField] private TakeInfo[] takes = new TakeInfo[1];

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
            for (int i = 0; i < timelines.Length; i++)
            {
                timelines[i].LoadComponents(captureInterval, loadOnHead, takes);
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