using System.Windows;

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
        private ColorScheme CurrentScheme;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Height = 300;
            IntensitySlider.IsEnabled = false;
            SensitivitySlider.IsEnabled = false;
            ResetSensitivity.IsEnabled = false;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Height = 200;
            IntensitySlider.IsEnabled = true;
            SensitivitySlider.IsEnabled = true;
            ResetSensitivity.IsEnabled = true;
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

            CurrentScheme = ColorScheme.RGB;

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

            CurrentScheme = ColorScheme.HSL;

            Slider1.Maximum = 360;
            Slider1.Value = 360;

            Slider2.Maximum = 100;
            Slider2.Value = 100;
            Slider2.IsEnabled = true;

            Slider3.Maximum = 100;
            Slider3.Value = 100;
            Slider3.IsEnabled = true;

            Slider1_Percent.ContentStringFormat = "{0}";
            Slider2_Percent.ContentStringFormat = "{0}%";
            Slider3_Percent.ContentStringFormat = "{0}%";
        }

        private void RainbowButton_Checked(object sender, RoutedEventArgs e)
        {
            Label1.Content = "Speed";
            Label2.Content = "";
            Label3.Content = "";

            CurrentScheme = ColorScheme.Rainbow;

            Slider1.Maximum = 500;
            Slider1.Value = 100;

            Slider2.Maximum = 0;
            Slider2.Value = 0;
            Slider2.IsEnabled = false;

            Slider3.Maximum = 0;
            Slider3.Value = 0;
            Slider3.IsEnabled = false;

            Slider1_Percent.ContentStringFormat = "{0}%";
            Slider2_Percent.ContentStringFormat = "{0}";
            Slider3_Percent.ContentStringFormat = "{0}";
        }

        private void Intensity_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void Sensitivity_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
        }

        private void ResetSensitivity_OnClick(object sender, RoutedEventArgs e)
        {
            SensitivitySlider.Value = 1;
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void Random_OnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
