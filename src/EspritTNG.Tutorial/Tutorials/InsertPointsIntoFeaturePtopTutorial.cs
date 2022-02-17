using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class InsertPointsIntoFeaturePtopTutorial : BaseTutorial
    {
        public InsertPointsIntoFeaturePtopTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void InsertPointIntoFeaturePtop()
        {
            Esprit.FeaturePtop featurePtop = null;
            try
            {
                featurePtop = Document.GetAnyElement("Pick Feature Ptop", EspritConstants.espGraphicObjectType.espFeaturePtop) as Esprit.FeaturePtop;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                return;
            }

            if (featurePtop == null)
            {
                return;
            }

            var previousGridMode = EspritApplication.Configuration.EnableGridMode;
            EspritApplication.Configuration.EnableGridMode = true;

            Esprit.Point insertionPoint = null;
            Esprit.Point point = null;
            try
            {
                insertionPoint = Document.GetPoint("Pick Insertion Location");
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                return;
            }
            EspritApplication.Configuration.EnableGridMode = previousGridMode;

            try
            {
                point = Document.GetPoint("Pick Point To Add");
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                return;
            }

            if (insertionPoint == null || point == null)
            {
                return;
            }

            var shortestDistance = 0.0;
            var nearestItemIndex = 0;

            for (var i = 1; i<= featurePtop.Count; i++)
            {
                var distance = Geometry.Distance(featurePtop.Item[i] as Esprit.Point, insertionPoint);
                if (i == 1)
                {
                    shortestDistance = distance;
                    nearestItemIndex = i;
                }
                else
                {
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        nearestItemIndex = i;
                    }
                }
            }

            Esprit.FeaturePtop secondFeaturePtop = null;
            if (nearestItemIndex == 1)
            {
                secondFeaturePtop = Document.FeaturePtops.Add(point);
                secondFeaturePtop.Add(featurePtop.Item[1] as Esprit.Point);
            }
            else if (nearestItemIndex > 1)
            {
                secondFeaturePtop = Document.FeaturePtops.Add(featurePtop.Item[1] as Esprit.Point);
            }
            else
            {
                return;
            }

            for (var i = 2; i <= featurePtop.Count; i++)
            {
                if (i == nearestItemIndex)
                {
                    secondFeaturePtop.Add(point);
                }

                secondFeaturePtop.Add(featurePtop.Item[i]);
            }

            Document.FeaturePtops.Remove(featurePtop.Key);
            Document.Refresh();
        }

        //! [Code snippet]

        public override void Execute()
        {
            InsertPointIntoFeaturePtop();
        }

        public override string Name => "Inserting Points into a FeaturePtop";
        public override string HtmlPath => "html/insert_points_into_featureptop_tutorial.html";

    }
}
