namespace TutorialCSharp.Tutorials.Helpers
{
    public static class ColorHelper
    {
        //! [Code snippet color to uint]

        public static uint ColorToUInt(System.Drawing.Color color)
        {
            var value = (uint)((color.A << 24) | (color.B << 16) |
                               (color.G << 8) | color.R);
            return value;
        }

        //! [Code snippet color to uint]

        //! [Code snippet uint to color]

        public static System.Drawing.Color UIntToColor(uint color)
        {
            byte a = (byte)(color >> 24);
            byte r = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte b = (byte)(color >> 0);

            return System.Drawing.Color.FromArgb(a, r, g, b);
        }

        //! [Code snippet uint to color]

    }
}
