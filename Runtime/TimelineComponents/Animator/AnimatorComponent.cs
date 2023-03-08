using System;
using Moein.ComponentInterface;
using UnityEngine;

namespace Moein.TimeSystem
{
    public class AnimatorComponent : TimelineComponent<RewindableAnimator, AnimatorSnapshot>
    {
        public AnimatorComponent(RewindableAnimator component) : base(component)
        {
        }

        public AnimatorComponent(RewindableAnimator component, int maxTapeSize) : base(component, maxTapeSize)
        {
        }

        public override AnimatorSnapshot CurrentSnapshot => null;

        public override void CaptureSnapshot(int index)
        {
            throw new NotImplementedException();
        }

        public override void CaptureSnapshot()
        {
            throw new NotImplementedException();
        }

        public override void CaptureSnapshot(int index, AnimatorSnapshot snapshot)
        {
            if (index >= CaptureCount) CaptureSnapshot(snapshot);
            else tape[index] = snapshot;
        }

        public override void CaptureSnapshot(AnimatorSnapshot snapshot)
        {
            tape.Add(snapshot);
        }

        public override void ApplySnapshot(AnimatorSnapshot snapshot)
        {
            component.ApplySnapshot(snapshot);
        }

        public override AnimatorSnapshot LerpSnapshot(AnimatorSnapshot from, AnimatorSnapshot to, float t)
        {
            return to;
        }
    }
}