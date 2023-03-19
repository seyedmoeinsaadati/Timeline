using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Moein.TimeSystem
{
    public class RecordLoader : MonoBehaviour
    {
        [SerializeField] private string takeName = "TakeName";
        [SerializeField] private int takeNumber = 1;

        [SerializeField] private float captureInterval = .5f;

        [SerializeField] private bool autoLoad;
        [SerializeField] private bool loadOnHead;
        [SerializeField] private bool renameTimelines;

        private FileTimeline[] timelines = null;

        public string DirectoryName => $"{takeName}_{takeNumber}";

        private void Start()
        {
            FindObjects();
            if (autoLoad) Load();
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

        public void Load()
        {
            for (int i = 0; i < timelines.Length; i++)
            {
                timelines[i].LoadComponents(DirectoryName, captureInterval, loadOnHead);
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