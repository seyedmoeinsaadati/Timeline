using System.Collections.Generic;

namespace Moein.TimeSystem
{
    public abstract class TimelineComponent<TComponent, TSnapshot>
    {
        protected TComponent component;
        protected List<TSnapshot> tape;

        public List<TSnapshot> Tape
        {
            get => tape;
            set => tape = value;
        }

        public TSnapshot HeadSnapshot => tape[CaptureCount - 1];

        public int CaptureCount => tape.Count;
        public abstract TSnapshot CurrentSnapshot { get; }

        protected TimelineComponent(TComponent component)
        {
            this.component = component;
            tape = new List<TSnapshot>();
        }

        public void SetTargetComponent(TComponent component) => this.component = component;

        public abstract void CaptureSnapshot();

        public abstract void ApplySnapshot(TSnapshot snapshot);

        public abstract TSnapshot LerpSnapshot(TSnapshot from, TSnapshot to, float t);
        public TSnapshot LerpSnapshot(int index1, int index2, float t) => LerpSnapshot(tape[index1], tape[index2], t);
    }
}