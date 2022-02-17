using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class CreateNotesBlockTutorial : BaseTutorial
    {
        public CreateNotesBlockTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        private void SetNoteGeometry(Esprit.INote note, EspritConstants.espUnitType unit, Esprit.IPoint startPoint, double height, double x, double y)
        {
            double coefUnit = (unit == EspritConstants.espUnitType.espInch) ? 1 : 25.4;

            note.Height = coefUnit * height / 72.0;
            note.X = startPoint.X + coefUnit * x;
            note.Y = startPoint.Y + coefUnit * y;
            note.Z = startPoint.Z;
        }

        private Esprit.INote AddNote(Esprit.Notes collection, string text, bool isBold)
        {
            var note = collection.Add();
            
            note.String = text;
            note.Font.Name = "Arial";
            note.Font.Bold = isBold;

            return note;
        }

        public void AddCompanyInfoNotes()
        {
            Esprit.Point point = null;
            try
            {
                point = Document.GetPoint("Select Lower Left Corner for Company Info Location");
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                return;
            }

            if (point == null)
            {
                return;
            }

            var notes = Document.Annotations.AddNotes();
            notes.Color = ColorHelper.ColorToUInt(System.Drawing.Color.Red);

            var note = AddNote(notes, "ESPRIT", true);
            SetNoteGeometry(note, Document.SystemUnit, point, 48, 1.5, 1.5);
            
            notes = Document.Annotations.AddNotes();
            notes.Color = ColorHelper.ColorToUInt(System.Drawing.Color.Black);

            note = AddNote(notes, "The", true);
            SetNoteGeometry(note, Document.SystemUnit, point, 12, 1.5, 1.3);
            
            note = AddNote(notes, "NEXT", true);
            SetNoteGeometry(note, Document.SystemUnit, point, 12, 1.975, 1.3);

            note = AddNote(notes, "Generation", true);
            SetNoteGeometry(note, Document.SystemUnit, point, 12, 2.575, 1.3);

            note = AddNote(notes, "CAM", true);
            SetNoteGeometry(note, Document.SystemUnit, point, 12, 3.5, 1.3);

            notes = Document.Annotations.AddNotes();
            notes.Color = ColorHelper.ColorToUInt(System.Drawing.Color.Black);

            note = AddNote(notes, "DP Technology Corp.", true);
            SetNoteGeometry(note, Document.SystemUnit, point, 12, 0.75, 0.8);

            note = AddNote(notes, "1150 Avenida Acaso", false);
            SetNoteGeometry(note, Document.SystemUnit, point, 12, 0.825, 0.55);

            note = AddNote(notes, "Camarillo, CA 93012", false);
            SetNoteGeometry(note, Document.SystemUnit, point, 12, 0.8, 0.3);

            note = AddNote(notes, "USA", false);
            SetNoteGeometry(note, Document.SystemUnit, point, 12, 2, 0.07);

            notes = Document.Annotations.AddNotes();
            notes.Color = ColorHelper.ColorToUInt(System.Drawing.Color.Black);

            note = AddNote(notes, "Tel: 805 388 6000", false);
            SetNoteGeometry(note, Document.SystemUnit, point, 12, 3.74, 0.8);

            note = AddNote(notes, "Fax: 805 388 3085", false);
            SetNoteGeometry(note, Document.SystemUnit, point, 12, 3.7, 0.55);

            note = AddNote(notes, "esprit@dptechnology.com", true);
            SetNoteGeometry(note, Document.SystemUnit, point, 12, 3.17, 0.3);

            notes = Document.Annotations.AddNotes();
            notes.Color = ColorHelper.ColorToUInt(System.Drawing.Color.Blue);

            note = AddNote(notes, "www.dptechnology.com", true);
            SetNoteGeometry(note, Document.SystemUnit, point, 12, 3.327, 0.05);

            Document.Refresh();
        }

        //! [Code snippet]

        public override void Execute()
        {
            AddCompanyInfoNotes();
        }

        public override string Name => "Create Notes Block";
        public override string HtmlPath => "html/create_notes_block_tutorial.html";

    }
}
