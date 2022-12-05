using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace SortingVisualization.Sound
{
    public sealed class Beeper : IDisposable
    {
        private readonly WaveOutEvent _waveOut;
        private readonly MixingSampleProvider _provider;
        private readonly Func<float, SoundWave> _factory;
        private readonly Queue<SoundWave> _wavesPool;

        public Beeper(Func<float, SoundWave> factory)
        {
            _waveOut = new();
            _waveOut.DesiredLatency = 200;
            _waveOut.NumberOfBuffers = 3;
            _provider = new(WaveFormat.CreateIeeeFloatWaveFormat(44100, 1));
            _factory = factory;
            _wavesPool = new();
            _provider.MixerInputEnded += Provider_MixerInputEnded;
            _waveOut.Init(_provider);
        }

        public void Beep(float frequency, float amplitude, float duration)
        {
            SoundWave soundWave = GetSoundWave(duration);
            soundWave.Frequency = frequency;
            soundWave.Amplitude = amplitude;
            _provider.AddMixerInput((ISampleProvider)soundWave);
            if (_waveOut.PlaybackState != PlaybackState.Playing)
                _waveOut.Play();
        }

        public void Stop()
        {
            _waveOut.Stop();
            foreach (ISampleProvider provider in _provider.MixerInputs)
                _wavesPool.Enqueue((SoundWave)provider);
            _provider.RemoveAllMixerInputs();
        }

        public void Dispose()
        {
            _waveOut.Dispose();
        }

        private SoundWave GetSoundWave(float duration)
        {
            if (_wavesPool.Count > 0)
            {
                SoundWave wave = _wavesPool.Dequeue();
                wave.SetDuration(duration);
                return wave;
            }
            return _factory(duration);
        }

        private void Provider_MixerInputEnded(object? sender, SampleProviderEventArgs e)
        {
            _provider.RemoveMixerInput(e.SampleProvider);
            _wavesPool.Enqueue((SoundWave)e.SampleProvider);
        }
    }
}
