using Moein.Timeline;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    protected TimeState timeState = TimeState.Forward;
    public StoreType storeType = StoreType.NoMemory;
    public float timeScale = 1;

    private TimelineComponent<Transform, TransformSnapshot> transformTimeline;

    private void Start()
    {
        transformTimeline = new TimelineComponent<Transform, TransformSnapshot>(transform);
    }

    protected void Rewind()
    {
    }

    protected void Record()
    {
    }

    public void Pause()
    {
    }

    public void StartRewind()
    {
    }

    public void StopRewind()
    {
    }

    public void ChangeTimeState(TimeState newTimeState)
    {
        timeState = newTimeState;
    }
}