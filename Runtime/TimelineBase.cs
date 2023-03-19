using System;
using UnityEngine;

namespace Moein.TimeSystem
{
    [DefaultExecutionOrder(-100)]
    public abstract class TimelineBase : MonoBehaviour
    {
        [SerializeField] protected bool initialized;
        [HideInInspector, SerializeField] protected float timelineTime; // between 0, maxRecordingTime

        #region TimelineComponents

        // transform
        [SerializeField] protected float captureInterval = .5f;
        protected float t;
        protected int pointer;
        protected int maxTimelineCaptureCount;
        protected TransformComponent transformTimeline = null;
        protected AnimatorComponent animatorComponent;

        #endregion

        private void Start()
        {
            Init();
        }

        protected abstract void Init();

        public abstract void Progress(float timescale);

        public abstract void Rewind(float timescale);

        protected abstract void CalculatingTime();

        public abstract void Capture();
        protected abstract void Apply();
    }
}