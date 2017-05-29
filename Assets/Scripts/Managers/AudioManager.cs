using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static AudioManager _shared = null;
    public static AudioManager Shared {
        get {
            if (_shared == null) {
                Loader loader = FindObjectOfType<Loader>();
                _shared = loader.LoadManager<AudioManager>();
            }
            return _shared;
        }
    }

    public bool SFXEnabled = true;
    public bool musicEnabled = true;

    public AudioSource efxSource;
    public AudioSource musicSource;

    public float lowPitchRange = 0.95f;
    public float highPitchRange = 1.05f;

    // Use this for initialization
    void Awake() {
        if (_shared == null) {
            _shared = this;
        }
        else if (_shared != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingle(AudioClip clip) {
        if (SFXEnabled == false) return;

        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        efxSource.pitch = randomPitch;
        efxSource.clip = clip;
        efxSource.Play();
    }

    public void RandomizeSfx(params AudioClip[] clips) {
        if (SFXEnabled == false) return;

        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }

    public void PlayMusic(AudioClip musicClip) {
        if (musicEnabled == false) return;

        musicSource.clip = musicClip;
        musicSource.Play();
    }
}
