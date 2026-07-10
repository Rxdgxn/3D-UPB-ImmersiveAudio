using System;
using System.Collections.Generic;
using UnityEngine;

namespace Workshop.Scaffolding.Nature.Scripts.Audio.Manager
{
    public class UnityAudioManager : AudioManager
    {
        public AudioSource footstepsSource;
        public AudioClip[] dirtSounds, stoneSounds, woodSounds;
        public static List<AudioClip[]> footstepSounds;
        private void OnEnable()
        {
            fpsController.OnFootstepDetected += HandleFootsteps;

            footstepSounds = new List<AudioClip[]>
            {
                dirtSounds,
                stoneSounds,
                woodSounds
            };
        }

        private void OnDisable()
        {
            fpsController.OnFootstepDetected -= HandleFootsteps;

            footstepSounds = null; // for GC
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
