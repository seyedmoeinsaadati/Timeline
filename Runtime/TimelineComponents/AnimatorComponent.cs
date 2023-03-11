using System;
using UnityEngine;

namespace Moein.TimeSystem
{
    public class AnimatorComponent : TimelineComponent<Animator, float>
    {
        private float speed = 1;
        private int recordedFrames = 100;
        private bool recording;

        public int RecordedFrames
        {
            get => recordedFrames;
            set => recordedFrames = Mathf.Clamp(value, 1, 10000);
        }

        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        public AnimatorComponent(Animator component) : base(component)
        {
            RecordedFrames = 10000;
        }

        public AnimatorComponent(Animator component, int maxTapeSize) : base(component, maxTapeSize)
        {
            RecordedFrames = 0;
        }

        public override float CurrentSnapshot => Speed;

        public override void CaptureSnapshot(int index)
        {
        }

        public override void CaptureSnapshot()
        {
        }

        public override void CaptureSnapshot(int index, float snapshot)
        {
        }

        /// <summary>
        /// recording management  
        /// </summary>
        /// <param name="timescale"></param>
        public override void CaptureSnapshot(float timescale)
        {
            component.speed = Speed * timescale;

            if (timescale > 0 && recording == false) // Stopped rewind
            {
                component.StopPlayback();
                component.StartRecording(recordedFrames);
                recording = true;
            }
        }

        /// <summary>
        /// playback management
        /// </summary>
        /// <param name="timescale"></param>
        public override void ApplySnapshot(float timescale)
        {
            if (recording && timescale < 0) // Start rewinding
            {
                component.speed = 0;
                component.StopRecording();
                recording = false;
                component.StartPlayback();
                component.playbackTime = component.recorderStopTime;
                return;
            }

            component.speed = Speed * timescale;
            component.playbackTime =
                Mathf.Max(component.recorderStartTime, component.playbackTime + Time.fixedDeltaTime * timescale);
        }

        public override float LerpSnapshot(float from, float to, float t)
        {
            return to;
        }
    }
}