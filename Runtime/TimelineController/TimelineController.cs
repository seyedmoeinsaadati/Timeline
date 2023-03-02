﻿using System.Collections.Generic;
using UnityEngine;

namespace Moein.TimeSystem
{
    public class TimelineController : MonoBehaviour
    {
        [SerializeField] private float timeScale = 1;
        [SerializeField] private List<Timeline> timelines;

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

        private void RewindTimelines()
        {
            for (int i = 0; i < timelines.Count; i++)
            {
                timelines[i].Rewind(timeScale);
            }
        }

        private void ProgressTimelines()
        {
            for (int i = 0; i < timelines.Count; i++)
            {
                timelines[i].Progress(timeScale);
            }
        }

        public void Register(Timeline timeline)
        {
            if (timelines.Contains(timeline) == false)
            {
                timelines.Add(timeline);
            }
        }

        public void Remove(Timeline timeline)
        {
            if (timelines.Contains(timeline))
            {
                timelines.Remove(timeline);
            }
        }
    }
}