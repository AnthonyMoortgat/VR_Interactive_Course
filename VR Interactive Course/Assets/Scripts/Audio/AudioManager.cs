using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    // Singleton
    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        foreach(Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Start()
    {
        Play("Intro");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if(s == null)
        {
            Debug.LogWarning("S is null");
            return;
        }

        s.source.Play();

    }

    public bool isSoundPlaying()
    {
        bool isplayingOrNot = false;

        foreach (Sound sound in sounds)
        {
            // return sound.source.isPlaying();
            AudioSource audioSource = sound.source;
            isplayingOrNot = audioSource.isPlaying;
            if(isplayingOrNot == true)
            {
                return true;
            }
        }

        return false;
    }

    public void Stop()
    {
        bool isplayingOrNot = false;

        foreach (Sound sound in sounds)
        {
            // return sound.source.isPlaying();
            AudioSource audioSource = sound.source;
            isplayingOrNot = audioSource.isPlaying;
            if (isplayingOrNot == true && sound.name != "Intro")
            {
                audioSource.Stop();
            }
        }
    }

    public float getLenght(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("S is null");
            return 0;
        }

        return s.clip.length;
    }
}
