namespace TutorialCSharp.Tutorials.Helpers
{
    public static class PlaneHelper
    {
        //! [Code snippet unique plane]

        public static Esprit.Plane AddUniquePlane(Esprit.IPlanes planes, string baseName)
        {
            var suffix = 0;
            foreach (Esprit.Plane plane in planes)
            {
                var planeName = plane.Name;
                if (planeName.Length > baseName.Length && planeName.StartsWith(baseName) && planeName[baseName.Length] == '_')
                {
                    if (int.TryParse(planeName.Substring(baseName.Length + 1), out var index))
                    {
                        if (index >= suffix)
                        {
                            suffix = index + 1;
                        }
                    }
                }
            }

            var name = $"{baseName}_{suffix}";
            return planes.Add(name);
        }

        //! [Code snippet unique plane]

        //! [Code snippet copy to points]

        public static Esprit.Plane CopyWorkPlaneToPoint(Esprit.IPlanes planes, Esprit.IPlane planeToCopy, Esprit.Point point)
        {
            var plane = (planeToCopy.Name == string.Empty)
                ? AddUniquePlane(planes, "UVW Copy")
                : AddUniquePlane(planes, $"{planeToCopy.Name} Copy");

            plane.IsWork = true;
            plane.Ux = planeToCopy.Ux;
            plane.Uy = planeToCopy.Uy;
            plane.Uz = planeToCopy.Uz;
            plane.Vx = planeToCopy.Vx;
            plane.Vy = planeToCopy.Vy;
            plane.Vz = planeToCopy.Vz;
            plane.Wx = planeToCopy.Wx;
            plane.Wy = planeToCopy.Wy;
            plane.Wz = planeToCopy.Wz;
            plane.X = point.X;
            plane.Y = point.Y;
            plane.Z = point.Z;

            return plane;
        }

        //! [Code snippet copy to points]

        //! [Code snippet plane vectors]

        public static void SetPlaneVectors(ref Esprit.Plane plane,
            double x, double y, double z,
            double Ux, double Uy, double Uz,
            double Vx, double Vy, double Vz,
            double Wx, double Wy, double Wz)
        {
            plane.X = x;
            plane.Y = y;
            plane.Z = z;
            plane.Ux = Ux;
            plane.Uy = Uy;
            plane.Uz = Uz;
            plane.Vx = Vx;
            plane.Vy = Vy;
            plane.Vz = Vz;
            plane.Wx = Wx;
            plane.Wy = Wy;
            plane.Wz = Wz;
        }

        //! [Code snippet plane vectors]

        //! [Code snippet getplane]

        public static Esprit.Plane GetPlane(Esprit.Document document, string name)
        {
            Esprit.Plane plane = null;

            foreach (Esprit.Plane p in document.Planes)
            {
                if (p.Name == name)
                {
                    plane = p;
                    break;
                }
            }

            if (plane == null)
            {
                plane = document.Planes.Add(name);
            }

            return plane;
        }

        //! [Code snippet getplane]

    }
}
