using System;
using System.Windows.Forms;

namespace TutorialCSharp.Tutorials
{
    class UserFormsMoreTutorial : BaseTutorial
    {
        private Form _form;
        private TextBox txtNumberOfPasses;
        private TextBox txtInitialStock;
        private TextBox txtFinalStock;
        private TextBox txtStockOnPass1;
        private TextBox txtStockOnPass2;
        private TextBox txtStockOnPass3;

        CheckBox cbEvenPasses;
        Label lblNumberOfPasses;
        Label lblInitialStock;
        Label lblFinalStock;
        Label lblStockOnPass1;
        Label lblStockOnPass2;
        Label lblStockOnPass3;

        EspritTechnology.TechMillContour1 _techMillContour1;

        //! [Code snippet close]

        private void CloseForm()
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
            cbEvenPasses = new CheckBox();
            lblNumberOfPasses = new Label();
            txtNumberOfPasses = new TextBox();
            lblInitialStock = new Label();
            txtInitialStock = new TextBox();
            lblFinalStock = new Label();
            txtFinalStock = new TextBox();

            lblStockOnPass1 = new Label();
            lblStockOnPass2 = new Label();
            lblStockOnPass3 = new Label();

            txtStockOnPass1 = new TextBox();
            txtStockOnPass2 = new TextBox();
            txtStockOnPass3 = new TextBox();
            
            Button buttonOk = new Button();
            Button buttonCancel = new Button();
            Button editTechnologyButton = new Button();

            cbEvenPasses.Text = "Even Passes";
            _form.Text = "UserFormsMoreTutorial form";
            lblNumberOfPasses.Text = "Number of Passes";
            lblInitialStock.Text = "Initial Stock";
            txtNumberOfPasses.Text = "3";
            txtInitialStock.Text = "3.0";
            lblFinalStock.Text = "Final Stock";
            txtFinalStock.Text = "4.0";

            lblStockOnPass1.Text = "Stock on Pass 1";
            txtStockOnPass1.Text = "4.0";
            lblStockOnPass2.Text = "Stock on Pass 2";
            txtStockOnPass2.Text = "5.0";
            lblStockOnPass3.Text = "Stock on Pass 3";
            txtStockOnPass3.Text = "6.0";

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;
            editTechnologyButton.Text = "Edit Technology";

            cbEvenPasses.SetBounds(9, 15, 372, 20);
            lblNumberOfPasses.SetBounds(9, 40, 372, 20);
            txtNumberOfPasses.SetBounds(12, 65, 372, 20);
            lblInitialStock.SetBounds(9, 90, 372, 20);
            txtInitialStock.SetBounds(12, 115, 372, 20);
            lblFinalStock.SetBounds(9, 140, 372, 20);
            txtFinalStock.SetBounds(12, 165, 372, 20);

            lblStockOnPass1.SetBounds(12, 190, 372, 20);
            txtStockOnPass1.SetBounds(12, 215, 372, 20);
            lblStockOnPass2.SetBounds(12, 240, 372, 20);
            txtStockOnPass2.SetBounds(12, 265, 372, 20);
            lblStockOnPass3.SetBounds(12, 290, 372, 20);
            txtStockOnPass3.SetBounds(12, 315, 372, 20);

            editTechnologyButton.SetBounds(110, 360, 110, 23);
            buttonOk.SetBounds(228, 360, 75, 23);
            buttonCancel.SetBounds(309, 360, 75, 23);

            buttonOk.Anchor = buttonCancel.Anchor = editTechnologyButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            _form.ClientSize = new System.Drawing.Size(396, 400);
            _form.Controls.AddRange(new Control[] { lblInitialStock, cbEvenPasses, txtStockOnPass1, txtStockOnPass2, txtStockOnPass3, lblStockOnPass1, lblStockOnPass2, lblStockOnPass3,
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
            cbEvenPasses.CheckedChanged += OnEnablePassesChanged;

            cbEvenPasses.Checked = true;
            EnablePasses(cbEvenPasses.Checked);

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            _form.Show(Control.FromHandle(myWindowHandle));
        }

        //! [Code snippet setcheck]

        private void EnablePasses(bool enable)
        {
            txtNumberOfPasses.Enabled = enable;
            txtInitialStock.Enabled = enable;
            txtFinalStock.Enabled = enable;

            lblNumberOfPasses.Enabled = enable;
            lblInitialStock.Enabled = enable;
            lblFinalStock.Enabled = enable;

            txtStockOnPass1.Enabled = !enable;
            txtStockOnPass2.Enabled = !enable;
            txtStockOnPass3.Enabled = !enable;

            lblStockOnPass1.Enabled = !enable;
            lblStockOnPass2.Enabled = !enable;
            lblStockOnPass3.Enabled = !enable;

            txtStockOnPass1.BackColor = enable ? System.Drawing.Color.Gray : System.Drawing.Color.White;
            txtStockOnPass2.BackColor = enable ? System.Drawing.Color.Gray : System.Drawing.Color.White;
            txtStockOnPass3.BackColor = enable ? System.Drawing.Color.Gray : System.Drawing.Color.White;

            txtNumberOfPasses.BackColor = enable ? System.Drawing.Color.White : System.Drawing.Color.Gray;
            txtInitialStock.BackColor = enable ? System.Drawing.Color.White : System.Drawing.Color.Gray;
            txtFinalStock.BackColor = enable ? System.Drawing.Color.White : System.Drawing.Color.Gray;
        }

        private void OnEnablePassesChanged(object sender, EventArgs e)
        {
            var checker = sender as CheckBox;
            EnablePasses(checker.Checked);
        }

        //! [Code snippet setcheck]

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
            var isEvenPassesChecked = cbEvenPasses.Checked;

            if (isEvenPassesChecked)
            {
                initializedValue &= int.TryParse(txtNumberOfPasses.Text, out numberOfPasses);
                initializedValue &= double.TryParse(txtInitialStock.Text, out initialStock);
                initializedValue &= double.TryParse(txtFinalStock.Text, out finalStock);
            }
            else
            {
                numberOfPasses = 3;
                initializedValue &= double.TryParse(txtStockOnPass1.Text, out stockValue[0]);
                initializedValue &= double.TryParse(txtStockOnPass2.Text, out stockValue[1]);
                initializedValue &= double.TryParse(txtStockOnPass3.Text, out stockValue[2]);
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

        public UserFormsMoreTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet execute]

        public void MultiPassContourFormOption()
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
            MultiPassContourFormOption();
        }

        public override string Name => "More On UserForms";
        public override string HtmlPath => "html/userfroms_more_tutorial.html";
    }
}
