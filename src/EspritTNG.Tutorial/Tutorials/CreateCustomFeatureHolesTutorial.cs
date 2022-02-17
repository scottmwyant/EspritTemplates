using System;
using System.Collections.Generic;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class CreateCustomFeatureHolesTutorial : BaseTutorial
    {
        public CreateCustomFeatureHolesTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        private void CreateCustomFeatureHoles()
        {
            var tech = Document.TechnologyUtility.CreateTechnology(EspritConstants.espTechnologyType.espTechMillHoleMachining, Document.SystemUnit);
            var res = Document.GUI.EditTechnology(tech);
            if (res != (int)EspritConstants.espGuiReturnVal.espGuiComplete)
            {
                return;
            }

            var featureHoles = Document.HolesFeatures.Add() as Esprit.HolesFeature;
            var comFeatureHoles = featureHoles.Object as EspritFeatures.IComFeatureHoles;
            comFeatureHoles.Name = "Test HolesFeature";
            comFeatureHoles.PartID = Document.Parts[1].Key;
            comFeatureHoles.WorkCoordinateNumber = int.Parse(Document.WorkCoordinates[1].Key);
            comFeatureHoles.WorkPlaneNumber = 8;
            comFeatureHoles.FeatureType = EspritFeatures.geoFeatureType.geoHolesFeature;

            var holeNormal = new EspritGeometry.ComVector();
            holeNormal.SetXyz(0, 0, 1);

            var holePoint = new EspritGeometry.ComPoint();

            var holes = new List<EspritFeatures.IComFeatureHole>();
            for (var i = 0; i < 4; i++)
            {
                holePoint.SetXyz(10 + 20 * i, 30 + 20 * i, 0);

                var hole = new EspritFeatures.ComFeatureHole()
                {
                    HoleDiameter = 10,
                    HoleDepth = 10,
                    ThroughAll = true,
                    BoreDepth = 0,
                    BoreDiameter = 0,
                    BottomAngleDeg = 0,
                    ChamferAngleDeg = 0,
                    ChamferDiameter = 0,
                    HeadClearance = 20,
                    MajorDiameter = 0,
                    MinorDiameter = 0,
                    ThreadDepth = 0,
                    ThreadDiameter = 0,
                    ThreadPitch = 0,

                    HolePoint = holePoint,
                    HoleNormal = holeNormal,
                    Type = EspritFeatures.espFeatureHoleType.espFeatureHoleDrill
                };

                holes.Add(hole);
            }

            comFeatureHoles.SetHoles(holes.ToArray());

            try
            {
                Document.PartOperations.Add(tech, featureHoles);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "CreateCustomFeatureHolesTutorial");
            }
        }

        private void ModifyCustomFeatureHoles()
        {
            foreach(Esprit.HolesFeature featureHole in Document.HolesFeatures)
            {
                if (featureHole.Name == "Test HolesFeature")
                {
                    var comFeatureHole = featureHole.Object as EspritFeatures.IComFeatureHoles;

                    var holes = comFeatureHole.GetHoles();
                    for (var i = holes.GetLowerBound(0); i <= holes.GetUpperBound(0); i++)
                    {
                        var hole = holes.GetValue(i) as EspritFeatures.ComFeatureHole;
                        hole.HoleDiameter = 5 * (i + 1);
                    }
                }
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            CreateCustomFeatureHoles();
            ModifyCustomFeatureHoles();
        }

        public override string Name => "Create Custom Feature Holes";
        public override string HtmlPath => "html/create_custom_feature_holes_tutorial.html";

    }
}
