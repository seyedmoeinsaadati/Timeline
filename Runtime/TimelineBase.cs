using System;
using UnityEngine;

namespace Moein.TimeSystem
{
    [DefaultExecutionOrder(-100)]
    public abstract class TimelineBase : MonoBehaviour
    {
        [SerializeField] protected bool initialized;
        [HideInInspector, SerializeField] protected float timelineTime; // between 0, recordingTime

        private void Start()
        {
            Init();
        }

        protected virtual void Init()
        {
            InitComponents();
            initialized = true;
        }

        public abstract void Progress(float timescale);

        public abstract void Rewind(float timescale);

        protected abstract void CalculateLerping(float timescale);

        public abstract void Capture();

        #region TimelineComponents

        // transform
        [SerializeField] protected float captureInterval = .5f;
        protected float t;
        protected int pointer;
        protected int maxTimelineCaptureCount;
        protected TransformComponent transformTimeline = null;

        protected virtual void InitComponents()
        {
            transformTimeline = new TransformComponent(transform, maxTimelineCaptureCount);
        }

        protected abstract void CaptureComponents();

        protected abstract void ApplyComponents();

        #endregion
    }
}