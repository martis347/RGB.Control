using System;
using NAudio.Wave;

namespace SoundProcessing
{
    public class SoundValue
    {
        private int avgSum = 0;
        private int avgPos = 0;
        private int[] avgVals = new int[10];

        private int PeakMaxNormalized = 0;
        private int PeakMinNormalized = 0;

        public int Analyze(WaveInEventArgs e, double sensitivity)
        {
            int max = GetMax(e, sensitivity);
            int averageTotal = GetAverageTotal(max);
            int peak = GetPeak(averageTotal);

            return peak;
        }

        private int GetMax(WaveInEventArgs e, double sensitivity)
        {
            int max = 0;
            for (int x = 0; x < e.BytesRecorded; x += 2)
            {
                if (BitConverter.ToInt16(e.Buffer, x) > max)
                {
                    max = (int) Math.Abs(BitConverter.ToInt16(e.Buffer, x) * sensitivity);
                }
            }

            return max;
        }

        private int GetAverageTotal(int max)
        {
            avgSum -= avgVals[avgPos];
            avgSum += max;
            avgVals[avgPos] = max;
            avgPos = (avgPos + 1) % avgVals.Length;
            int avgTotal = avgSum / avgVals.Length;

            return avgTotal;
        }

        private int GetPeak(int value)
        {
            if (PeakMinNormalized > value)
            {
                int newMin = PeakMinNormalized - (PeakMinNormalized / 100);
                if (newMin < 0)
                    newMin = 0;
                PeakMinNormalized = newMin;
            }
            else
            {
                PeakMinNormalized += 10;
            }

            if ((PeakMinNormalized + 5500) > PeakMaxNormalized)
            {
                PeakMaxNormalized = PeakMinNormalized + 5500;
                if (PeakMaxNormalized > 32767)
                    PeakMaxNormalized = 32767;
            }

            if (PeakMaxNormalized < value)
                PeakMaxNormalized = value;
            else
            {
                PeakMaxNormalized -= (PeakMaxNormalized - PeakMinNormalized > 10000 ? 15 : 4);
            }


            double normalizedDivider = (PeakMaxNormalized - PeakMinNormalized) / 1024.0;
            int diff = value - PeakMinNormalized;
            if (diff < 0)
                diff = 0;
            return (int)((diff / normalizedDivider) + 0.9);
        }
    }
}
