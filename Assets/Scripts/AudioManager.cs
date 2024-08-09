using System;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public Sound[] sounds;

    protected override void Awake()
    {
        base.Awake();
        loadAudioSources();
    }

    private void loadAudioSources()
    {
        foreach (var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void play(string soundName)
    {
        var s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogError("Sound " + soundName + " not found");
            return;
        }

        s.source.Play();
    }

    public void stop(string soundName)
    {
        var s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogError("Sound " + soundName + " not found");
            return;
        }

        if (s.source.isPlaying)
        {
            s.source.Stop();
        }
    }

    public bool isPlaying(string soundName)
    {
        var s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.LogError("Sound " + soundName + " not found");
            return false;
        }

        return s.source.isPlaying;
    }
}