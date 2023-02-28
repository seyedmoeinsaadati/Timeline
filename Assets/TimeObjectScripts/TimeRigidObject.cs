using UnityEngine;
using System.Collections;

//[RequireComponent (typeof(Rigidbody))]
//public class TimeRigidObject : TimeTransformObject
//{
//	Rigidbody rb;
//	// Use this for initialization
//	new void Start ()
//	{
//		base.Start ();
//		rb = GetComponent<Rigidbody> ();
//	}

//	public override void StartRewind ()
//	{
//		base.StartRewind ();
//		rb.isKinematic = true;
//	}

//	public override void StopRewind ()
//	{
//		base.StopRewind ();
//		rb.isKinematic = false;
//	}

//	public override void Pause ()
//	{
//		base.Pause ();
//		rb.isKinematic = true;
//	}

//	public override void ChangeTimeState (TimeState newTimeState)
//	{
//		base.ChangeTimeState (newTimeState);
//		ChangeKinematic ();
//	}

//	void ChangeKinematic ()
//	{
//		switch (timeState) {
//		case TimeState.Forward:
//			rb.isKinematic = false;
//			break;
//		case TimeState.Pause:
//			rb.isKinematic = true;		
//			break;
//		default:
//			break;
//		}
//	}
//}