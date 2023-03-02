using Moein.TimeSystem;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    [SerializeField] public float captureInterval = .5f;
    [SerializeField] private StoreType storeType = StoreType.NoMemory;
    private Timeline[] children;

    private float lerpT;
    private float time;
    private int currentPointer;
    private int headIndex;

    private void Start()
    {
        currentPointer = headIndex = 0;
        InitComponents();
        children = GetComponentsInChildren<Timeline>();
    }

    public void Progress(float timeScale)
    {
        currentPointer = (int) (time / captureInterval);

        if (storeType == StoreType.NoMemory || currentPointer == headIndex)
        {
            Capture();
        }
        else
        {
            lerpT = timeScale == 0 ? lerpT : time - (currentPointer * captureInterval);
            transformTimeline.ApplySnapshot(transformTimeline.LerpSnapshot(currentPointer, currentPointer + 1, lerpT));
        }

        time += Time.fixedDeltaTime * timeScale;
        time = Mathf.Max(0.001f, time);
    }

    public void Rewind(float timeScale)
    {
        // calculate tape pointer based on recordingTime
        // calculate lerpT base on recordingTime
        currentPointer = (int) (time / captureInterval);

        if (currentPointer == headIndex)
        {
            transformTimeline.ApplySnapshot(transformTimeline.LerpSnapshot(currentPointer, currentPointer, lerpT));
            return;
        }

        lerpT = time - (currentPointer * captureInterval);
        transformTimeline.ApplySnapshot(transformTimeline.LerpSnapshot(currentPointer, currentPointer + 1, lerpT));


        time += Time.fixedDeltaTime * timeScale;
        time = Mathf.Max(0.001f, time);
    }


    #region Capturing

    private float capturingTimer;

    private void Capture()
    {
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

    #endregion


    #region TimelineComponents

    private TransformComponent transformTimeline;

    private void InitComponents()
    {
        transformTimeline = new TransformComponent(transform);
    }

    #endregion
}