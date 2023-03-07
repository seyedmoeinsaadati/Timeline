using System;
using Moein.ComponentInterface;
using UnityEngine;

namespace Moein.TimeSystem
{
    public class AnimatorComponent : TimelineComponent<IAnimator, AnimatorSnapshot>
    {
        public AnimatorComponent(IAnimator component) : base(component)
        {
        }

        public AnimatorComponent(IAnimator component, int maxTapeSize) : base(component, maxTapeSize)
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
            switch (snapshot.type)
            {
                case AnimatorSnapshot.ActionType.Bool:
                    component.SetBool(snapshot.name, (bool) snapshot.value);
                    break;
                case AnimatorSnapshot.ActionType.Float:
                    component.SetFloat(snapshot.name, (float) snapshot.value);
                    break;
                case AnimatorSnapshot.ActionType.Int:
                    component.SetInteger(snapshot.name, (int) snapshot.value);
                    break;
                case AnimatorSnapshot.ActionType.Trigger:
                    component.SetTrigger((string) snapshot.value);
                    break;
            }
        }

        public override AnimatorSnapshot LerpSnapshot(AnimatorSnapshot from, AnimatorSnapshot to, float t)
        {
            return to;
        }
    }
}