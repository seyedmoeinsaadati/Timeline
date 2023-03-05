using System.Collections.Generic;
using UnityEngine;

namespace Moein.TimeSystem
{
    public class TimelineController : TimelineControllerBase
    {
        public bool findTimelinesAtStart;

        private void Start()
        {
            if (findTimelinesAtStart)
            {
                timelines.AddRange(FindObjectsOfType<TimelineBase>());
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