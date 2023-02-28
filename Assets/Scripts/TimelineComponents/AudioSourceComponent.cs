using System.Collections.Generic;
using UnityEngine;

namespace Moein.Timeline
{
    public class AudioSourceCompontent : TimelineComponent<AudioSource, float>
    {
        public AudioSourceCompontent(AudioSource component) : base(component) { }

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