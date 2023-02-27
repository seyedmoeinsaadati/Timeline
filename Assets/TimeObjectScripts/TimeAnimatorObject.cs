using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
public class TimeAnimatorObject : TimeObject
{
	Animator animator;

	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<Animator> ();
		//animator.Play ("forward", -1, float.NegativeInfinity);
	}

	protected override void Rewind ()
	{
		throw new System.NotImplementedException ();
	}

	protected override void Record ()
	{
		throw new System.NotImplementedException ();
	}

	public override void StartRewind ()
	{
		isRewinding = true;
		animator.SetFloat ("direction", -1);
		//animator.Play ("forward");
	}

	public override void StopRewind ()
	{
		isRewinding = false;
		animator.SetFloat ("direction", 1);
		//animator.Play ("forward");
	}

	public override void Pause ()
	{
		animator.speed = 0;
	}

	void Resume ()
	{
		animator.speed = 1;
	}

	public override void ChangeTimeState (TimeState newTimeState)
	{
		timeState = newTimeState;
		switch (timeState) {
		case TimeState.Pause: 
			Pause ();
			break;
		case TimeState.Forward:
			Resume ();
			break;
		}
	}
}
