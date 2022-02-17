namespace TutorialCSharp.Tutorials
{
    class MoveP0CircleCenterTutorial : BaseTutorial
    {
        public MoveP0CircleCenterTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void MoveP0ToCircleCenter()
        {
            Esprit.Circle circle = null;

            try
            {
                circle = Document.GetCircle("Select Reference Circle");
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                return;
            }

            if (circle != null)
            {
                Document.MoveP0(circle.CenterPoint);
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            MoveP0ToCircleCenter();
        }

        public override string Name => "Move P0 to Circle Center";
        public override string HtmlPath => "html/movep0_circle_center_tutorial.html";

    }
}
