using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace BeatDetector
{
    public class BeatDetectorWrapper : MonoBehaviour
    {
        [DllImport("BeatDetector")]
        private static extern void Initialize(int sampleRate, int bufferSize);

        [DllImport("BeatDetector")]
        private static extern void ProcessSpectrum(float[] spectrum, int numBands, int numChannels, float[] avgSpectrum, out bool isBass, out bool isLow);

        [Header("Spawning Settings")]
        public float spawnRate = 1.5f;
        
        private float _spawnTimer;
        private AudioSource _audioSource;
        private bool _bassDetected, _lowDetected;
        private float[] _freqAvgSpectrum = new float[4];
        private float[] _freqSpectrum = new float[1024 * 2];

        private void Start()
        {
            // DUMB AUDIO MANAGER. WHO EVER MADE THIS KYS
            var audioClip = Array.Find(AudioManager.Instance.sounds, sound => sound.name == "GamePlayBackground").clip;
            
            // Now find the audio source with the audio clip in the audio manager object
            _audioSource = Array.Find(AudioManager.Instance.gameObject.GetComponents<AudioSource>(), 
                source => source.GetComponent<AudioSource>().clip == audioClip); 
            if (_audioSource == null)
            {
                Debug.Log("Audio source not found");
                return;
            }
            else
            {
                Debug.Log("Audio source found");
            }
            
            var sampleRate = AudioSettings.outputSampleRate;
            var bufferSize = 1024;
            Initialize(sampleRate, bufferSize);
        }

        private void Update()
        {
            var numChannels = _audioSource.clip.channels;
            
            // Get spectrum data from both channels
            var tempSampleLeft = new float[1024];
            var tempSampleRight = new float[1024];

            // Might need to try catch the left channel too later
            _audioSource.GetSpectrumData(tempSampleLeft, 0, FFTWindow.Rectangular);
            try
            {
                _audioSource.GetSpectrumData(tempSampleRight, 1, FFTWindow.Rectangular);
            }
            catch (ArgumentException e)
            {
                Debug.Log($"<color=yellow>Failed to get spectrum data from right channel: {e.Message}</color>");
                Array.Clear(tempSampleRight, 0, tempSampleRight.Length);
            }

            // Combine both channels into a single array
            for (var i = 0; i < 1024; i++)
            {
                _freqSpectrum[i] = tempSampleLeft[i];
                _freqSpectrum[i + 1024] = tempSampleRight[i];
            }

            // Process the combined spectrum data
            ProcessSpectrum(_freqSpectrum, 2, numChannels, _freqAvgSpectrum, out _bassDetected, out _lowDetected);

            HandleDetection();
        }

        private void HandleDetection()
        {
            if (_bassDetected && _spawnTimer <= 0.0f)
            {
                SpawnHype();
                _spawnTimer = spawnRate;
            }
            
            _spawnTimer -= Time.deltaTime;
        }
        
        private void SpawnHype()
        {
            if (_bassDetected)
            {
                HypeManager.Instance.SpawnHype();
                _bassDetected = false;
                Debug.Log("Bass detected!");
            }
            
            // TODO: IDK maybe spawn a different object for low detection?
            // if (_lowDetected)
            // {
            //     _lowDetected = false;
            //     Debug.Log("Low detected!");
            // }
        }
    }
}