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
}