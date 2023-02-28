using System.Collections.Generic;
using UnityEngine;

public class TimelineController : MonoBehaviour
{
    [SerializeField] private float captureInterval = .5f;
    private float timeScale = 1;
    private List<Timeline> timelines;

    private int captureCount;
    private float timer;

    public int CaptureCount => captureCount;

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime * timeScale;

        if (timeScale > 0)
        {
            // forward and recording
            if (timer > captureInterval)
            {
                Capture();
                timer = 0;
            }
        }
        else if (timeScale == 0)
        {
            // pause
            // no record snapshots
        }
        else
        {
            // rewinding
            timelines[0].Backward(2f);
        }
    }

    private void Capture()
    {
        for (int i = 0; i < timelines.Count; i++)
        {
            timelines[i].Record();
        }
        captureCount++;
    }

    public void RegisterTimeline(Timeline timeline)
    {
        if (timelines.Contains(timeline) == false)
        {
            timelines.Add(timeline);
        }
    }

    public void RemoveTimeline(Timeline timeline)
    {
        if (timelines.Contains(timeline))
        {
            timelines.Remove(timeline);
        }
    }

}