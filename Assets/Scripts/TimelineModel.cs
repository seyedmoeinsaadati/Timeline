using System.Collections.Generic;

public class TimelineComponent<TComponent, TSnapshot>
{
    protected int pointer, crntPointer;
    protected TComponent component;
    public int tapeIndex;
    protected List<TSnapshot> tape;

    public List<TSnapshot> Tape
    {
        get => tape;
        set => tape = value;
    }

    public virtual void CaptureData()
    {
    }

    public virtual void SetData(TSnapshot snapshot)
    {
    }

    public int SamplingCount => tape.Count;

    public TimelineComponent(TComponent component)
    {
        this.component = component;
        tape = new List<TSnapshot>();
    }
}