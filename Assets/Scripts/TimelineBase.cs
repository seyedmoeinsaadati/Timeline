using UnityEditor;
using UnityEngine;

namespace Moein.TimeSystem
{
    public abstract class TimelineBase : MonoBehaviour
    {
        [SerializeField] protected float captureInterval = .5f;
        [SerializeField, Min(1)] protected float recordingTime = 30f;

        protected float t;
        [HideInInspector, SerializeField] protected float timelineTime; // between 0, recordingTime
        protected int pointer;
        protected int maxTimelineCaptureCount;

        private void Start()
        {
            Init();
        }

        protected virtual void Init()
        {
            InitComponents();
        }

        public virtual void Progress(float timeScale)
        {
        }

        public virtual void Rewind(float timeScale)
        {
        }

        protected virtual void CalculateLerping(float timeScale)
        {
        }

        public virtual void Capture()
        {
        }

        #region TimelineComponents

        protected TransformComponent transformTimeline;

        protected virtual void InitComponents()
        {
            transformTimeline = new TransformComponent(transform, maxTimelineCaptureCount);
        }

        protected virtual void CaptureComponents()
        {
        }

        protected virtual void ApplyComponents()
        {
        }

        #endregion
    }
}