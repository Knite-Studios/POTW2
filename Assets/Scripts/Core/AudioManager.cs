using UnityEngine;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager>
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(0.1f, 3f)]
        public float pitch = 1f;
        public bool loop = false;

        [HideInInspector]
        public AudioSource source;
    }

    [SerializeField] private Sound[] sounds;
    private Dictionary<string, Sound> soundDictionary;

    protected override void Awake()
    {
        base.Awake();
        InitializeSounds();
    }

    private void InitializeSounds()
    {
        soundDictionary = new Dictionary<string, Sound>();
        foreach (Sound s in sounds)
        {
            if (soundDictionary.ContainsKey(s.name))
            {
                Debug.LogWarning($"Duplicate sound name detected: {s.name}. Skipping.");
                continue;
            }

            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            soundDictionary[s.name] = s;
        }
    }

    public void Play(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound s))
        {
            s.source.Play();
        }
        else
        {
            Debug.LogWarning($"Sound {name} not found in AudioManager.");
        }
    }

    public void Stop(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound s))
        {
            s.source.Stop();
        }
        else
        {
            Debug.LogWarning($"Sound {name} not found in AudioManager.");
        }
    }

    public bool IsPlaying(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound s))
        {
            return s.source.isPlaying;
        }
        Debug.LogWarning($"Sound {name} not found in AudioManager.");
        return false;
    }

    public AudioSource GetAudioSource(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound s))
        {
            return s.source;
        }
        Debug.LogWarning($"Sound {name} not found in AudioManager.");
        return null;
    }
}