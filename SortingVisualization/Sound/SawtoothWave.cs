namespace SortingVisualization.Sound
{
    public class SawtoothWave : SoundWave
    {
        public SawtoothWave(float duration) : base(duration)
        {
        }

        /// <inheritdoc/>
        public override float CalculateWave(float ft)
        {
            return ft - MathF.Floor(ft);
        }
    }
}
