using System;
using System.Collections.Generic;
using System.Windows;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class MachiningFaceProfilesTutorial : BaseTutorial
    {
        public MachiningFaceProfilesTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void MachineHoleProfiles()
        {
            var plane = Document.ActivePlane;
            if (plane.Name == string.Empty || plane.Name == "UVW")
            {
                MessageBox.Show("Must activate a named plane. This code cannot use UVW.", "MachiningFaceProfilesTutorial", MessageBoxButton.OK);
                return;
            }

            var set = SelectionSetHelper.GetSelectionSet(Document, "Temp");
            set.RemoveAll();

            Esprit.Solid solid = null;

            try
            {
                solid = Document.GetAnyElement("Select Reference Solid", EspritConstants.espGraphicObjectType.espSolidModel) as Esprit.Solid;
            }
            catch(System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                return;
            }

            if (solid == null)
            {
                return;
            }

            var minPocketDiameter = (Document.SystemUnit == EspritConstants.espUnitType.espInch)
                ? 2.0
                : 50.0;

            if (!RequestUserInput("Enter the Minimum Diameter for Pocketing", "Minimum Diameter", ref minPocketDiameter))
            {
                return;
            }

            EspritApplication.Configuration.ConfigurationFeatureRecognition.Tolerance = (Document.SystemUnit == EspritConstants.espUnitType.espInch)
                ? 0.0001
                : 0.0025;

            EspritApplication.Configuration.ConfigurationHoleRecognition.ConnectAllHoles = true;
            EspritApplication.Configuration.ConfigurationHoleRecognition.MaxDiameterDefined = false;
            EspritApplication.Configuration.ConfigurationHoleRecognition.MinDiameterDefined = true;
            EspritApplication.Configuration.ConfigurationHoleRecognition.MinDiameter = minPocketDiameter;

            EspritApplication.Configuration.ConfigurationProfileRecognition.CreateHoles = true;
            EspritApplication.Configuration.ConfigurationProfileRecognition.CreateProfilesOnHoles = true;

            set.Add(solid);

            Array faceFeatures = null;
            try
            {
                faceFeatures = Document.FeatureRecognition.CreateFaceProfileFeatures(set, new Esprit.Plane[1] { plane });
            }
            catch(Exception)
            {
                MessageBox.Show("Fail on creationg face profile features.", "MachiningFaceProfilesTutorial", MessageBoxButton.OK);
                return;
            }

            Esprit.FeatureChain outsideProfile = null;
            double outsideArea = 0;
            Esprit.FeaturePtop drillFeature = null;
            var holeProfiles = new List<Esprit.FeatureChain>();
            var cutoutProfiles = new List<Esprit.FeatureChain>();

            for (var i = faceFeatures.GetLowerBound(0); i <= faceFeatures.GetUpperBound(0); i++)
            {
                var graphicObject = faceFeatures.GetValue(i) as Esprit.GraphicObject;
                switch (graphicObject.GraphicObjectType)
                {
                    case EspritConstants.espGraphicObjectType.espFeatureChain:
                        var featureChain = graphicObject as Esprit.FeatureChain;
                        if (featureChain.Area > outsideArea)
                        {
                            if (outsideProfile != null)
                            {
                                if (outsideProfile.IsClosed && outsideProfile.Count == 1)
                                {
                                    holeProfiles.Add(outsideProfile);
                                }
                                else
                                {
                                    cutoutProfiles.Add(outsideProfile);
                                }
                            }
                            outsideProfile = featureChain;
                            outsideArea = outsideProfile.Area;
                        }
                        else
                        {
                            if (featureChain.IsClosed && featureChain.Count == 1)
                            {
                                holeProfiles.Add(featureChain);
                            }
                            else
                            {
                                cutoutProfiles.Add(featureChain);
                            }
                        }
                        break;
                    case EspritConstants.espGraphicObjectType.espFeaturePtop:
                        var featurePtop = graphicObject as Esprit.FeaturePtop;
                        if (drillFeature == null)
                        {
                            drillFeature = featurePtop;
                        }
                        else if (featurePtop.Count > drillFeature.Count)
                        {
                            drillFeature = featurePtop;
                        }
                        break;
                }
            }

            if (holeProfiles.Count == 0 && cutoutProfiles.Count == 0)
            {
                return;
            }

            if (drillFeature != null)
            {
                var drill = new EspritTechnology.TechMillDrill();
                if (Document.GUI.EditTechnology(drill) == EspritConstants.espGuiReturnVal.espGuiComplete)
                {
                    try
                    {
                        var obj = Document.PartOperations.Add(drill, drillFeature) as Esprit.GraphicObject;
                        GraphicObjectHelper.SetDefaultAttributes(obj, EspritApplication.Configuration);
                    }
                    catch(System.Runtime.InteropServices.COMException e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }

            EspritTechnology.TechMillPocket2 millPocket1 = new EspritTechnology.TechMillPocket2();
            EspritTechnology.TechMillPocket2 millPocket2 = new EspritTechnology.TechMillPocket2();
            EspritTechnology.TechMillPocket2 millPocket3 = new EspritTechnology.TechMillPocket2();

            if (Document.GUI.EditTechnology(millPocket1) == EspritConstants.espGuiReturnVal.espGuiCancel)
            {
                return;
            }
            var genericTech = millPocket1 as EspritTechnology.Technology;
            genericTech.CopyTo(millPocket2 as EspritTechnology.Technology, true);
            genericTech.CopyTo(millPocket3 as EspritTechnology.Technology, true);

            for (var i = 0; i < holeProfiles.Count; i++)
            {
                millPocket2.EntryMode = EspritConstants.espMillPocketEntryMode.espMillPocketEntryModePlungeAtPoint;
                millPocket2.TotalDepth = holeProfiles[i].Depth;

                var arc = holeProfiles[i].Item[1] as Esprit.Arc;
                holeProfiles[i].LeadInPoint = arc.CenterPoint;

                try
                {
                    var op = Document.PartOperations.Add(millPocket2, holeProfiles[i]);
                    GraphicObjectHelper.SetDefaultAttributes(op as Esprit.GraphicObject, EspritApplication.Configuration);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            for (var i = 0; i < cutoutProfiles.Count; i++)
            {
                if (millPocket3.EntryMode == EspritConstants.espMillPocketEntryMode.espMillPocketEntryModeHelicalAtPoint)
                {
                    millPocket3.EntryMode = EspritConstants.espMillPocketEntryMode.espMillPocketEntryModeHelixProtected;
                }
                else if (millPocket3.EntryMode == EspritConstants.espMillPocketEntryMode.espMillPocketEntryModePlungeAtPoint)
                {
                    millPocket3.EntryMode = EspritConstants.espMillPocketEntryMode.espMillPocketEntryModePlunge;
                }

                millPocket3.TotalDepth = cutoutProfiles[i].Depth;

                try
                {
                    var op = Document.PartOperations.Add(millPocket3, cutoutProfiles[i]);
                    GraphicObjectHelper.SetDefaultAttributes(op as Esprit.GraphicObject, EspritApplication.Configuration);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            Document.SelectionSets.Remove(set.Name);
            Document.Refresh();
        }

        //! [Code snippet]

        public override void Execute()
        {
            MachineHoleProfiles();
        }

        public override string Name => "Machining Face Profiles";
        public override string HtmlPath => "html/machining_face_profiles_tutorial.html";

    }
}
