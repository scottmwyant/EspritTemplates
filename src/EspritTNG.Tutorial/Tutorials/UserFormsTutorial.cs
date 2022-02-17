using System;
using System.Windows.Forms;

using EspritConstants;
using EspritTechnology;

namespace TutorialCSharp.Tutorials
{
    class UserFormsTutorial : BaseTutorial
    {
        private Form _form;
        private TextBox txtNumberOfPasses;
        private TextBox txtInitialStock;
        private TextBox txtFinalStock;

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

        public void Show()
        {
            _form = new Form();
            Label lblNumberOfPasses = new Label();
            txtNumberOfPasses = new TextBox();
            Label lblInitialStock = new Label();
            txtInitialStock = new TextBox();
            Label lblFinalStock = new Label();
            txtFinalStock = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();
            Button editTechnologyButton = new Button();

            _form.Text = "UserFormsTutorial form";
            lblNumberOfPasses.Text = "Number of Passes";
            lblInitialStock.Text = "Initial Stock";
            txtNumberOfPasses.Text = "3";
            txtInitialStock.Text = "3.0";
            lblFinalStock.Text = "Final Stock";
            txtFinalStock.Text = "4.0";

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;
            editTechnologyButton.Text = "Edit Technology";

            lblNumberOfPasses.SetBounds(9, 15, 372, 20);
            txtNumberOfPasses.SetBounds(12, 40, 372, 20);
            lblInitialStock.SetBounds(9, 65, 372, 20);
            txtInitialStock.SetBounds(12, 90, 372, 20);
            lblFinalStock.SetBounds(9, 115, 372, 20);
            txtFinalStock.SetBounds(12, 140, 372, 20);

            editTechnologyButton.SetBounds(110, 272, 110, 23);
            buttonOk.SetBounds(228, 272, 75, 23);
            buttonCancel.SetBounds(309, 272, 75, 23);

            _form.ClientSize = new System.Drawing.Size(396, 307);
            _form.Controls.AddRange(new Control[] { lblInitialStock,
                txtInitialStock, lblFinalStock, txtFinalStock, lblNumberOfPasses, txtNumberOfPasses, editTechnologyButton, buttonOk, buttonCancel });
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

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            _form.Show(Control.FromHandle(myWindowHandle));
        }

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

            if (!int.TryParse(txtNumberOfPasses.Text, out var numberOfPasses))
            {
                return;
            }

            if (numberOfPasses > 0)
            {
                double.TryParse(txtInitialStock.Text, out var initialStock);
                double.TryParse(txtFinalStock.Text, out var finalStock);

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

                double stockIncrement = numberOfPasses == 1 ? 0 : (initialStock - finalStock) / (numberOfPasses - 1);
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

                Document.Refresh();

            }
        }

        //! [Code snippet OK]

        public UserFormsTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet execute]

        public void MultiPassContourForm()
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
            MultiPassContourForm();
        }

        public override string Name => "Introduction to Using UserForms";
        public override string HtmlPath => "html/userforms_tutorial.html";

    }
}
