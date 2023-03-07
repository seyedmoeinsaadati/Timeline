using UnityEngine;

namespace Moein.TimeSystem
{
    [DefaultExecutionOrder(-100)]
    public abstract class TimelineBase : MonoBehaviour
    {
        [SerializeField] protected bool initilized;
        [HideInInspector, SerializeField] protected float timelineTime; // between 0, recordingTime


        private void Start()
        {
            Init();
        }

        protected virtual void Init()
        {
            InitComponents();
            initilized = true;
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


        // animator
        // protected int animatorHeadIndex;
        // protected int animatorPointer;
        // protected float AnimatorLastCapturingTime => animatorComponent.HeadSnapshot.time;
        // protected float AnimatorCurrentCapturingTime => animatorComponent.Tape[animatorPointer].time;
        // protected RewindableAnimator rewindableAnimator = null;
        // protected AnimatorComponent animatorComponent = null;

        protected virtual void InitComponents()
        {
            transformTimeline = new TransformComponent(transform, maxTimelineCaptureCount);

            // rewindableAnimator = GetComponent<RewindableAnimator>();
            // if (rewindableAnimator != null)
            // {
            //     animatorHeadIndex = animatorPointer = -1;
            //     animatorComponent = new AnimatorComponent(rewindableAnimator);
            //     rewindableAnimator.OnChangeAnimator(CaptureAnimator);
            // }
        }

        protected abstract void CaptureComponents();

        protected abstract void ApplyComponents();

        // protected virtual void CaptureAnimator(AnimatorSnapshot snapshot)
        // {
        //     snapshot.time = timelineTime;
        //     animatorHeadIndex++;
        //     animatorPointer = animatorHeadIndex;
        //     animatorComponent?.CaptureSnapshot(snapshot);
        //
        //     Debug.Log($"{GetType().Name}: Animator Captured. Time: {timelineTime}");
        // }

        #endregion
    }
}