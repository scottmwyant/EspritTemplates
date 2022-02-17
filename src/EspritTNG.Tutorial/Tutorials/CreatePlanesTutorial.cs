using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class CreatePlanesTutorial : BaseTutorial
    {
        public CreatePlanesTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        private void CreateNewPlane()
        {
            var plane = PlaneHelper.AddUniquePlane(Document.Planes, "Test plane");
            plane.IsWork = true;

            plane.Ux = -0.70711;
            plane.Uy = 0.70711;
            plane.Uz = 0;

            plane.Vx = -0.40825;
            plane.Vy = -0.40825;
            plane.Vz = 0.8165;

            plane.Wx = 0.57735;
            plane.Wy = 0.57735;
            plane.Wz = 0.57735;

            plane.X = 0;
            plane.Y = 0;
            plane.Z = 0;

            plane.Activate();
        }

        //! [Code snippet]

        public override void Execute()
        {
            CreateNewPlane();
        }

        public override string Name => "Introduction to Creating Planes";
        public override string HtmlPath => "html/create_planes_tutorial.html";

    }
}
