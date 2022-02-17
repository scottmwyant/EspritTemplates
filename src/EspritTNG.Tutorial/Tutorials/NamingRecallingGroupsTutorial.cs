using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace TutorialCSharp.Tutorials
{
    class NamingRecallingGroupsTutorial : BaseTutorial
    {
        ListBox _listGroups;

        public NamingRecallingGroupsTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet name]

        public void NameGroup()
        {
            var groupName = $"Group {Document.SelectionSets.Count + 1}";

            if (!RequestUserInput("Enter Group Name", $"Group Name:", ref groupName) || groupName.Length == 0)
            {
                return;
            }

            Esprit.SelectionSet selection = Document.SelectionSets.Add(groupName);

            for (int i = 1; i <= Document.Group.Count; i++)
            {
                var item = Document.Group[i] as Esprit.GraphicObject;
                selection.Add(item);
            }
        }

        //! [Code snippet name]

        //! [Code snippet recall]

        public void RecallGroup()
        {
            if (_listGroups.SelectedItems.Count > 0)
            {
                var selectedItem = _listGroups.SelectedItems;
                string groupName = selectedItem[0] as string;

                Document.Group.Clear();
                var group = Document.SelectionSets[groupName];
                for (int i = 1; i <= group.Count; i++)
                {
                    var obj = group[i] as Esprit.GraphicObject;
                    obj.Grouped = true;
                }
            }
            else
            {
                MessageBox.Show("Group Name Not Selected", "Error");
            }

            Document.Refresh();
        }

        //! [Code snippet recall]

        public DialogResult showGroupsSelectionForm()
        {
            Form displayGroupsForm = new Form();
            displayGroupsForm.Text = "Select group";

            _listGroups = new ListBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            _listGroups.AutoSize = false;
            _listGroups.SelectionMode = SelectionMode.One;

            displayGroupsForm.ClientSize = new System.Drawing.Size(220, 280);
            displayGroupsForm.Controls.AddRange(new Control[] { _listGroups, buttonOk, buttonCancel });

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            _listGroups.SetBounds(30, 10, 150, 200);
            buttonOk.SetBounds(20, 240, 80, 25);
            buttonCancel.SetBounds(130, 240, 80, 25);

            displayGroupsForm.AcceptButton = buttonOk;
            displayGroupsForm.CancelButton = buttonCancel;

            buttonOk.Anchor = buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            displayGroupsForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            displayGroupsForm.StartPosition = FormStartPosition.CenterScreen;
            displayGroupsForm.MinimizeBox = false;
            displayGroupsForm.MaximizeBox = false;

            displayGroupsForm.Load += OnFormLoad;

            return displayGroupsForm.ShowDialog();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            //! [Code snippet init listbox]

            for (int i = 1; i <= Document.SelectionSets.Count; i++)
            {
                string groupName = Document.SelectionSets[i].Name;
                _listGroups.Items.Add(groupName);
            }

            //! [Code snippet init listbox]
        }

        public override void Execute()
        {
            Form form = new Form();
            form.Text = "Naming Recalling Groups Tutorial";

            Button buttonName = new Button();
            Button buttonRecall = new Button();

            form.ClientSize = new System.Drawing.Size(300, 50);
            form.Controls.AddRange(new Control[] { buttonName, buttonRecall });

            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;

            buttonName.Text = "Name Group";
            buttonRecall.Text = "Recall Group";

            buttonName.SetBounds(40, 10, 100, 30);
            buttonRecall.SetBounds(160, 10, 100, 30);

            buttonRecall.Anchor = buttonName.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            buttonName.Click += OnButtonNameClick;
            buttonRecall.Click += OnButtonRecallClick;

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            form.Show(Control.FromHandle(myWindowHandle));
        }

        private void OnButtonNameClick(object sender, EventArgs e)
        {
            NameGroup();
        }

        private void OnButtonRecallClick(object sender, EventArgs e)
        {
            if (showGroupsSelectionForm() == DialogResult.OK)
            {
                RecallGroup();
            }
        }

        public override string Name => "Naming And Recalling Groups";
        public override string HtmlPath => "html/naming_recalling_groups_tutorial.html";

    }
}
