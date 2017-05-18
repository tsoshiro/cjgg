using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {
	public AudioClip[] sounds;
	List<AudioSource> audioSources = new List<AudioSource>();

	public AudioSource bgmSource;
	public AudioClip [] bgm;
	AudioSource _audio;
	public bool isMute;

	//DEBUG
	public List<AudioClip []> sampleSounds = new List<AudioClip []>();

	public AudioClip [] SE_GOOD;
	public AudioClip [] SE_BAD;
	public AudioClip [] SE_UP;
	public AudioClip [] SE_NO;
	public AudioClip [] SE_BUTTON;

	public List<int> sampleSoundsDirector;

	void Start() {
		Init ();
	}

	public void Init() {
		_audio = GetComponent<AudioSource>();

		for (int i = 0; i < sounds.Length; i++) {
			AudioSource aSource = this.gameObject.AddComponent<AudioSource>();
			audioSources.Add (aSource);
		}	
	}

	public void play(int id) {
		if (isMute) {
			DebugLogger.Log ("Setting is MUTE.");
			return;
		}

		if (sounds [id] == null) {
			DebugLogger.Log ("No audiosource exists.");
			return;
		}

		audioSources[id].clip = sounds[id];
		audioSources[id].Play();
	}

	public void playBGM (int pIndex = 0) {
		if (bgm [pIndex] == null) {
			DebugLogger.Log ("No bgm exists.");
			return;
		}

		bgmSource.clip = bgm[pIndex];
		bgmSource.volume = Const.BGM_VOLUME;
		bgmSource.loop = true;
		bgmSource.Play ();
	}

	public void setMuteFlg (bool pFlg) {
		bgmSource.mute = pFlg;
	}

	#region SAMPLE
	void setSampleSounds() {
		sampleSounds.Add (SE_GOOD);
		sampleSounds.Add (SE_BAD);
		sampleSounds.Add (SE_UP);
		sampleSounds.Add (SE_NO);
		sampleSounds.Add (SE_BUTTON);
	}

	public void _play (int id) { // id = sampleSounds[i]
		if (isMute) {
			return;
		}
		_audio.clip = sampleSounds [id] [sampleSoundsDirector [id]];
		_audio.Play ();		
	}
	#endregion
}