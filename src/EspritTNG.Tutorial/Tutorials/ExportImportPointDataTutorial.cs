using System;
using System.IO;
using System.Windows.Forms;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class ExportImportPointDataTutorial : BaseTutorial
    {
        private Form _form;
        private readonly string _fileName;

        private void Show()
        {
            _form = new Form();
            var exportButton = new Button();
            var importButton = new Button();

            _form.Text = "ExportImportPointDataTutorial";
            exportButton.Text = "Export points";
            importButton.Text = "Import points";

            exportButton.SetBounds(5, 5, 200, 25);
            importButton.SetBounds(5, 40, 200, 25);

            _form.ClientSize = new System.Drawing.Size(250, 100);
            _form.Controls.AddRange(new Control[] { exportButton, importButton });
            _form.FormBorderStyle = FormBorderStyle.FixedDialog;
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.MinimizeBox = false;
            _form.MaximizeBox = false;

            _form.FormClosed += OnFormClosed;
            exportButton.Click += ExportButtonClick;
            importButton.Click += ImportButtonClick;

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            _form.Show(Control.FromHandle(myWindowHandle));
        }

        //! [Code snippet Import]

        private void ImportPoints()
        {
            var filePath = Path.Combine(EspritApplication.TempDir, _fileName + ".csv");
            if (!File.Exists(filePath))
            {
                System.Windows.MessageBox.Show("Import file does not exist");
                return;
            }

            using (var inStream = new StreamReader(filePath))
            {
                var charSeparators = new char[] { ' ' };
                var data = string.Empty;

                while ((data = inStream.ReadLine()) != null)
                {
                    var pointData = data.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                    if (pointData.Length != 3)
                    {
                        continue;
                    }

                    if (!double.TryParse(pointData[0], out var x))
                    {
                        continue;
                    }
                    if (!double.TryParse(pointData[1], out var y))
                    {
                        continue;
                    }
                    if (!double.TryParse(pointData[2], out var z))
                    {
                        continue;
                    }

                    if (x != 0 || y != 0 || z != 0)
                    {
                        GraphicObjectHelper.SetDefaultAttributes(Document.Points.Add(x, y, z) as Esprit.GraphicObject, EspritApplication.Configuration);
                    }
                }
            }

            Document.Refresh();
        }

        //! [Code snippet Import]

        private void ImportButtonClick(object sender, EventArgs e)
        {
            ImportPoints();
        }

        //! [Code snippet Export]

        private void ExportPoints()
        {
            var filePath = Path.Combine(EspritApplication.TempDir, _fileName + ".csv");
            using (var outStream = new StreamWriter(filePath))
            {
                foreach (Esprit.Point point in Document.Points)
                {
                    outStream.WriteLine($"{point.X} {point.Y} {point.Z}");
                }
            }
        }

        //! [Code snippet Export]

        private void ExportButtonClick(object sender, EventArgs e)
        {
            ExportPoints();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _form = null;
        }

        public ExportImportPointDataTutorial(Esprit.Application app): base(app)
        {
            _fileName = Guid.NewGuid().ToString();
        }

        public override void Execute()
        {
            if (_form == null)
            {
                Show();
            }
        }

        public override string Name => "Exporting and Importing Point Data";
        public override string HtmlPath => "html/export_import_point_data_tutorial.html";

    }
}
