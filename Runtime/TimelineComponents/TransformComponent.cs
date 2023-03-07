using UnityEngine;

namespace Moein.TimeSystem
{
    public class TransformComponent : TimelineComponent<Transform, TransformSnapshot>
    {
        public TransformComponent(Transform component) : base(component)
        {
        }

        public TransformComponent(Transform component, int tapeSize) : base(component, tapeSize)
        {
        }

        public override TransformSnapshot CurrentSnapshot
        {
            get { return new TransformSnapshot(component.localPosition, component.localRotation); }
        }

        public override void CaptureSnapshot(int index)
        {
            if (index >= CaptureCount)
                CaptureSnapshot();
            else
                tape[index] = new TransformSnapshot(component.localPosition, component.localRotation);
        }

        public override void CaptureSnapshot()
        {
            tape.Add(new TransformSnapshot(component.localPosition, component.localRotation));
        }

        public override void CaptureSnapshot(int index, TransformSnapshot snapshot)
        {
            if (index >= CaptureCount)
                CaptureSnapshot();
            else
                tape[index] = new TransformSnapshot(component.localPosition, component.localRotation);
        }

        public override void CaptureSnapshot(TransformSnapshot snapshot)
        {
            tape.Add(snapshot);
        }

        public override void ApplySnapshot(TransformSnapshot snapshot)
        {
            component.localPosition = snapshot.position;
            component.localRotation = snapshot.rotation;
        }

        public override TransformSnapshot LerpSnapshot(TransformSnapshot @from, TransformSnapshot to, float t)
        {
            return TransformSnapshot.Lerp(from, to, t);
        }
    }
}