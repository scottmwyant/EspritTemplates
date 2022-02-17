using System.Collections.Generic;

namespace TutorialCSharp.Tutorials
{
    class ReorderingOperationsTutorial : BaseTutorial
    {

        //! [Code snippet]

        private void MoveGroupedOperationsToTop()
        {
            var operations = new List<Esprit.PartOperation>();
            for (var i = 1; i <= Document.Group.Count; i++)
            {
                var obj = Document.Group[i] as Esprit.IGraphicObject;
                if (obj.GraphicObjectType == EspritConstants.espGraphicObjectType.espOperation)
                {
                    var operation = obj as Esprit.PartOperation;
                    operations.Add(operation);
                }
            }

            if (operations.Count > 0)
            {
                Document.PartOperations.Reorder(operations.ToArray(), 1, true);
            }
        }

        private void MoveGroupedOperationsToBottom()
        {
            var operations = new List<Esprit.PartOperation>();
            for (var i = 1; i <= Document.Group.Count; i++)
            {
                var obj = Document.Group[i] as Esprit.IGraphicObject;
                if (obj.GraphicObjectType == EspritConstants.espGraphicObjectType.espOperation)
                {
                    var operation = obj as Esprit.PartOperation;
                    operations.Add(operation);
                }
            }

            if (operations.Count > 0)
            {
                Document.PartOperations.Reorder(operations.ToArray(), Document.PartOperations.Count, false);
            }
        }

        //! [Code snippet]

        public ReorderingOperationsTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            MoveGroupedOperationsToTop();
        }

        public override string Name => "Reordering Operations";
        public override string HtmlPath => "html/reordering_operations_tutorial.html";

    }
}
