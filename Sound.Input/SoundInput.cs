using NAudio.Wave;

namespace Sound.Input
{
    public static class SoundInput
    {
        private static readonly WaveIn MicIn = new WaveIn();

        public static WaveInEventArgs E;

        public static void Start()
        {
            MicIn.DeviceNumber = 0;
            MicIn.WaveFormat = new WaveFormat(44100, 16, 1);
            MicIn.BufferMilliseconds = 8;
            MicIn.DataAvailable += Record;
            MicIn.StartRecording();
        }

        private static void Record(object sender, WaveInEventArgs e)
        {
            E = e;
        }
    }
}
