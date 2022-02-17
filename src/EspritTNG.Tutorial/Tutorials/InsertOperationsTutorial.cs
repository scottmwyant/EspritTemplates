namespace TutorialCSharp.Tutorials
{
    class InsertOperationsTutorial : BaseTutorial
    {
        private void AutoTailstock()
        {

        }

        public InsertOperationsTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            AutoTailstock();
        }

        public override string Name => "Inserting Operations at Specific Locations";
        public override string HtmlPath => "html/insert_operations_tutorial.html";

    }
}
