namespace SortingVisualization.Sound
{
    public sealed class SquareWave : SoundWave
    {
        public SquareWave(float duration) : base(duration)
        {
        }

        /// <inheritdoc/>
        public override float CalculateWave(float ft)
        {
            return 2 * (2 * MathF.Floor(ft) - MathF.Floor(2 * ft)) + 1;
        }
    }
}
