using System;
using Esprit;

namespace TutorialCSharp.Tutorials.Helpers
{
    public static class Geometry
    {
        //! [Code snippet distance]

        public static double Distance(Point point1, Point point2 = null)
        {
            var x2 = point2?.X ?? 0;
            var y2 = point2?.Y ?? 0;
            var z2 = point2?.Z ?? 0;

            return Distance(point1.X, point1.Y, point1.Z, x2, y2, z2);
        }

        public static double Distance(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            var dz = z2 - z1;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        //! [Code snippet distance]

        public static double Distance(double x1, double y1, double x2, double y2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        //! [Code snippet toradians]

        public static double ToRadians(double deg)
        {
            return deg * Math.PI / 180;
        }

        //! [Code snippet toradians]

    }
}
