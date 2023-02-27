using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ITimeAction
{
	void OnStartRewind ();

	void OnStopRewind ();

	void OnFinishTape ();

	void OnStartTape ();

	void OnPause ();

	void OnChangeTimeState ();
	
}