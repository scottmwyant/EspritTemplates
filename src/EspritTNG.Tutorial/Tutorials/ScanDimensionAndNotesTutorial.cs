using System.Text;
using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class ScanDimensionAndNotesTutorial : BaseTutorial
    {
        public ScanDimensionAndNotesTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void ScanDimensionAndNoteText()
        {
            EspritApplication.EventWindow.Clear();
            EspritApplication.EventWindow.Visible = true;

            if (Document.Annotations.Count == 0)
            {
                MessageBox.Show("There are no Annotations", "ScanDimensionAndNotesTutorial");
                return;
            }

            var index = 0;
            foreach (Esprit.Annotation annotation in Document.Annotations)
            {
                switch (annotation.AnnotationType)
                {
                    case EspritConstants.espAnnotationType.espAnnotationDimension:
                        index++;
                        var annotationDimension = annotation as Esprit.Dimension;

                        for (var i = 1; i <= annotationDimension.Notes.Count; i++)
                        {
                            var note = annotationDimension.Notes.Item[i];
                            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanDimensionAndNotesTutorial", $"Dimension {annotationDimension.Key} line {i} is {note.String}");
                        }
                        break;

                    case EspritConstants.espAnnotationType.espAnnotationNote:
                        index++;
                        var annotationNotes = annotation as Esprit.Notes;

                        for (var i = 1; i <= annotationNotes.Count; i++)
                        {
                            var note = annotationNotes.Item[i];
                            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ScanDimensionAndNotesTutorial", $"Note {annotationNotes.Key} line {i} is {note.String}");
                        }
                        break;
                }
            }
            if (index == 0)
            {
                MessageBox.Show("There are no Dimension or Note Annotations", "ScanDimensionAndNotesTutorial");
            }
        }
        //! [Code snippet]

        public override void Execute()
        {
            ScanDimensionAndNoteText();
        }

        public override string Name => "Scan Dimension and Notes Text";
        public override string HtmlPath => "html/scan_dimension_and_notes_tutorial.html";

    }
}
