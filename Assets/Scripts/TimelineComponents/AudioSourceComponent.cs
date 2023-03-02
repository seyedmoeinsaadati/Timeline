using System.Collections.Generic;
using UnityEngine;

namespace Moein.TimeSystem
{
    public class AudioSourceComponent : TimelineComponent<AudioSource, float>
    {
        public AudioSourceComponent(AudioSource component) : base(component)
        {
        }

        public override float CurrentSnapshot => component.pitch;

        public override void CaptureSnapshot()
        {
            tape.Add(component.pitch);
        }

        public override void ApplySnapshot(float snapshot)
        {
            component.pitch = snapshot;
        }

        public override void LerpSnapshot(int index1, int index2, float t)
        {
            ApplySnapshot(Mathf.Lerp(tape[index1], tape[index2], t));
        }
    }
}