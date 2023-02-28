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
    }

    public class AudioRecordModel : RecordModel<AudioSource, float>
    {
    }
}