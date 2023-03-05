using Moein.Core;
using UnityEngine;

namespace Moein.TimeSystem
{
    [RequireComponent(typeof(Collider))]
    public class Area3DTimelineController : TimelineControllerBase
    {
        //public enum AreaMode
        //{
        //    Constant,
        //    DistanceFromCenter
        //}

        [SerializeField] private LayerMask layers;

        private void OnTriggerEnter(Collider other)
        {
            var timeline = other.GetComponent<TimelineBase>();
            if (timeline != null && layers.Contains(other.gameObject.layer))
            {
                Register(timeline);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var timeline = other.GetComponent<TimelineBase>();
            if (timeline != null && layers.Contains(other.gameObject.layer))
            {
                UnRegister(timeline);
            }
        }

        private void FixedUpdate()
        {
            if (timeScale >= 0)
            {
                ProgressTimelines();
            }
            else if (timeScale < 0)
            {
                RewindTimelines();
            }
        }
    }
}