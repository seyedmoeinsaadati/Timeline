using System.Collections.Generic;
using UnityEngine;

namespace Moein.TimeSystem
{
    public abstract class TimelineControllerBase : MonoBehaviour
    {
        [SerializeField] protected float timeScale = 1;
        [SerializeField] protected List<TimelineBase> timelines = new List<TimelineBase>();

        protected void RewindTimelines()
        {
            for (int i = 0; i < timelines.Count; i++)
            {
                timelines[i].Rewind(timeScale);
            }
        }

        protected void ProgressTimelines()
        {
            for (int i = 0; i < timelines.Count; i++)
            {
                timelines[i].Progress(timeScale);
            }
        }

        public void Register(TimelineBase timeline)
        {
            if (timelines.Contains(timeline) == false)
            {
                timelines.Add(timeline);
            }
        }

        public void UnRegister(TimelineBase timeline)
        {
            if (timelines.Contains(timeline))
            {
                timelines.Remove(timeline);
            }
        }

        void OnDrawGizmos()
        {
            if (timeScale > 0)
            {
                Gizmos.DrawIcon(transform.position, "forward_icon.png", true);
            }
            else if (timeScale == 0)
            {
                Gizmos.DrawIcon(transform.position, "stop_icon.png", true);
            }
            else if (timeScale < 0)
            {
                Gizmos.DrawIcon(transform.position, "backward_icon.png", true);
            }
        }
    }
}