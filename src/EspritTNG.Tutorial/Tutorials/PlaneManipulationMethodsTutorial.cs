namespace TutorialCSharp.Tutorials
{
    class PlaneManipulationMethodsTutorial : BaseTutorial
    {
        public PlaneManipulationMethodsTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet set]

        private void TranslateWorkPlaneToPointCoordinatesDemo()
        {
            Esprit.Point point = null;

            try
            {
                point = Document.GetPoint("Select Destination Point");
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                return;
            }

            if (point == null)
            {
                return;
            }

            var plane = Document.ActivePlane;
            plane.X = point.X;
            plane.Y = point.Y;
            plane.Z = point.Z;
            plane.Activate();
        }

        //! [Code snippet set]

        //! [Code snippet translate]

        private void TranslateWorkPlaneToPointDemo()
        {
            Esprit.Point point = null;
            try
            {
                point = Document.GetPoint("Select Destination Point");
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                return;
            }

            if (point == null)
            {
                return;
            }

            var plane = Document.ActivePlane;
            plane.Translate(point.X - plane.X, point.Y - plane.Y, point.Z - plane.Z);
            plane.Activate();
        }

        //! [Code snippet translate]

        public override void Execute()
        {
            TranslateWorkPlaneToPointDemo();
            Document.Refresh();
        }

        public override string Name => "Introduction to Plane Manipulation Methods";
        public override string HtmlPath => "html/plane_manipulation_methods_tutorial.html";

    }
}
