using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Workshop.Scaffolding.Nature.Scripts.Collectible;

namespace Workshop.Scaffolding.Nature.Scripts.Audio.Manager
{
    public class UnityAudioManager : AudioManager
    {
        public AudioSource footstepsSource;
        public AudioClip[] dirtSounds, stoneSounds, woodSounds;
        public List<AudioClip[]> footstepSounds;

        public AudioSource daySource;
        public AudioSource nightSource;
        public AudioClip dayAmbience;
        public AudioClip nightAmbience;

        public AudioSource collectibleSource;

        public AudioSource musicSource;
        public AudioClip musicClip;

        public AudioMixer audioMixer;
        private string[] mixerOptions = {"Master", "SFX", "Ambience", "Music"};

        public void Start()
        {
            daySource.volume = 1;
            nightSource.volume = 0;

            daySource.loop = true;
            nightSource.loop = true;

            daySource.clip = dayAmbience;
            nightSource.clip = nightAmbience;

            daySource.Play();
            nightSource.Play();

            musicSource.clip = musicClip;
            musicSource.loop = true;
            musicSource.volume = 1;

            musicSource.Play();
        }

        private void OnEnable()
        {
            fpsController.OnFootstepDetected += HandleFootsteps;

            footstepSounds = new List<AudioClip[]>
            {
                dirtSounds,
                stoneSounds,
                woodSounds
            };

            dayNightCycleController.OnDayNightCycleValueChanged += HandleCycleChange;

            CollectibleTracker.Instance.OnCollectibleGathered += HandleCollection;

            audioOptionsUIController.OnAudioOptionChanged += HandleOptionChanged;
        }

        private void OnDisable()
        {
            fpsController.OnFootstepDetected -= HandleFootsteps;

            footstepSounds = null; // for GC

            dayNightCycleController.OnDayNightCycleValueChanged -= HandleCycleChange;

            CollectibleTracker.Instance.OnCollectibleGathered -= HandleCollection;

            audioOptionsUIController.OnAudioOptionChanged -= HandleOptionChanged;
        }

        private void HandleOptionChanged(AudioUtils.AudioOptionType type, float value)
        {
            float dbValue = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
            audioMixer.SetFloat(mixerOptions[(int) type], dbValue);
        }

        private void HandleCollection(CollectibleData data)
        {
            collectibleSource.transform.position = data.Position;
            collectibleSource.PlayOneShot(data.Clip);
        }

        private void HandleCycleChange(float slider)
        {
            daySource.volume = 1 - slider;
            nightSource.volume = slider;
        }

        private void HandleFootsteps(AudioUtils.AudioSurfaceType type, float speed)
        {
            if (type == AudioUtils.AudioSurfaceType.None)
                return;

            var sounds = footstepSounds[(int) type];
            var soundIdx = UnityEngine.Random.Range(0, sounds.Length);

            // Debug.Log("Playing " + type + " sound, numero " + soundIdx);

            footstepsSource.PlayOneShot(sounds[soundIdx]);
        }
    }
}
