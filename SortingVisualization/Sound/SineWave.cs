namespace SortingVisualization.Sound
{
    public sealed class SineWave : SoundWave
    {
        public SineWave(float duration) : base(duration)
        {
        }

        /// <inheritdoc/>
        public override float CalculateWave(float ft)
        {
            return MathF.Sin(MathF.Tau * ft);
        }
    }
}
