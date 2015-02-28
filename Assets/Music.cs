using UnityEngine;
using System.Collections;
using System;

public class Music : MonoBehaviour {

	float bar = 3/8;

	public int[] notes;
	//e.g. 8 for 1/8 note
	public int[] fraction;

	float frequency = 0;
	double gain = 0.5;
	
	private double increment;
	private double phase;
	private double sampling_frequency = 48000;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.P)) {
			StartCoroutine("playNotes");
		}
	}

	IEnumerator playNotes () {
		for (int i=0; i<notes.Length; i++) {
			setFrequency(notes[i]);
			yield return new WaitForSeconds(1/(float)fraction[i]);
		}
	}

	void setFrequency (int keyNumber) {
		frequency = 440 * Mathf.Pow (2, ((float)keyNumber - 49)/12);
	}

	void OnAudioFilterRead (float[] data, int channels) {
		increment = frequency * 2 * Math.PI / sampling_frequency;
		for (var i = 0; i < data.Length; i = i + channels)
		{
			phase = phase + increment;
			// this is where we copy audio data to make them “available” to Unity
			data[i] = (float)(gain*Math.Sin(phase));
			// if we have stereo, we copy the mono data to each channel
			if (channels == 2) data[i + 1] = data[i];
			if (phase > 2 * Math.PI) phase = 0;
		}
	}
}
