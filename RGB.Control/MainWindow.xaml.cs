using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Color.Converter;
using NAudio.Wave;
using SoundProcessing;
using ArduinoControl;

namespace RGB.Control
{
    enum ColorScheme
    {
        RGB,
        HSL,
        Rainbow
    }

    public partial class MainWindow : Window
    {
        private ColorScheme _currentScheme;
        private static readonly WaveIn MicIn = new WaveIn();
        private SoundValue _soundValue;
        private static int _peak;
        private Arduino _arduino;
        private double _internsity;
        private double _sensitivity;
        private bool _customColor;
        private double _rainbowValue;
        private bool _blink = false;
        private int _colorArrangement;

        public MainWindow()
        {
            InitializeComponent();

            PlayLeds();
        }

        private void PlayLeds()
        {
            _arduino = new Arduino("COM7", 115200);
            _arduino.Connect();

            _soundValue = new SoundValue();
            StartRecording();
        }

        private void StartRecording()
        {
            MicIn.DeviceNumber = 0;
            MicIn.WaveFormat = new WaveFormat(44100, 16, 1);
            MicIn.BufferMilliseconds = 8;
            MicIn.DataAvailable += Record;
            MicIn.StartRecording();
        }

        private void Record(object sender, WaveInEventArgs e)
        {
            byte[] colors = {0, 0, 0};
            if (!_customColor)
            {
                _peak = _soundValue.Analyze(e, _sensitivity);

                double luminosity = _internsity/250;

                if (_blink)
                {
                    luminosity = 0.36*_peak/300;
                    if (luminosity > 1)
                    {
                        luminosity = 1;
                    }
                }
                var rgb = HSLToRGB.HSL2RGB((((double)_peak + 150 * _colorArrangement)%1024) / 1024, 1, luminosity);

                colors = new[] {rgb.R, rgb.G, rgb.B};

                PBar.Value = _peak;
            }
            else
            {
                switch (_currentScheme)
                {
                    case ColorScheme.HSL:
                        colors = HSLScheme();
                        break;
                    case ColorScheme.RGB:
                        colors = RGBScheme();
                        break;
                    case ColorScheme.Rainbow:
                        colors = RainbowSchema();
                        break;
                }
            }

            _arduino.Write(colors);
        }

        private byte[] RGBScheme()
        {
            return new [] {(byte)Slider1.Value, (byte)Slider2.Value, (byte)Slider3.Value};
        }

        private byte[] HSLScheme()
        {
            var colors = HSLToRGB.HSL2RGB(Slider1.Value/361, Slider2.Value/100, Slider3.Value/100);

            return new[] { colors.R, colors.G , colors.B };
        }

        private byte[] RainbowSchema()
        {
            while (true)
            {
                var colors = HSLToRGB.HSL2RGB((_rainbowValue % 100) / 100, Slider2.Value/100, Slider3.Value / 100);
                _rainbowValue += 0.01 * Slider1.Value / 20;

                return new[] { colors.R, colors.G, colors.B };
            }
        }


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Height = 300;
            IntensitySlider.IsEnabled = false;
            SensitivitySlider.IsEnabled = false;
            ResetSensitivity.IsEnabled = false;

            _customColor = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Height = 200;
            IntensitySlider.IsEnabled = true;
            SensitivitySlider.IsEnabled = true;
            ResetSensitivity.IsEnabled = true;

            _customColor = false;
        }

        private void Slider1_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider1_Percent.Content = e.NewValue;
        }

        private void Slider2_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider2_Percent.Content = e.NewValue;
        }

        private void Slider3_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider3_Percent.Content = e.NewValue;
        }

        private void RGBButton_Checked(object sender, RoutedEventArgs e)
        {
            Label1.Content = "Red";
            Label2.Content = "Green";
            Label3.Content = "Blue";

            _currentScheme = ColorScheme.RGB;

            Slider1.Maximum = 255;
            Slider1.Value = 255;

            Slider2.Maximum = 255;
            Slider2.Value = 255;
            Slider2.IsEnabled = true;

            Slider3.Maximum = 255;
            Slider3.Value = 255;
            Slider3.IsEnabled = true;

            Slider1_Percent.ContentStringFormat = "{0}";
            Slider2_Percent.ContentStringFormat = "{0}";
            Slider3_Percent.ContentStringFormat = "{0}";
        }

        private void HSLButton_Checked(object sender, RoutedEventArgs e)
        {
            Label1.Content = "Hue";
            Label2.Content = "Saturation";
            Label3.Content = "Luminance";

            _currentScheme = ColorScheme.HSL;

            Slider1.Maximum = 360;
            Slider1.Value = 360;

            Slider2.Maximum = 100;
            Slider2.Value = 100;
            Slider2.IsEnabled = true;

            Slider3.Maximum = 100;
            Slider3.Value = 36;
            Slider3.IsEnabled = true;

            Slider1_Percent.ContentStringFormat = "{0}";
            Slider2_Percent.ContentStringFormat = "{0}%";
            Slider3_Percent.ContentStringFormat = "{0}%";
        }

        private void RainbowButton_Checked(object sender, RoutedEventArgs e)
        {
            Label1.Content = "Speed";
            Label2.Content = "Saturation";
            Label3.Content = "Luminance";

            _currentScheme = ColorScheme.Rainbow;

            Slider1.Maximum = 500;
            Slider1.Value = 100;

            Slider2.Maximum = 100;
            Slider2.Value = 100;
            Slider2.IsEnabled = true;

            Slider3.Maximum = 100;
            Slider3.Value = 50;
            Slider3.IsEnabled = true;

            Slider1_Percent.ContentStringFormat = "{0}%";
            Slider2_Percent.ContentStringFormat = "{0}";
            Slider3_Percent.ContentStringFormat = "{0}";
        }

        private void Intensity_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _internsity = e.NewValue;
        }

        private void Sensitivity_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _sensitivity = e.NewValue;
        }

        private void ResetSensitivity_OnClick(object sender, RoutedEventArgs e)
        {
            SensitivitySlider.Value = 1;
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }


        private void Blink_Checked(object sender, RoutedEventArgs e)
        {
            _blink = true;
        }

        private void Blink_Unchecked(object sender, RoutedEventArgs e)
        {
            _blink = false;
        }

        private void NextColor_OnClick(object sender, RoutedEventArgs e)
        {
            _colorArrangement = (_colorArrangement + 1) % 6;
        }

        private void PreviousColor_OnClick(object sender, RoutedEventArgs e)
        {
            if (_colorArrangement == 1)
            {
                _colorArrangement = 6;
            }
            else
            {
                _colorArrangement -= 1;
            }

        }
    }
}
