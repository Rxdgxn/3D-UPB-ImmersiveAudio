#if FMOD_INSTALLED

using System;
using FMODUnity;
using NaughtyAttributes;
using UnityEngine;

namespace Workshop.Scaffolding.Nature.Scripts.Audio.Manager
{
    public class FMODAudioManager : AudioManager
    {
        [SerializeField, BoxGroup("FMOD Events")]
        private EventReference footstepEvent;

        [SerializeField, BoxGroup("FMOD Events")]
        private EventReference ambientEvent;
        
        [SerializeField, BoxGroup("FMOD Events")]
        private EventReference collectiblePickupEvent;
        
        [SerializeField, BoxGroup("FMOD Events")]
        private EventReference musicEvent;
        
        [SerializeField, BoxGroup("FMOD Events")]
        private EventReference underwaterSnapshot;

        [SerializeField, BoxGroup("FMOD Events")]
        private EventReference jumpEvent;
        
        [SerializeField, BoxGroup("FMOD VCAs")]
        private string vcaMaster   = "vca:/VCA_Master";
        
        [SerializeField, BoxGroup("FMOD VCAs")]
        private string vcaSFX      = "vca:/VCA_SFX";
        
        [SerializeField, BoxGroup("FMOD VCAs")]
        private string vcaAmbience = "vca:/VCA_Ambience";
        
        [SerializeField, BoxGroup("FMOD VCAs")]
        private string vcaMusic    = "vca:/VCA_Music";

        private string lastSurfaceTouched; // for proper jump audio

        private void OnEnable()
        {
            fpsController.OnFootstepDetected += HandleFootsteps;
            fpsController.OnJump += HandleJump;
        }

        private void OnDisable()
        {
            fpsController.OnFootstepDetected -= HandleFootsteps;
            fpsController.OnJump -= HandleJump;
        }

        private void HandleFootsteps(AudioUtils.AudioSurfaceType type, float speed)
        {
            var inst = RuntimeManager.CreateInstance(footstepEvent);

            string surfaceType = type.ToString();
            lastSurfaceTouched = surfaceType;

            inst.setParameterByNameWithLabel("MaterialType", surfaceType);
            inst.start();
            inst.release();
        }

        private void HandleJump()
        {
            var inst = RuntimeManager.CreateInstance(jumpEvent);

            inst.setParameterByNameWithLabel("MaterialType", lastSurfaceTouched);
            inst.start();
            inst.release();
        }
    }
}

#endif