using System;
using System.Text;
using System.Windows;

using System.Windows.Forms;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class FunctionsTutorial : BaseTutorial
    {
        private Form _form;

        private void Show()
        {
            _form = new Form();
            Button displayLayerCountsButton = new Button();
            Button removeEmptyLayersButton = new Button();
            Button findMidPointButton = new Button();
            Button activateLayerButton = new Button();
            Button createLineButton = new Button();

            _form.Text = "FunctionsTutorial form";
            displayLayerCountsButton.Text = "Layer Counts";
            removeEmptyLayersButton.Text = "Remove Empty Layers";
            findMidPointButton.Text = "Find Mid Point";
            activateLayerButton.Text = "Activate Layer";
            createLineButton.Text = "Create Line";

            displayLayerCountsButton.SetBounds(9, 9, 150, 23);
            removeEmptyLayersButton.SetBounds(9, 59, 150, 23);
            findMidPointButton.SetBounds(9, 109, 150, 23);
            activateLayerButton.SetBounds(9, 159, 150, 23);
            createLineButton.SetBounds(9, 209, 150, 23);

            createLineButton.AutoSize = activateLayerButton.AutoSize = findMidPointButton.AutoSize = 
                removeEmptyLayersButton.AutoSize = displayLayerCountsButton.AutoSize = true;
            createLineButton.Anchor = activateLayerButton.Anchor = findMidPointButton.Anchor = removeEmptyLayersButton.Anchor = 
                displayLayerCountsButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            _form.ClientSize = new System.Drawing.Size(250, 270);
            _form.Controls.AddRange(new Control[] { displayLayerCountsButton, removeEmptyLayersButton, findMidPointButton, activateLayerButton, createLineButton });
            _form.FormBorderStyle = FormBorderStyle.FixedDialog;
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.MinimizeBox = false;
            _form.MaximizeBox = false;

            displayLayerCountsButton.Click += OnDisplayLayerCountsButtonClick;
            removeEmptyLayersButton.Click += OnRemoveEmptyLayersButtonClick;
            findMidPointButton.Click += OnFindMidPointButtonClick;
            activateLayerButton.Click += OnActivateLayerButtonClick;
            createLineButton.Click += OnCreateLineButtonClick;

            _form.FormClosed += (o, e) => { _form = null; };

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            _form.Show(Control.FromHandle(myWindowHandle));
        }

         //! [Code snippet midpoint]

        private void MidPointDemo()
        {
            Esprit.Point point1 = null;
            Esprit.Point point2 = null;

            _form.Hide();

            try
            {
                point1 = Document.GetPoint("DateTimePicker Point 1");
                point2 = Document.GetPoint("DateTimePicker Point 2");
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
            }

            if (point1 != null && point2 != null)
            {
                var midPoint = AddPoint(Document, GetMidPoint(Document, point1, point2));

                Document.Refresh();

                EspritApplication.EventWindow.Clear();
                EspritApplication.EventWindow.Visible = true;

                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FunctionsTutorial", $"Midpoint is {midPoint.X}, {midPoint.Y}, {midPoint.Z}");
            }

            _form.Show();
        }

        private static Esprit.Point AddPoint(Esprit.Document document, Esprit.Point point)
        {
            return document.Points.Add(point.X, point.Y, point.Z);
        }

        private static Esprit.Point GetMidPoint(Esprit.Document document, Esprit.Point point1, Esprit.Point point2)
        {
            return document.GetPoint((point1.X + point2.X) / 2.0, (point1.Y + point2.Y) / 2.0, (point1.Z + point2.Z) / 2.0);
        }

        //! [Code snippet midpoint]

        private void OnFindMidPointButtonClick(object sender, EventArgs e)
        {
            MidPointDemo();
        }

        private void OnDisplayLayerCountsButtonClick(object sender, EventArgs e)
        {
            DisplayLayerCounts();
        }

        //! [Code snippet display count]

        private void DisplayLayerCounts()
        {
            EspritApplication.EventWindow.Clear();
            EspritApplication.EventWindow.Visible = true;

            foreach (Esprit.Layer layer in Document.Layers)
            {
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FunctionsTutorial", $"Layer {layer.Number} {layer.Name} contains {GetLayerCount(Document, layer)} items.");
            }
        }

        //! [Code snippet display count]

        //! [Code snippet get count]

        private static uint GetLayerCount(Esprit.Document document, Esprit.Layer layer)
        {
            uint count = 0;
            for (var i = 1; i <= document.GraphicsCollection.Count; i++)
            {
                var graphicObject = document.GraphicsCollection[i] as Esprit.GraphicObject;
                if (graphicObject.Layer.Name == layer.Name && graphicObject.TypeName != "Unknown")
                {
                    count++;
                }
            }
            return count;
        }

        //! [Code snippet get count]

        private void OnRemoveEmptyLayersButtonClick(object sender, EventArgs e)
        {
            RemoveEmptyLayers();
        }

        //! [Code snippet remove empty]

        private void RemoveEmptyLayers()
        {
            foreach (Esprit.Layer layer in Document.Layers)
            {
                if (IsEmpty(Document, layer))
                {
                    Document.Layers.Remove(layer.Name);
                }
            }
        }

        private static bool IsEmpty(Esprit.Document document, Esprit.Layer layer)
        {
            return GetLayerCount(document, layer) == 0;
        }

        //! [Code snippet remove empty]

        //! [Code snippet exists]

        private static bool LayerExist(Esprit.Document document, string layerName)
        {
            bool result = false;

            foreach (Esprit.Layer layer in document.Layers)
            {
                if (layer.Name == layerName)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        //! [Code snippet exists]

        //! [Code snippet get layer]

        private void GetLayerDemo()
        {
            var layerName = "Default";
            if (RequestUserInput("Enter Layer to Get", "GetLayerDemo", ref layerName))
            {
                Document.ActiveLayer = GetLayer(Document, layerName);
            }
        }

        private static Esprit.Layer GetLayer(Esprit.Document document, string layerName)
        {
            if (LayerExist(document, layerName))
            {
                return document.Layers[layerName];
            }
            else
            {
                return document.Layers.Add(layerName);
            }
        }

        //! [Code snippet get layer]

        private void OnActivateLayerButtonClick(object sender, EventArgs e)
        {
            GetLayerDemo();
        }

        //! [Code snippet line]

        private void Line2Demo()
        {
            Esprit.Point point1 = null;
            Esprit.Point point2 = null;

            _form.Hide();

            try
            {
                point1 = Document.GetPoint("Select First Point");
                point2 = Document.GetPoint("Select Second Point");
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
            }

            if (point1 != null && point2 != null)
            {
                var line = Line2(Document, point1, point2);

                EspritApplication.EventWindow.Clear();
                EspritApplication.EventWindow.Visible = true;

                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FunctionsTutorial", $"L.Ux = {line.Ux}");
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FunctionsTutorial", $"L.Uy = {line.Uy}");
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FunctionsTutorial", $"L.Uz = {line.Uz}");
            }

            _form.Show();

            Document.Refresh();
        }

        private static Esprit.Line Line2(Esprit.Document document, Esprit.Point point1, Esprit.Point point2)
        {
            var deltaX = point2.X - point1.X;
            var deltaY = point2.Y - point1.Y;
            var deltaZ = point2.Z - point1.Z;
            var distance = Geometry.Distance(point1, point2);
            return document.Lines.Add(point1, deltaX / distance, deltaY / distance, deltaZ / distance);
        }

        //! [Code snippet line]

        private void OnCreateLineButtonClick(object sender, EventArgs e)
        {
            Line2Demo();
        }

        public FunctionsTutorial(Esprit.Application app) : base(app)
        {
        }

        public override void Execute()
        {
            if (_form == null)
            {
                Show();
            }
        }

        public override string Name => "Fun with Functions";
        public override string HtmlPath => "html/functions_tutorial.html";

    }
}
