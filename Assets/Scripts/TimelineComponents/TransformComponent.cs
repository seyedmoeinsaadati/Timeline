using System.Collections.Generic;
using UnityEngine;

namespace Moein.Timeline
{
    public class TransformComponent : TimelineComponent<Transform, TransformSnapshot>
    {
        public TransformComponent(Transform component) : base(component) { }

        public override void CaptureSnapshot()
        {
            tape.Add(new TransformSnapshot(component.localPosition, component.localRotation));
        }

        public override void ApplySnapshot(TransformSnapshot snapshot)
        {
            component.localPosition = snapshot.position;
            component.localRotation = snapshot.rotation;
        }

        public override void LerpSnapshot(int index1, int index2, float t)
        {
            ApplySnapshot(TransformSnapshot.Lerp(tape[index1], tape[index2], t));
        }
    }
}