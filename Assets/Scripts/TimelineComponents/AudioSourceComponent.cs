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

    }
}