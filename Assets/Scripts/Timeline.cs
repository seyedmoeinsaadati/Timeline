using Moein.Timeline;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    public float captureInterval = .5f;

    public StoreType storeType = StoreType.NoMemory;
    private int currentPointer;
    private int headIndex;

    private Timeline[] children;

    private float lerpT;
    private float recordingTime;

    public bool IsTapeOnHead => headIndex == currentPointer;

    private void Start()
    {
        currentPointer = headIndex = 0;
        InitComponents();
        children = GetComponentsInChildren<Timeline>();
    }

    public void Progress()
    {
        if (storeType == StoreType.NoMemory || IsTapeOnHead)
        {
            // Record();
        }
        else
        {
            // calculate tape pointer based on recordingTime
            // calculate lerpT base on recordingTime

            // skip recording until pointer is on head
            // Forward();

            // lerp from prevIndex, currentIndex
            if (lerpT >= 1)
            {
                currentPointer++;
            }

            if (currentPointer == headIndex)
            {
                // transformTimeline.LerpSnapshot(currentPointer, currentPointer, t);
            }
            else
            {
                transformTimeline.LerpSnapshot(currentPointer, currentPointer + 1, lerpT);
            }
        }

        recordingTime += Time.fixedTime; // * timeScale;
    }

    public void Rewind()
    {
        // calculate tape pointer based on recordingTime
        // calculate lerpT base on recordingTime

        if (lerpT >= 1)
        {
            currentPointer--;
        }

        if (currentPointer == 0)
        {
            transformTimeline.LerpSnapshot(currentPointer, currentPointer, lerpT);
        }
        else
        {
            transformTimeline.LerpSnapshot(currentPointer, currentPointer - 1, lerpT);
        }

        recordingTime -= Time.fixedTime; // * timeScale;
    }

    private float capturingTimer = 0;

    private void Record()
    {
        // recording
        capturingTimer += Time.fixedDeltaTime;
        if (capturingTimer > captureInterval)
        {
            CaptureSnapshots();
            capturingTimer = 0;
        }
    }

    private void CaptureSnapshots()
    {
        if (currentPointer != headIndex) return;
        transformTimeline.CaptureSnapshot();
        headIndex++;
    }

    #region TimelineComponents

    private TransformComponent transformTimeline;

    private void InitComponents()
    {
        transformTimeline = new TransformComponent(transform);
    }

    #endregion
}