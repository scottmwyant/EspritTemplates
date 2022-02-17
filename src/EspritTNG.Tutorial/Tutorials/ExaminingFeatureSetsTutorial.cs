namespace TutorialCSharp.Tutorials
{
    class ExaminingFeatureSetsTutorial : BaseTutorial
    {
        public ExaminingFeatureSetsTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void ScanAllFeatureSets()
        {
            var output = EspritApplication.EventWindow;
            output.Clear();
            output.Visible = true;

            foreach (Esprit.FeatureSet featureSet in Document.FeatureSets)
            {
                ScanFeatureSet(featureSet, output);
            }
        }

        static private void ScanFeatureSet(Esprit.FeatureSet featureSet, Esprit.EventWindow output)
        {
            output.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ExaminingFeatureSetsTutorial", $"FeatureSet {featureSet.Key} ({featureSet.Name}) contains {featureSet.Count} items");

            for (var i = 1; i <= featureSet.Count; i++)
            {
                switch (featureSet.Item[i].GraphicObjectType)
                {
                    case EspritConstants.espGraphicObjectType.espFeatureChain:
                        var featureChainObject = featureSet.Item[i] as Esprit.FeatureChain;
                        output.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ExaminingFeatureSetsTutorial", $"FeatureSet {featureSet.Key} ({featureSet.Name}) item {i} is FeatureChain {featureChainObject.Key} ({featureChainObject.Name}) type = {TypeDescription(featureChainObject.Type)}");
                        break;
                    case EspritConstants.espGraphicObjectType.espFeatureSet:
                        var featureSetObject = featureSet.Item[i] as Esprit.FeatureSet;
                        output.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ExaminingFeatureSetsTutorial", $"FeatureSet {featureSet.Key} ({featureSet.Name}) item {i} is FeatureChain {featureSetObject.Key} ({featureSetObject.Name})");
                        break;
                }
            }
        }
        static private string TypeDescription(EspritConstants.espFeatureType type)
        {
            var result = string.Empty;
            switch (type)
            {
                case EspritConstants.espFeatureType.espFeatureTypeBasic:
                    result = "Basic";
                    break;
                case EspritConstants.espFeatureType.espFeatureTypeBoundary:
                    result = "Boundary";
                    break;
                case EspritConstants.espFeatureType.espFeatureTypeHole:
                    result = "Hole";
                    break;
                case EspritConstants.espFeatureType.espFeatureTypeIsland:
                    result = "Island";
                    break;
                case EspritConstants.espFeatureType.espFeatureTypePocket:
                    result = "Pocket";
                    break;
                case EspritConstants.espFeatureType.espFeatureTypeProfile:
                    result = "Profile";
                    break;
            }

            return result;
        }

        //! [Code snippet]

        public override void Execute()
        {
            ScanAllFeatureSets();
        }

        public override string Name => "Examining Feature Sets";
        public override string HtmlPath => "html/examining_feature_sets_tutorial.html";

    }
}
