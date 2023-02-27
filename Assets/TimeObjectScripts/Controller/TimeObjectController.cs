using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeObjectController : MonoBehaviour
{
	[SerializeField]
	protected TimeObjectManager timeObjectManager;

	public virtual void StartRewind ()
	{
		try {
			timeObjectManager.StartRewind ();
		} catch (System.Exception ex) {
		}
	}

	public virtual void StopRewind ()
	{
		try {
			timeObjectManager.StopRewind ();
		} catch (System.Exception ex) {
		}
	}

	public void Pause ()
	{
		timeObjectManager.ChangeTimeState (TimeState.Pause);
	}

	public void Resume ()
	{
		timeObjectManager.ChangeTimeState (TimeState.Forward);
	}


	protected void Action (TimeAction action)
	{
		switch (action) {
		case TimeAction.StartRewind:
			StartRewind ();
			break;
		case TimeAction.StopRewind:
			StopRewind ();
			break;
		case TimeAction.Pause:
			Pause ();
			break;
		case TimeAction.Resume:
			Resume ();
			break;
		case TimeAction.Nothing:
		default:
			break;
		}
	}

}