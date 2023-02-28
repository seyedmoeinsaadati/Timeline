using System.Collections.Generic;
using UnityEngine;

namespace Moein.Timeline
{
    public abstract class RecordModel<Component, DataModel>
    {
        protected Component component;
        public string fileName;
        public int tapeIndex;
        protected List<DataModel> tape = new List<DataModel>();

        public void SetTargetComponent(Component component) => this.component = component;
        public abstract void CaptureData();
        public abstract void SetData(DataModel dataModel);

        public int SamplingCount => tape.Count;

        public void Save(string directory)
        {
            TimeRecorderFileHandler.Save(directory, fileName, tape);
        }

        public void Load(string directory)
        {
            tape = TimeRecorderFileHandler.Load<DataModel>(directory, fileName);
        }
    }

    public class TransformRecordModel : RecordModel<Transform, TransformSnapshot>
    {
        public override void CaptureData()
        {
            var model = new TransformSnapshot(component.localPosition, component.localRotation);
            tape.Add(model);
        }

        public override void SetData(TransformSnapshot dataModel)
        {
            component.localPosition = dataModel.position;
            component.localRotation = dataModel.rotation;
        }
    }

    public class AudioRecordModel : RecordModel<AudioSource, float>
    {
        public override void CaptureData()
        {
            tape.Add(component.pitch);
        }

        public override void SetData(float dataModel)
        {
            component.pitch = dataModel;
        }
    }
}