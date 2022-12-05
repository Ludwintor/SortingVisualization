using NAudio.Wave;

namespace SortingVisualization.Sound
{
    public abstract class SoundWave : WaveProvider32
    {
        private int _sample;
        private int _targetSamples;
        private int _fadeInSamples;
        private int _fadeOutSamples;
        private int _fadeOutStart;
        private object _lock = new();

        public float Frequency { get; set; }
        public float Amplitude { get; set; }

        public SoundWave(float duration) : base(44100, 1)
        {
            SetDuration(duration);
            Frequency = 2000;
            Amplitude = 0.1f;
        }

        public void SetDuration(float duration)
        {
            _sample = 0;
            _targetSamples = (int)Math.Floor(WaveFormat.SampleRate * duration / 1000f);
            _fadeInSamples = (int)Math.Floor(WaveFormat.SampleRate * duration / 1000f / 1.2f);
            _fadeOutSamples = _fadeInSamples;
            _fadeOutStart = _targetSamples - _fadeOutSamples;
        }

        /// <summary>
        /// Wave function to sample audio from. Amplitude will be applied automatically
        /// </summary>
        /// <param name="ft">Wave time multiplied by frequency</param>
        /// <returns>Wave value</returns>
        public abstract float CalculateWave(float ft);

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            lock (_lock)
            {
                int samplesLeft = Math.Min(_targetSamples - _sample, sampleCount);
                if (samplesLeft <= 0)
                    return 0;
                int sampleRate = WaveFormat.SampleRate;
                for (int i = 0; i < samplesLeft; i++)
                {
                    int position = _sample + i;
                    buffer[i + offset] = Amplitude * CalculateWave(Frequency * position / sampleRate);
                }

                for (int i = 0; i < samplesLeft; i++)
                {
                    int position = _sample + i;
                    if (_fadeInSamples > 0 && position < _fadeInSamples)
                        buffer[i + offset] *= position / (float)_fadeInSamples;

                    if (_fadeOutSamples > 0)
                    {
                        if (position >= _fadeOutStart && position < _fadeOutStart + _fadeOutSamples)
                            buffer[i + offset] *= 1 - (position - _fadeOutStart) / (float)_fadeOutSamples;
                    }
                }

                _sample += samplesLeft;
                return samplesLeft;
            }
        }
    }
}
