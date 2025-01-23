using CHARK.GameManagement.Systems;

namespace UABPetelnija.GGJ2025.Runtime.Systems.Settings
{
    internal interface ISettingsSystem : ISystem
    {
        /// <summary>
        /// Current settings.
        /// </summary>
        public SettingsData Settings { get; set; }
    }
}
