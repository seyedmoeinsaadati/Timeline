using UnityEngine;
using UnityEngine.XR.WSA.Input;

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

        public static AnimationCurve[] ToAnimationCurve(TransformComponent self, float interval)
        {
            // if you need to record scale, add scale fields in following curve array
            // position (x, y, z), rotation (x, y, z, w)
            AnimationCurve[] curves = new AnimationCurve[7];
            curves[0] = new AnimationCurve();
            curves[1] = new AnimationCurve();
            curves[2] = new AnimationCurve();
            curves[3] = new AnimationCurve();
            curves[4] = new AnimationCurve();
            curves[5] = new AnimationCurve();
            curves[6] = new AnimationCurve();

            if (self.CaptureCount == 0) return curves;

            for (int i = 0; i < self.tape.Count; i++)
            {
                float time = i * interval;
                var point = self.tape[i];

                // position
                curves[0].AddKey(time, point.position.x);
                curves[1].AddKey(time, point.position.y);
                curves[2].AddKey(time, point.position.z);

                // rotation
                curves[3].AddKey(time, point.rotation.x);
                curves[4].AddKey(time, point.rotation.y);
                curves[5].AddKey(time, point.rotation.z);
                curves[6].AddKey(time, point.rotation.w);
            }

            return curves;
        }
    }
}