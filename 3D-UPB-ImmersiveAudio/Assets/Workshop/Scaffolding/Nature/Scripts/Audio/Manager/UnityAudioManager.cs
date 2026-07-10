using System;
using UnityEngine;

namespace Workshop.Scaffolding.Nature.Scripts.Audio.Manager
{
    public class UnityAudioManager : AudioManager
    {
        public AudioSource footstepsSource;
        public AudioClip[] footstepSounds;

        private void OnEnable()
        {
            fpsController.OnFootstepDetected += HandleFootsteps;
        }

        private void OnDisable()
        {
            fpsController.OnFootstepDetected -= HandleFootsteps;
        }

        private void HandleFootsteps(AudioUtils.AudioSurfaceType type, float speed)
        {
            if (type == AudioUtils.AudioSurfaceType.None)
                return;

            Debug.Log(type);

            footstepsSource.PlayOneShot(footstepSounds[(int) type]);
        }
    }
}
