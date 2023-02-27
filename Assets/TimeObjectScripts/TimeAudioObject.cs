using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(AudioSource))]
public class TimeAudioObject : TimeObject
{
	int currentSample, totalSample;
	public AudioSource audioSource;
	public bool guiDebug;

	new void Awake ()
	{
		base.Awake ();
		audioSource = GetComponent<AudioSource> ();
		totalSample = audioSource.clip.samples;
	}

	// Update is called once per frame
	void Update ()
	{
		currentSample = audioSource.timeSamples;
	}

	protected override void Record ()
	{
		throw new System.NotImplementedException ();
	}

	protected override void Rewind ()
	{
		throw new System.NotImplementedException ();
	}

	public override void StartRewind ()
	{
		if (!audioSource.isPlaying) {
			currentSample = audioSource.clip.samples - 1;
		}

		isRewinding = true;
		audioSource.pitch = -1;
		audioSource.timeSamples = currentSample;
		audioSource.Play ();
	}

	public override void StopRewind ()
	{
		if (!audioSource.isPlaying) {
			currentSample = 0;
		}

		isRewinding = false;
		audioSource.pitch = 1;
		audioSource.timeSamples = currentSample;
		audioSource.Play ();
	}

	public override void Pause ()
	{
		audioSource.Pause ();
	}

	public override void ChangeTimeState (TimeState newTimeState)
	{
		timeState = newTimeState;

	}


	void Play ()
	{
		Debug.Log ("Samples :" + audioSource.clip.samples);
		audioSource.Play ();
	}

	void Stop ()
	{
		audioSource.Stop ();
	}

	void OnGUI ()
	{
		if (guiDebug) {
			GUI.HorizontalSlider (new Rect (25, 75, 100, 30), currentSample, 0, totalSample);	
		}
	}
}