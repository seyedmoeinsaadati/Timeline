using System;
using UnityEngine;

namespace Moein.TimeSystem
{
    [Serializable]
    public class TransformSnapshot
    {
        public SerializableVector3 position;

        public SerializableQuaternion rotation;
        // public SerializableVector3 scale;

        public TransformSnapshot(Vector3 position, Quaternion rotation /*, Vector3 scale*/)
        {
            this.position = new SerializableVector3(position);
            this.rotation = new SerializableQuaternion(rotation);
            // this.scale = new SerializableVector3(scale);
        }

        public static TransformSnapshot Lerp(TransformSnapshot a, TransformSnapshot b, float t)
        {
            var pos = Vector3.Lerp(a.position, b.position, t);
            var rot = Quaternion.Slerp(a.rotation, b.rotation, t);

            return new TransformSnapshot(pos, rot);
        }
    }

    public class AnimatorSnapshot
    {
        public enum ActionType
        {
            None = 0,
            Bool = 1,
            Float = 2,
            Int = 3,
            Trigger = 4
        }

        public ActionType type;
        public float time;
        public string name;
        public object value;

        public AnimatorSnapshot(float time, string name, object value, ActionType type)
        {
            this.time = time;
            this.type = type;
            this.name = name;
            this.value = value;
        }
    }
}