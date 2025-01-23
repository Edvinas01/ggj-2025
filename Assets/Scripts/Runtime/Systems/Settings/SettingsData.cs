using Newtonsoft.Json;

namespace UABPetelnija.GGJ2025.Runtime.Systems.Settings
{
    internal struct SettingsData
    {
        public float LookSensitivity { get; set; }

        public float MasterVolume { get; set; }

        public float MusicVolume { get; set; }

        public float SfxVolume { get; set; }

        [JsonConstructor]
        public SettingsData(
            float lookSensitivity,
            float masterVolume,
            float musicVolume,
            float sfxVolume
        )
        {
            LookSensitivity = lookSensitivity;
            MasterVolume = masterVolume;
            MusicVolume = musicVolume;
            SfxVolume = sfxVolume;
        }
    }
}
