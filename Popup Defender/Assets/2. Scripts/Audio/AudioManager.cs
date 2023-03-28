using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    public float MasterVolume = 1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Will stay from scene to scene :D
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume * MasterVolume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("MenuBGM");
    }

    public void Play(string name)
    {
        //Sound s = Array.Find(sounds, sound => sound.name == name);
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                s.source.Play();
                break;
            }
        }
    }

    public void Stop(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                s.source.Stop();
                break;
            }
        }
    }

    public void SetPitch(string name, float pitchValue)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                s.source.pitch = pitchValue;
                break;
            }
        }
    }

    public void Pause(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                s.source.Pause();
                break;
            }
        }
    }

    public void Unpause(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                s.source.UnPause();
                break;
            }
        }
    }
}
