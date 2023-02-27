using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;


public class TimeActionObject : TimeObject
{
	int pointer, crntPointer;

	List<bool> boolTape;
	public float MaxTapeIndex;

	public Component[] trackingComponent;

	new void Awake ()
	{
		base.Awake ();
		boolTape = new List<bool> ();
	}

	public void Start ()
	{
		MaxTapeIndex = (Mathf.Round (recordTime / fixedDeltaTime));
	}

	void FixedUpdate ()
	{
		switch (timeState) {
		case TimeState.Forward:
			if (isRewinding) {
				// backtrack tape
				Rewind ();
			} else {
				// Recording present time
				Record ();
			}
			break;
		case TimeState.Pause:
			Pause ();
			break;
		default:
			break;
		}
	}

	protected override void Rewind ()
	{	
		if (boolTape.Count > 0 && crntPointer < MaxTapeIndex - 1) {
			bool IK = boolTape [crntPointer];
			// Set Fields Function
			(trackingComponent [0] as Rigidbody).isKinematic = IK;

			switch (storeType) {
			case StoreType.NoMemory:
				boolTape.RemoveAt (0);
				break;
			case StoreType.WithMemory:
				crntPointer++;
				break;
			default:
				// To BE or NOT To BE
				break;
			}
		} else {
			StopRewind ();
		}
	}

	public override void StartRewind ()
	{
		isRewinding = true;
	}

	public override void StopRewind ()
	{
		isRewinding = false;
	}

	protected override void Record ()
	{
		if (crntPointer != 0) {
			bool anyChange = boolTape [--crntPointer];
			(trackingComponent [0] as Rigidbody).isKinematic = anyChange;
			return;
		}

		if (TapeIsFull ()) {
			boolTape.RemoveAt (boolTape.Count - 1);
		}

		bool IK = (trackingComponent [0] as Rigidbody).isKinematic;
		boolTape.Insert (0, IK);

	}

	public override void Pause ()
	{
		bool boolInfo = boolTape [0];
		(trackingComponent [0] as Rigidbody).isKinematic = boolInfo;
	}

	public override void ChangeTimeState (TimeState newTimeState)
	{
		timeState = newTimeState;
	}

	public bool TapeIsFull ()
	{
		return boolTape.Count > MaxTapeIndex;
	}
}
