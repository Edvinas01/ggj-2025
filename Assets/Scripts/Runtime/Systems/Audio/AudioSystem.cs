using System;
using CHARK.GameManagement;
using CHARK.GameManagement.Systems;
using CHARK.ScriptableAudio;
using FMODUnity;
using UABPetelnia.GGJ2025.Runtime.Settings;
using UABPetelnia.GGJ2025.Runtime.Systems.Settings;
using UnityEngine;

namespace UABPetelnia.GGJ2025.Runtime.Systems.Audio
{
    internal sealed class AudioSystem : MonoSystem, IAudioSystem
    {
        [Header("Banks")]
        [SerializeField]
        private StudioBankLoader bankLoader;

        [Header("Global Parameters")]
        [SerializeField]
        private AudioParameter globalMasterVolumeParameter;

        [SerializeField]
        private AudioParameter globalMusicVolumeParameter;

        [SerializeField]
        private AudioParameter globalSfxVolumeParameter;

        private ISettingsSystem settingsSystem;

        public bool IsLoading
        {
            get
            {
                if (RuntimeManager.HaveAllBanksLoaded == false)
                {
                    return true;
                }

                if (RuntimeManager.AnySampleDataLoading())
                {
                    return true;
                }

                return false;
            }
        }

        public void LoadBanks()
        {
            bankLoader.Load();
        }

        public void UnLoadBanks()
        {
            bankLoader.Unload();
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            settingsSystem = GameManager.GetSystem<ISettingsSystem>();
        }

        private void Start()
        {
            InitializeGlobalVolumeParameters();
        }

        public float GetVolume(VolumeType type)
        {
            var settings = settingsSystem.Settings;
            var volume = type switch
            {
                VolumeType.Master => settings.MasterVolume,
                VolumeType.Music => settings.MusicVolume,
                VolumeType.SFX => settings.SfxVolume,
                _ => GeneralSettings.MaxVolume,
            };

            return GetNormalizedVolume(volume);
        }

        public void SetVolume(VolumeType type, float volume)
        {
            var clampedVolume = GetNormalizedVolume(volume);
            var settings = settingsSystem.Settings;

            switch (type)
            {
                case VolumeType.Master:
                    globalMasterVolumeParameter.SetParameterValue(clampedVolume);
                    settings.MasterVolume = clampedVolume;
                    break;
                case VolumeType.Music:
                    globalMusicVolumeParameter.SetParameterValue(clampedVolume);
                    settings.MusicVolume = clampedVolume;
                    break;
                case VolumeType.SFX:
                    globalSfxVolumeParameter.SetParameterValue(clampedVolume);
                    settings.SfxVolume = clampedVolume;
                    break;
                default:
                    Debug.LogWarning($"Unsupported volume type: {type}", this);
                    break;
            }

            settingsSystem.Settings = settings;
        }

        private void InitializeGlobalVolumeParameters()
        {
            globalMasterVolumeParameter.SetParameterValue(GetVolume(VolumeType.Master));
            globalMusicVolumeParameter.SetParameterValue(GetVolume(VolumeType.Music));
            globalSfxVolumeParameter.SetParameterValue(GetVolume(VolumeType.SFX));
        }

        private static float GetNormalizedVolume(float volume)
        {
            var clampedVolume = Mathf.Clamp(
                volume,
                GeneralSettings.MinVolume,
                GeneralSettings.MaxVolume
            );

            var roundVolume = (float)Math.Round(clampedVolume, 2);

            return roundVolume;
        }
    }
}
