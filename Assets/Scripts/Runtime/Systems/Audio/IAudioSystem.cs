using CHARK.GameManagement.Systems;

namespace UABPetelnija.GGJ2025.Runtime.Systems.Audio
{
    internal interface IAudioSystem : ISystem
    {
        /// <returns>
        /// Audio volume for given <paramref name="type"/> in [0, 1] range.
        /// </returns>
        public float GetVolume(VolumeType type);

        /// <summary>
        /// Change audio volume of given <paramref name="type"/>. The provided
        /// <paramref name="volume"/> must be within [0, 1] range.
        /// </summary>
        public void SetVolume(VolumeType type, float volume);
    }
}
