using System;
using System.Text;
using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class ScanAnnotationsTutorial : BaseTutorial
    {
        public ScanAnnotationsTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void ScanAnnotations()
        {
            EspritApplication.EventWindow.Clear();
            EspritApplication.EventWindow.Visible = true;

            var index = 0;
            foreach(Esprit.Annotation annotation in Document.Annotations)
            {
                index++;

                switch (annotation.AnnotationType)
                {
                    case EspritConstants.espAnnotationType.espAnnotationDimension:
                        var annotationDimension = annotation as Esprit.Dimension;
                        EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanAnnotationsTutorial", $"Item {index} is Dimension {annotation.Key}");
                        EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanAnnotationsTutorial", $"Dimension {annotationDimension.Key} contains {annotationDimension.AnnotationSegments.Count} segment(s) and {annotationDimension.Notes.Count} note(s).");
                        break;

                    case EspritConstants.espAnnotationType.espAnnotationHatch:
                        var annotationHatch = annotation as Esprit.Hatch;
                        EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanAnnotationsTutorial", $"Item {index} is Hatch {annotation.Key}");
                        EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanAnnotationsTutorial", $"{Environment.NewLine}Hatch {annotationHatch.Key} contains {annotationHatch.AnnotationSegments.Count} segment(s).");
                        break;

                    case EspritConstants.espAnnotationType.espAnnotationLeader:
                        var annotationLeader = annotation as Esprit.Leader;
                        EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanAnnotationsTutorial", $"Item {index} is Leader {annotation.Key}");
                        EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanAnnotationsTutorial", $"{Environment.NewLine}Leader {annotationLeader.Key} contains {annotationLeader.LeaderPoints.Count} leader point(s).");
                        break;

                    case EspritConstants.espAnnotationType.espAnnotationNote:
                        var annotationNotes = annotation as Esprit.Notes;
                        EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanAnnotationsTutorial", $"Item {index} is Note {annotation.Key}");
                        EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanAnnotationsTutorial", $"{Environment.NewLine}Note {annotationNotes.Key} contains {annotationNotes.Count} line(s) of text(s).");
                        break;
                }
            }

            if (index == 0)
            {
                MessageBox.Show("There are no Annotations", "ScanAnnotationsTutorial");
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            ScanAnnotations();
        }

        public override string Name => "Scan Annotations";
        public override string HtmlPath => "html/scan_annotations_tutorial.html";

    }
}
