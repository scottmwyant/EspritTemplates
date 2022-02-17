namespace TutorialCSharp.Tutorials
{
    class ModifyNoteTextTutorial : BaseTutorial
    {
        public ModifyNoteTextTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void ModifyNoteOrDimensionText()
        {
            var mask = new EspritConstants.espGraphicObjectType[2]
                {
                    EspritConstants.espGraphicObjectType.espAnnotation,
                    EspritConstants.espGraphicObjectType.espDimension
                };

            Esprit.Annotation annotation = null;
            try
            {
                annotation = Document.GetAnyElement("Pick an Annotation", mask) as Esprit.Annotation;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                return;
            }

            if (annotation == null)
            {
                return;
            }

            if (annotation.AnnotationType == EspritConstants.espAnnotationType.espAnnotationNote)
            {
                var notes = annotation as Esprit.Notes;
                for (var i = 1; i <= notes.Count; i++)
                {
                    var tempString = notes.Item[i].String;
                    if (RequestUserInput($"Enter New Note (Line {i})", "Modify Note", ref tempString))
                    {
                        notes.Item[i].String = tempString;
                    }
                }
            }
            else if (annotation.AnnotationType == EspritConstants.espAnnotationType.espAnnotationDimension)
            {
                var dimension = annotation as Esprit.Dimension;
                var notes = dimension.Notes;
                for (var i = 1; i <= notes.Count; i++)
                {
                    var tempString = notes.Item[i].String;
                    if (RequestUserInput($"Enter New Dimension (Part {i} of {notes.Count})", "Modify Note", ref tempString))
                    {
                        notes.Item[i].String = tempString;
                    }
                }
            }

            Document.Refresh();
        }

        //! [Code snippet]

        public override void Execute()
        {
            ModifyNoteOrDimensionText();
        }

        public override string Name => "Modify Note Text";
        public override string HtmlPath => "html/modify_note_text_tutorial.html";

    }
}
