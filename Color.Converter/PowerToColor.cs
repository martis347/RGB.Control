namespace Color.Converter
{
    public class PowerToColor
    {
        public byte[] Translate(int peak)
        {
            ColorRGB rgb = new ColorRGB();

            rgb = HSLToRGB.HSL2RGB((double)peak / 1000, 1, 0.5);

            return new[] { rgb.R, rgb.G, rgb.B };
        }
    }
}