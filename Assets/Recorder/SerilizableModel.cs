using UnityEngine;

namespace Moein.Timeline
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
    public class SerializableQuaternion
    {
        private float x, y, z, w;

        public SerializableQuaternion(Quaternion quaternion)
        {
            x = quaternion.x;
            y = quaternion.y;
            z = quaternion.z;
            w = quaternion.w;
        }

        public static implicit operator Quaternion(SerializableQuaternion value)
        {
            return new Quaternion(value.x, value.y, value.z, value.w);
        }
    }
    
}