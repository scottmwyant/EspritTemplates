namespace TutorialCSharp.Tutorials
{
    class ScanPlanesTutorial : BaseTutorial
    {
        public ScanPlanesTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void ScanPlanes()
        {
            var outputWindow = EspritApplication.EventWindow;

            outputWindow.Clear();
            outputWindow.Visible = true;

            foreach (Esprit.Plane plane in Document.Planes)
            {
                if (plane == null)
                {
                    continue;
                }

                if (plane.IsWork)
                {
                    outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanPlanesTutorial", $"Plane {plane.Name} is a WorkPlane");
                }

                outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanPlanesTutorial", $"Plane {plane.Name}: X = {plane.X:0.0####}, Y = {plane.Y:0.0####}, Z = {plane.Z:0.0####}");
                outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanPlanesTutorial", $"Plane {plane.Name}: Ux = {plane.Ux:0.0####}, Uy = {plane.Uy:0.0####}, Uz = {plane.Uz:0.0####}");
                outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanPlanesTutorial", $"Plane {plane.Name}: Vx = {plane.Vx:0.0####}, Vy = {plane.Vy:0.0####}, Vz = {plane.Vz:0.0####}");
                outputWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanPlanesTutorial", $"Plane {plane.Name}: Wx = {plane.Wx:0.0####}, Wy = {plane.Wy:0.0####}, Wz = {plane.Wz:0.0####}");
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            ScanPlanes();
        }

        public override string Name => "Scan Planes";
        public override string HtmlPath => "html/scan_planes_tutorial.html";

    }
}
