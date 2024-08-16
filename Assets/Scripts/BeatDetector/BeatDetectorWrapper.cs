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
            InitializeAudioSource();
            InitializeBeatDetector();
        }

        private void InitializeAudioSource()
        {
            _audioSource = AudioManager.Instance.GetAudioSource("GamePlayBackground");
            if (_audioSource == null)
            {
                Debug.LogError("GamePlayBackground audio source not found in AudioManager");
                enabled = false;
                return;
            }
            Debug.Log("GamePlayBackground audio source found");
        }

        private void InitializeBeatDetector()
        {
            var sampleRate = AudioSettings.outputSampleRate;
            var bufferSize = 1024;
            Initialize(sampleRate, bufferSize);
        }

        private void Update()
        {
            if (_audioSource == null || _audioSource.clip == null) return;

            var numChannels = _audioSource.clip.channels;
            ProcessAudioData(numChannels);
            HandleDetection();
        }

        private void ProcessAudioData(int numChannels)
        {
            var tempSampleLeft = new float[1024];
            var tempSampleRight = new float[1024];

            _audioSource.GetSpectrumData(tempSampleLeft, 0, FFTWindow.Rectangular);
            
            if (numChannels > 1)
            {
                try
                {
                    _audioSource.GetSpectrumData(tempSampleRight, 1, FFTWindow.Rectangular);
                }
                catch (ArgumentException e)
                {
                    Debug.LogWarning($"Failed to get spectrum data from right channel: {e.Message}");
                    Array.Clear(tempSampleRight, 0, tempSampleRight.Length);
                }
            }
            else
            {
                Array.Copy(tempSampleLeft, tempSampleRight, tempSampleLeft.Length);
            }

            for (var i = 0; i < 1024; i++)
            {
                _freqSpectrum[i] = tempSampleLeft[i];
                _freqSpectrum[i + 1024] = tempSampleRight[i];
            }

            ProcessSpectrum(_freqSpectrum, 2, numChannels, _freqAvgSpectrum, out _bassDetected, out _lowDetected);
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
        }
    }
}