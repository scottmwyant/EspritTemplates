using System;
using System.Windows;
using System.Windows.Forms;

namespace TutorialCSharp.Tutorials
{
    class GroupManagerDialogTutorial : BaseTutorial
    {
        private Form _form;
        private ListBox _listBox;

        private void Show()
        {
            _form = new Form();
            _listBox = new ListBox();
            var activeButton = new Button();
            var newButton = new Button();
            var deleteButton = new Button();
            var replaceButton = new Button();
            var renameButton = new Button();

            _form.Text = "Group Manager Dialog Tutorial";

            activeButton.Text = "Activate";
            newButton.Text = "New...";
            deleteButton.Text = "Delete";
            replaceButton.Text = "Replace";
            renameButton.Text = "Rename";

            _listBox.AutoSize = false;

            activeButton.AutoSize = newButton.AutoSize = deleteButton.AutoSize = replaceButton.AutoSize = renameButton.AutoSize = true;
            _listBox.Anchor = activeButton.Anchor = newButton.Anchor = deleteButton.Anchor = replaceButton.Anchor = renameButton.Anchor | AnchorStyles.Right;

            _listBox.SetBounds(9, 5, 158, 200);
            activeButton.SetBounds(185, 5, 172, 13);
            newButton.SetBounds(185, 35, 172, 13);
            deleteButton.SetBounds(185, 65, 172, 13);
            replaceButton.SetBounds(185, 95, 172, 13);
            renameButton.SetBounds(185, 125, 172, 13);

            _form.ClientSize = new System.Drawing.Size(396, 227);
            _form.Controls.AddRange(new Control[] { _listBox, activeButton, newButton, deleteButton, replaceButton, renameButton });
            _form.ClientSize = new System.Drawing.Size(Math.Max(300, renameButton.Right + 10), _form.ClientSize.Height);
            _form.FormBorderStyle = FormBorderStyle.FixedDialog;
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.MinimizeBox = false;
            _form.MaximizeBox = false;

            _listBox.SelectionMode = SelectionMode.MultiExtended;

            _form.FormClosed += OnFormClosed;
            activeButton.Click += OnActiveButtonClick;
            newButton.Click += OnNewButtonClick;
            deleteButton.Click += OnDeleteButtonClick;
            replaceButton.Click += OnReplaceButtonClick;
            renameButton.Click += OnRenameButtonClick;
            _listBox.MouseDoubleClick += OnMouseDoubleClick;

            //! [Code snippet init listbox]

            foreach (Esprit.SelectionSet set in Document.SelectionSets)
            {
                _listBox.Items.Add(set.Name);
            }

            //! [Code snippet init listbox]

            IntPtr myWindowHandle = new IntPtr(EspritApplication.HWND);
            _form.Show(Control.FromHandle(myWindowHandle));
        }

        private void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            OnActiveButtonClick(null, null);
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _form = null;
        }

        //! [Code snippet rename]

        private void OnRenameButtonClick(object sender, EventArgs e)
        {
            if (_listBox.SelectedItems.Count != 1)
            {
                System.Windows.MessageBox.Show("Please select one and only one Group for Rename.", "GroupManagerDialogTutorial");
            }
            else
            {
                var oldName = _listBox.SelectedItem as string;
                var newName = oldName;

                do
                {
                    if (!RequestUserInput("Enter Group Name", "Group Name", ref newName))
                    {
                        return;
                    }
                    if (newName == string.Empty)
                    {
                        System.Windows.MessageBox.Show("Name cannot be empty. Please enter group name.", "GroupManagerDialogTutorial", MessageBoxButton.OKCancel);
                    }
                    else if (!GroupNameExists(newName))
                    {
                        break;
                    }
                    else if (System.Windows.MessageBox.Show($"{newName} already exists. Please choose another name.", "GroupManagerDialogTutorial", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                } while (true);

                if (newName.Length > 0)
                {
                    Document.SelectionSets[_listBox.SelectedItem].Name = newName;
                    _listBox.Items[_listBox.SelectedIndex] = newName;
                }
            }
        }

        //! [Code snippet rename]

        //! [Code snippet replace]

        private void OnReplaceButtonClick(object sender, EventArgs e)
        {
            if (_listBox.SelectedItems.Count != 1)
            {
                System.Windows.MessageBox.Show("Please select one and only one Group for Replace.", "GroupManagerDialogTutorial");
            }
            else
            {
                var set = Document.SelectionSets[_listBox.SelectedItem];
                set.RemoveAll();
                for (var i = 1; i <= Document.Group.Count; i++)
                {
                    set.Add(Document.Group[i]);
                }
            }
        }

        //! [Code snippet replace]

        //! [Code snippet new]

        private void OnNewButtonClick(object sender, EventArgs e)
        {
            var groupName = $"Group {Document.SelectionSets.Count + 1}";
            Esprit.SelectionSet set = null;

            do
            {
                if (!RequestUserInput("Enter Group Name", "Group Name", ref groupName) || groupName.Length == 0)
                {
                    return;
                }
                else if (GroupNameExists(groupName))
                {
                    var result = System.Windows.MessageBox.Show($"{groupName} already exists. Do you wish to replace it?", "GroupManagerDialogTutorial", MessageBoxButton.YesNoCancel);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            set = Document.SelectionSets[groupName];
                            set.RemoveAll();
                            break;
                        case MessageBoxResult.No:
                            break;
                        case MessageBoxResult.Cancel:
                            return;
                    }
                }
                else
                {
                    set = Document.SelectionSets.Add(groupName);
                    _listBox.Items.Add(groupName);
                }
            } while (set == null);

            for (var i = 1; i <= Document.Group.Count; i++)
            {
                set.Add(Document.Group[i]);
            }
        }

        //! [Code snippet new]

        //! [Code snippet exist]

        private bool GroupNameExists(string groupName)
        {
            var result = false;
            foreach (Esprit.SelectionSet set in Document.SelectionSets)
            {
                if (set.Name == groupName)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        //! [Code snippet exist]

        //! [Code snippet delete]

        private void OnDeleteButtonClick(object sender, EventArgs e)
        {
            var selectedItems = _listBox.SelectedItems;
            for (var i = selectedItems.Count -1; i >= 0; i--)
            {
                Document.SelectionSets.Remove(selectedItems[i] as string);
                _listBox.Items.Remove(selectedItems[i]);
            }
        }

        //! [Code snippet delete]

        //! [Code snippet activate]

        private void OnActiveButtonClick(object sender, EventArgs e)
        {
            Document.Group.Clear();

            var selectedItems = _listBox.SelectedItems;
            for (var i = selectedItems.Count - 1; i >= 0; i--)
            {
                var set = Document.SelectionSets[selectedItems[i]];
                for (var j = 1; j <= set.Count; j++)
                {
                    var obj = set[j] as Esprit.GraphicObject;
                    obj.Grouped = true;
                }
            }

            Document.Refresh();
        }

        //! [Code snippet activate]

        public GroupManagerDialogTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet execute]

        public override void Execute()
        {
            if (_form == null)
            {
                Show();
            }
        }

        //! [Code snippet execute]

        public override string Name => "Group Manager Dialog";
        public override string HtmlPath => "html/group_manager_dialog_tutorial.html";

    }
}
