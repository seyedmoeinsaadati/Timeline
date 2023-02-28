using Moein.Timeline;
using System;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    public StoreType storeType = StoreType.NoMemory;

    // timeline components
    private TransformComponent transformTimeline;

    private int pointer, currentPointer;
    private int headIndex;

    private Timeline[] children;

    private void Start()
    {
        pointer = currentPointer = headIndex = 0;
        InitComponents();
        children = GetComponentsInChildren<Timeline>();
    }

    private void InitComponents()
    {
        transformTimeline = new TransformComponent(transform);
    }

    public void Forward(float t)
    {
        // lerping from prevIndex, currentIndex
        if (t >= 1)
        {
            currentPointer++;
        }

        if (currentPointer == headIndex)
        {
            // transformTimeline.LerpSnapshot(currentPointer, currentPointer, t);
        }
        else
        {
            transformTimeline.LerpSnapshot(currentPointer, currentPointer + 1, t);
        }
    }

    public void Backward(float t)
    {
        // lerping from currentIndex, 
        if (t >= 1)
        {
            currentPointer--;
        }

        if (currentPointer == 0)
        {
            transformTimeline.LerpSnapshot(currentPointer, currentPointer, t);
        }
        else
        {
            transformTimeline.LerpSnapshot(currentPointer, currentPointer - 1, t);
        }
    }

    public void Record()
    {
        if (currentPointer != headIndex) return;
        transformTimeline.CaptureSnapshot();
        headIndex++;
    }
}