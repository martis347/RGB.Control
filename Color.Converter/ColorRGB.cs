namespace Color.Converter
{
    public struct ColorRGB
    {
        public byte R;
        public byte G;
        public byte B;

        private ColorRGB(System.Drawing.Color value)
        {
            this.R = value.R;
            this.G = value.G;
            this.B = value.B;
        }
        public static implicit operator System.Drawing.Color(ColorRGB rgb)
        {
            System.Drawing.Color c = System.Drawing.Color.FromArgb(rgb.R, rgb.G, rgb.B);
            return c;
        }
        public static explicit operator ColorRGB(System.Drawing.Color c)
        {
            return new ColorRGB(c);
        }
    }
}
