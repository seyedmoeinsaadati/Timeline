using UnityEngine;

namespace Moein.TimeSystem
{
    public class AudioSourceComponent : TimelineComponent<AudioSource, float>
    {
        public AudioSourceComponent(AudioSource component, int tapeSize) : base(component, tapeSize)
        {
        }

        public override float CurrentSnapshot => component.pitch;

        public override void CaptureSnapshot(int index)
        {
            tape[index] = component.pitch;
        }

        public override void ApplySnapshot(float snapshot)
        {
            component.pitch = snapshot;
        }

        public override float LerpSnapshot(float from, float to, float t)
        {
            return Mathf.Lerp(from, to, t);
        }
    }
}