using Moein.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Moein.TimeSystem
{
    [RequireComponent(typeof(Collider))]
    public class Area3DTimelineController : MonoBehaviour
    {
        [SerializeField] private float enterTimeScale = -1;
        [SerializeField] private float exitTimeScale = 1;
        [SerializeField] private float factor = 1;

        [SerializeField] private LayerMask layers;

        private void OnTriggerEnter(Collider other)
        {
            var timeline = other.GetComponent<TimelineControllerBase>();
            if (timeline != null && layers.Contains(other.gameObject.layer))
            {
                timeline.TimeScale = enterTimeScale * factor;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var timeline = other.GetComponent<TimelineControllerBase>();
            if (timeline != null && layers.Contains(other.gameObject.layer))
            {
                timeline.TimeScale = exitTimeScale * factor;
            }
        }

    }
}