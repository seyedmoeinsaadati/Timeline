using System.Collections.Generic;
using UnityEngine;

namespace Moein.Timeline
{
    public class TransformComponent : TimelineComponent<Transform, TransformSnapshot>
    {
        public TransformComponent(Transform component) : base(component){}

        public override void CaptureSnapshot()
        {
            tape.Add(new TransformSnapshot(component.localPosition, component.localRotation));
        }

        public override void ApplySnapshot(TransformSnapshot snapshot)
        {
            component.localPosition = snapshot.position;
            component.localRotation = snapshot.rotation;
        }
    }
}