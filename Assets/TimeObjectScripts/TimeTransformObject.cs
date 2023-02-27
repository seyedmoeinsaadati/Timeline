using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeTransformObject : TimeObject
{
    protected int pointer, crntPointer;
    List<TransformInfo> tape;
    public float MaxTapeIndex;

    TransformInfo transformInfo;

    new void Awake()
    {
        base.Awake();
        tape = new List<TransformInfo>();
    }

    public void Start()
    {
        MaxTapeIndex = (Mathf.Round(recordTime / fixedDeltaTime));
    }

    void FixedUpdate()
    {
        switch (timeState)
        {
            case TimeState.Forward:
                if (isRewinding)
                {
                    // backtrack tape
                    Rewind();
                }
                else
                {
                    // Recording present time
                    Record();
                }
                break;
            case TimeState.Pause:
                Pause();
                break;
            default:
                break;
        }
    }

    protected override void Rewind()
    {
        if (tape.Count > 0 && crntPointer < MaxTapeIndex - 1)
        {
            transformInfo = tape[crntPointer];
            UpdateTransform();

            switch (storeType)
            {
                case StoreType.NoMemory:
                    tape.RemoveAt(0);
                    break;
                case StoreType.WithMemory:
                    crntPointer++;
                    break;
                default:
                    // To BE or NOT To BE
                    break;
            }
        }
        else
        {
            StopRewind();
        }
    }

    void UpdateTransform()
    {
        Quaternion newRotation = Quaternion.identity;
        newRotation.eulerAngles = transformInfo.GetEularAngles();

        transform.localRotation = newRotation;
        transform.localPosition = transformInfo.GetPosition();
    }

    public override void StartRewind()
    {
        isRewinding = true;
    }

    public override void StopRewind()
    {
        isRewinding = false;
    }

    protected override void Record()
    {
        if (crntPointer != 0)
        {
            transformInfo = tape[--crntPointer];
            UpdateTransform();
            return;
        }

        if (TapeIsFull())
        {
            tape.RemoveAt(tape.Count - 1);
        }

        tape.Insert(0, new TransformInfo(transform.localPosition, transform.localRotation));
        //Debug.logger.Log ("D insert: ", crntPointer);
    }

    public override void Pause()
    {
        transformInfo = tape[0];
        UpdateTransform();
    }

    public override void ChangeTimeState(TimeState newTimeState)
    {
        timeState = newTimeState;
    }

    public bool TapeIsFull()
    {
        return tape.Count > MaxTapeIndex;
    }
}