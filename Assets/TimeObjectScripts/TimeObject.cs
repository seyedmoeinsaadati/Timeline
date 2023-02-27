using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(TimeObjectManager))]
public abstract class TimeObject :MonoBehaviour
{
	public bool isRewinding = false;
	protected TimeState timeState = TimeState.Forward;
	public StoreType storeType = StoreType.NoMemory;

	public float timeScale = 1;
	public float recordTime = 5f;
	public float fixedDeltaTime;

	protected abstract void Rewind ();

	protected abstract void Record ();

	public abstract void StartRewind ();

	public abstract void StopRewind ();

	public abstract void Pause ();

	public abstract void ChangeTimeState (TimeState newTimeState);

	public void Awake ()
	{
		fixedDeltaTime = Time.fixedDeltaTime;
	}
}