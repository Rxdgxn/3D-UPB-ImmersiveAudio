using System;
using System.Collections.Generic;
using UnityEngine;
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
        }

        private void OnDisable()
        {
            fpsController.OnFootstepDetected -= HandleFootsteps;

            footstepSounds = null; // for GC

            dayNightCycleController.OnDayNightCycleValueChanged -= HandleCycleChange;

            CollectibleTracker.Instance.OnCollectibleGathered -= HandleCollection;
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
