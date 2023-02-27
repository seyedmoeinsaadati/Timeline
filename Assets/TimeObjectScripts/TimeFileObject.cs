using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeFileObject : TimeObject
{
    public string fileName, directoryName;

    public bool active = true;

    int index;
    List<TransformInfo> tape;

    TransformInfo offset;

    void Awake()
    {
        fileName = GetComponentInParent<RecordObjectNaming>().GetName(transform);
        directoryName = GetComponentInParent<RecordObjectNaming>().GetDiretory();
        LoadFile();
        index = 0;
    }

    void Start()
    {
        offset = new TransformInfo(transform.localPosition, transform.localRotation);
        UpdateTransform();
    }

    void FixedUpdate()
    {
        if (!active)
        {
            return;
        }
        switch (timeState)
        {
            case TimeState.Forward:
                Record();
                break;
            case TimeState.Backward:
                Rewind();
                break;
            case TimeState.Pause:
                Pause();
                break;
            default:
                break;
        }
        UpdateTransform();
    }

    protected override void Rewind()
    {
        index--;
        if (index < 0)
        {
            // end of tape
            index = 0;
        }
    }

    protected override void Record()
    {
        index++;
        if (index > tape.Count - 1)
        {
            // end of tape

            // loop effect
            index = tape.Count - 1;
        }
    }

    public override void StartRewind()
    {
        isRewinding = true;
        ChangeTimeState(TimeState.Backward);
    }

    public override void StopRewind()
    {
        isRewinding = false;
        ChangeTimeState(TimeState.Forward);
    }

    public override void ChangeTimeState(TimeState newTimeState)
    {
        timeState = newTimeState;
    }

    public override void Pause()
    {
    }

    void LoadFile()
    {
        //tape = FileHandler.Load<TransformInfo> (fileName);
        tape = UnityFileHandler.Instance.Load<TransformInfo>(directoryName, fileName);
    }

    void UpdateTransform()
    {
        TransformInfo ti = tape[index];
        transform.localPosition = ti.GetPosition() + offset.GetPosition();
        Quaternion newRotation = transform.localRotation;
        newRotation.eulerAngles = ti.GetEularAngles();// + offset.GetEularAngles ();
        transform.localRotation = newRotation;
    }

}