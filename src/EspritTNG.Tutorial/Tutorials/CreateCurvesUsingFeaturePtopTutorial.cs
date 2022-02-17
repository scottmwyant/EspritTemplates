using System.Windows;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class CreateCurvesUsingFeaturePtopTutorial : BaseTutorial
    {
        private bool _initialized = false;

        public CreateCurvesUsingFeaturePtopTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            if (!_initialized)
            {
                EspritApplication.BeforeCommandRun += OnBeforeCommandRun;
                _initialized = true;
            }

            GroupedPointsToCurveViaPtop();
        }

        //! [Code snippet]

        private void GroupedPointsToCurveViaPtop()
        {
            Esprit.FeaturePtop featurePtop = null;

            for (var i = 1; i <= Document.Group.Count; i++)
            {
                var graphicObject = Document.Group[i] as Esprit.IGraphicObject;
                if (graphicObject.GraphicObjectType == EspritConstants.espGraphicObjectType.espPoint)
                {
                    var point = graphicObject as Esprit.Point;
                    if (featurePtop == null)
                    {
                        featurePtop = Document.FeaturePtops.Add(point);
                    }
                    else
                    {
                        featurePtop.Add(point);
                    }
                }
            }

            if (featurePtop == null)
            {
                MessageBox.Show("There are no selected points");
                return;
            }

            if (featurePtop.Count > 2 && MessageBox.Show("Close the curve?", "Close Curve", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                featurePtop.Add(featurePtop.Item[1]);
            }

            try
            {
                GraphicObjectHelper.SetDefaultAttributes(Document.Curves.AddFromElement(featurePtop) as Esprit.GraphicObject, EspritApplication.Configuration);
            }
            catch (System.Runtime.InteropServices.COMException)
            { }

            Document.FeaturePtops.Remove(featurePtop.Key);
            Document.Refresh();
        }

        //! [Code snippet]

        //! [Code snippet beforecommandrun]

        private void OnBeforeCommandRun(int nCmdId, int nAuxId, ref bool bOverride)
        {
            if (nCmdId != (int)EspritConstants.espCommands.espCmdNurb3dCurveAuto)
            {
                return;
            }

            var origCount = Document.FeaturePtops.Count;
            EspritApplication.RunCommand(EspritConstants.espCommands.espCmdDefptopNewOpen);
            EspritApplication.RunCommand(EspritConstants.espCommands.espCmdDefstructEndcycle);

            if (Document.FeaturePtops.Count > origCount)
            {
                var lastPtop = Document.FeaturePtops[Document.FeaturePtops.Count];
                try
                {
                    GraphicObjectHelper.SetDefaultAttributes(Document.Curves.AddFromElement(lastPtop) as Esprit.GraphicObject, EspritApplication.Configuration);
                }
                catch (System.Runtime.InteropServices.COMException)
                { }

                Document.FeaturePtops.Remove(lastPtop.Key);

                bOverride = true;
            }
        }

        //! [Code snippet beforecommandrun]

        public override string Name => "Creating Curves Using FeaturePtop Commands";
        public override string HtmlPath => "html/create_curves_using_featureptop_tutorial.html";

    }
}
