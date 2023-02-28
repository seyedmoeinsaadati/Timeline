using System.Collections.Generic;

namespace Moein.Timeline
{
    public abstract class TimelineComponent<TComponent, TSnapshot>
    {
        protected TComponent component;
        protected int pointer, crntPointer;
        public int tapeIndex;
        protected List<TSnapshot> tape;

        public List<TSnapshot> Tape
        {
            get => tape;
            set => tape = value;
        }

        public TimelineComponent(TComponent component)
        {
            this.component = component;
            tape = new List<TSnapshot>();
        }

        public void SetTargetComponent(TComponent component) => this.component = component;

        public abstract void CaptureSnapshot();

        public abstract void ApplySnapshot(TSnapshot snapshot);

        public int SamplingCount => tape.Count;

    }
}