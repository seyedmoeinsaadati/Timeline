using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Moein.TimeRecorder
{
    public abstract class RecordModel<T, DataModel>
    {
        protected T target;

        public string fileName;
        public int tapeIndex;
        List<DataModel> tape = new List<DataModel>();

        public abstract DataModel CaptureData();
        public void SetTarget(T target) => this.target = target;
    }

    public class TrnasformRecordModel : RecordModel<Transform, TransformModel>
    {
        public override TransformModel CaptureData()
        {
            return new TransformModel(target.localPosition, target.localEulerAngles);
        }
    }

    public class AudioRecordModel : RecordModel<AudioSource, float>
    {
        public override float CaptureData()
        {
            return target.pitch;
        }
    }

}