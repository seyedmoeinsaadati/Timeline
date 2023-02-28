using UnityEngine;

namespace Moein.Timeline
{
    public abstract class RecordModel<TComponent, TSnapshot>
    {
        public string fileName;
        public TimelineComponent<TComponent, TSnapshot> timeline;

        public void Save(string directory)
        {
            TimeRecorderFileHandler.Save(directory, fileName, timeline.Tape);
        }

        public void Load(string directory)
        {
            timeline.Tape = TimeRecorderFileHandler.Load<TSnapshot>(directory, fileName);
        }
    }

    public class TransformRecordModel : RecordModel<Transform, TransformSnapshot>
    {
        // public override void CaptureData()
        // {
        //     var model = new TransformSnapshot(component.localPosition, component.localRotation);
        //     tape.Add(model);
        // }
        //
        // public override void SetData(TransformSnapshot dataModel)
        // {
        //     component.localPosition = dataModel.position;
        //     component.localRotation = dataModel.rotation;
        // }
    }

    public class AudioRecordModel : RecordModel<AudioSource, float>
    {
        // public override void CaptureData()
        // {
        //     tape.Add(component.pitch);
        // }
        //
        // public override void SetData(float dataModel)
        // {
        //     component.pitch = dataModel;
        // }
    }
}