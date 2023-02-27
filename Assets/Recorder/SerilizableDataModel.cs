using UnityEngine;

namespace Moein.TimeRecorder
{
    [System.Serializable]
    public class SerializableVector3
    {
        private float x, y, z;

        public SerializableVector3(Vector3 v3)
        {
            x = v3.x;
            y = v3.y;
            z = v3.z;
        }

        public static implicit operator Vector3(SerializableVector3 value)
        {
            return new Vector3(value.x, value.y, value.z);
        }
    }

    [System.Serializable]
    public class TransformModel
    {
        public SerializableVector3 position;
        public SerializableVector3 eulerAngles;
        // public SerializableVector3 scale;

        public TransformModel(Vector3 position, Vector3 eulerAngles/*, Vector3 scale*/)
        {
            this.position = new SerializableVector3(position);
            this.eulerAngles = new SerializableVector3(eulerAngles);
            // this.scale = new SerializableVector3(scale);
        }
    }
}