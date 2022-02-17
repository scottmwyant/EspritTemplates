using System;
using System.Windows.Forms;

namespace TutorialCSharp.Tutorials
{
    class UserFormsAdvancedTutorial : BaseTutorial
    {
        private Form _form;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;

        RadioButton optEvenPasses;
        RadioButton optSpecifyStock;

        Label label1;
        Label label2;
        Label label3;

        EspritTechnology.TechMillContour1 _techMillContour1;

        //! [Code snippet close]

        void CloseForm()
        {
            if (_form != null)
            {
                _form.Close();
                _form = null;
            }
        }

        //! [Code snippet close]

        //! [Code snippet evenpasses2]

        private void SpecifyEvenPasses()
        {
            label1.Text = "Number of Passes";
            label2.Text = "Initial Stock";
            label3.Text = "Final Stock";

            textBox1.Text = "4";
            textBox3.Text = "0";

            textBox2.Text = (Document.SystemUnit == EspritConstants.espUnitType.espMetric)
                ? "12.5"
                : "0.5";
        }

        //! [Code snippet evenpasses2]

        //! [Code snippet stock2]

        private void SpecifyStock()
        {
            label1.Text = "Stock on Pass 1";
            label2.Text = "Stock on Pass 2";
            label3.Text = "Stock on Pass 3";

            if (Document.SystemUnit == EspritConstants.espUnitType.espMetric)
            {
                textBox1.Text = "10";
                textBox2.Text = "5";
            }
            else
            {
                textBox1.Text = "0.5";
                textBox2.Text = "0.25";
            }

            textBox3.Text = "0";
        }

        //! [Code snippet stock2]

        public void Show()
        {
            _form = new Form();
            optEvenPasses = new RadioButton();
            optSpecifyStock = new RadioButton();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();

            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();

            Button buttonOk = new Button();
            Button buttonCancel = new Button();
            Button editTechnologyButton = new Button();

            optSpecifyStock.Text = "Specify Stock";
            optEvenPasses.Text = "Even Passes";
            _form.Text = "UserFormsAdvancedTutorial form";

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;
            editTechnologyButton.Text = "Edit Technology";

            optSpecifyStock.SetBounds(9, 15, 372, 20);
            optEvenPasses.SetBounds(119, 15, 372, 20);
            label1.SetBounds(9, 40, 372, 20);
            textBox1.SetBounds(12, 65, 372, 20);
            label2.SetBounds(9, 90, 372, 20);
            textBox2.SetBounds(12, 115, 372, 20);
            label3.SetBounds(9, 140, 372, 20);
            textBox3.SetBounds(12, 165, 72, 20);

            editTechnologyButton.SetBounds(110, 292, 110, 23);
            buttonOk.SetBounds(228, 292, 75, 23);
            buttonCancel.SetBounds(309, 292, 75, 23);

            buttonOk.Anchor = buttonCancel.Anchor = editTechnologyButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            _form.ClientSize = new System.Drawing.Size(396, 327);
            _form.Controls.AddRange(new Control[] { label1, optEvenPasses, optSpecifyStock, label2, label3, textBox1, textBox2, textBox3, editTechnologyButton, buttonOk, buttonCancel });
            _form.FormBorderStyle = FormBorderStyle.FixedDialog;
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.MinimizeBox = false;
            _form.MaximizeBox = false;
            _form.AcceptButton = buttonOk;
            _form.CancelButton = buttonCancel;

            _form.FormClosed += (o, e) => { _form = null; };
            buttonOk.Click += OnButtonOkClick;
            editTechnologyButton.Click += OnEditTechnologyButtonClick;
            buttonCancel.Click += OnCancelButtonClick;
            optSpecifyStock.CheckedChanged += OnSpecifyStockCheckedChanged;
            optEvenPasses.CheckedChanged += OnEvenPassesStockCheckedChanged;

            optEvenPasses.Checked = true;
            SpecifyEvenPasses();

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            _form.Show(Control.FromHandle(myWindowHandle));
        }

        //! [Code snippet stock1]

        private void OnSpecifyStockCheckedChanged(object sender, EventArgs e)
        {
            SpecifyStock();
            _form.Refresh();
        }

        //! [Code snippet stock1]

        //! [Code snippet evenpasses1]

        private void OnEvenPassesStockCheckedChanged(object sender, EventArgs e)
        {
            SpecifyEvenPasses();
            _form.Refresh();
        }

        //! [Code snippet evenpasses1]

        //! [Code snippet cancel]

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            CloseForm();
        }

        //! [Code snippet cancel]

        //! [Code snippet edit]

        private void OnEditTechnologyButtonClick(object sender, EventArgs e)
        {
            var tools = Document.Tools as EspritTools.Tools;

            EspritConstants.espGuiReturnVal res = EspritConstants.espGuiReturnVal.espGuiComplete;

            if (tools.Count == 0)
            {
                var technologyUtility = Document.TechnologyUtility as EspritTechnology.TechnologyUtility;
                var toolEndMill = technologyUtility.CreateTechnology(EspritConstants.espTechnologyType.espToolMillEndMill, Document.SystemUnit);

                var parameter = toolEndMill.FindParameter("ToolID");
                if (parameter != null)
                {
                    parameter.Value = "EM 3";
                }
                parameter = toolEndMill.FindParameter("Comment");
                if (parameter != null)
                {
                    parameter.Value = "This endmill was added by tutorial extension";
                }
                parameter = toolEndMill.FindParameter("ToolNumber");
                if (parameter != null)
                {
                    parameter.Value = 3;
                }
                parameter = toolEndMill.FindParameter("Coolant");
                if (parameter != null)
                {
                    parameter.Value = EspritConstants.espCoolantType.espCoolantOn;
                }

                res = Document.GUI.EditTool(toolEndMill);

                if (res != EspritConstants.espGuiReturnVal.espGuiCancel)
                {
                    tools.Add(toolEndMill);
                    _techMillContour1.ToolID = toolEndMill.Caption;
                }
            }

            if (res != EspritConstants.espGuiReturnVal.espGuiCancel)
            {
                Document.GUI.EditTechnology(_techMillContour1);
            }
        }

        //! [Code snippet edit]

        //! [Code snippet OK]

        private void OnButtonOkClick(object sender, EventArgs e)
        {
            if (_techMillContour1.ToolID == string.Empty)
            {
                MessageBox.Show("No tool was found. Press Edit Technology button to select a tool");
                return;
            }

            var initializedValue = true;
            int numberOfPasses;
            double initialStock = 0;
            double finalStock = 0;
            var stockValue = new double[3];
            var isEvenPassesChecked = optEvenPasses.Checked;

            if (isEvenPassesChecked)
            {
                initializedValue &= int.TryParse(textBox1.Text, out numberOfPasses);
                initializedValue &= double.TryParse(textBox2.Text, out initialStock);
                initializedValue &= double.TryParse(textBox3.Text, out finalStock);
            }
            else
            {
                numberOfPasses = 3;
                initializedValue &= double.TryParse(textBox1.Text, out stockValue[0]);
                initializedValue &= double.TryParse(textBox2.Text, out stockValue[1]);
                initializedValue &= double.TryParse(textBox3.Text, out stockValue[2]);
            }

            if (!initializedValue || numberOfPasses < 2)
                return;

            CloseForm();

            Esprit.FeatureChain featureChain = null;

            try
            {
                featureChain = Document.GetAnyElement("Select Reference Feature", EspritConstants.espGraphicObjectType.espFeatureChain) as Esprit.FeatureChain;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Element was not set
                return;
            }

            if (featureChain == null)
            {
                return;
            }

            if ((_techMillContour1.LeadInType == EspritConstants.espMillLeadinType.espMillLeadinPosition) ||
                                   (_techMillContour1.LeadInType == EspritConstants.espMillLeadinType.espMillLeadinPositionRadius) ||
                                   (_techMillContour1.LeadInType == EspritConstants.espMillLeadinType.espMillLeadinPositionRadiusNormal))
            {
                try
                {
                    featureChain.LeadInPoint = Document.GetPoint("Select Entry Point");
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    // Element was not set
                    return;
                }
            }

            if ((_techMillContour1.LeadOutType == EspritConstants.espMillLeadoutType.espMillLeadoutPosition) ||
               (_techMillContour1.LeadOutType == EspritConstants.espMillLeadoutType.espMillLeadoutPositionRadius) ||
               (_techMillContour1.LeadOutType == EspritConstants.espMillLeadoutType.espMillLeadoutPositionRadiusNormal))
            {
                try
                {
                    featureChain.LeadOutPoint = Document.GetPoint("Select Exit Point");
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    // Element was not set
                    return;
                }
            }

            if (isEvenPassesChecked)
            {
                double stockIncrement = (initialStock - finalStock) / (numberOfPasses - 1);

                for (var i = 0; i < numberOfPasses; i++)
                {
                    _techMillContour1.StockAllowance = initialStock - i * stockIncrement;
                    try
                    {
                        Document.PartOperations.Add(_techMillContour1, featureChain);
                    }
                    catch (System.Runtime.InteropServices.COMException)
                    {
                    }
                }
            }
            else
            {
                for (var i = 0; i < numberOfPasses; i++)
                {
                    _techMillContour1.StockAllowance = stockValue[i];
                    try
                    {
                        Document.PartOperations.Add(_techMillContour1, featureChain);
                    }
                    catch (System.Runtime.InteropServices.COMException)
                    {
                    }
                }
            }

            Document.Refresh();
        }

        //! [Code snippet OK]

        public UserFormsAdvancedTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet execute]

        public void MultiPassContourFormAdvanced()
        {
            if (_techMillContour1 == null)
            {
                _techMillContour1 = new EspritTechnology.TechMillContour1();
            }

            if (_form == null)
            {
                Show();
            }
        }

        //! [Code snippet execute]

        public override void Execute()
        {
            MultiPassContourFormAdvanced();
        }

        public override string Name => "Even More on UserForms";
        public override string HtmlPath => "html/user_forms_advanced_tutorial.html";

    }
}
