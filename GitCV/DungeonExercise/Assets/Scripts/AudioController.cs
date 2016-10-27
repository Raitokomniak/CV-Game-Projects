using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

	AudioSource uiSounds;
	AudioSource audio2;
	AudioSource[] asources;

	public bool isLoading;

	// Use this for initialization
	void Awake () {
		asources = GetComponents<AudioSource>();
		uiSounds = asources[0];
		audio2 = asources[1];
		isLoading = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayUISound(string clip)
	{
		uiSounds.clip = Resources.Load(clip) as AudioClip;
		uiSounds.Play();
	}

	public void PlaySoundAudio2(string clip)
	{
		audio2.clip = Resources.Load(clip) as AudioClip;
		audio2.Play();
	}
}
