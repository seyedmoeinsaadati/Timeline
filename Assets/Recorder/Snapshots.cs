using UnityEngine;

namespace Moein.Timeline
{
    [System.Serializable]
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
    }
}