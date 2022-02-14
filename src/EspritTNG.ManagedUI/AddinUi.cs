using ESPRIT.NetApi.Ribbon;
using EspritTNG.ManagedUI.Images;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EspritTNG.ManagedUI
{
    sealed class AddinUi : IDisposable
    {
        private List<IRibbonItem> _ribbonItems;
        private Esprit.Application _espritApplication;
        private ESPRIT.NetApi.Ribbon.IRibbon _ribbon;
        private ImageManager _imageManager;
        private string _tabId;

        internal AddinUi(Esprit.Application espApp)
        {
            _tabId = Guid.NewGuid().ToString();

            _imageManager = new ImageManager();

            _espritApplication = espApp;

            _ribbon = _espritApplication.Ribbon as ESPRIT.NetApi.Ribbon.IRibbon;

            _ribbon.OnButtonClick += RibbonOnButtonClick;
            _ribbon.OnCheckBoxCheckChanged += OnCheckBoxCheckChanged;

            CreateUI();

            _espritApplication.AfterDocumentClose += OnAfterDocumentClose;
            _espritApplication.AfterDocumentOpen += OnAfterDocumentOpen;
            _espritApplication.AfterTemplateOpen += OnAfterDocumentOpen;
            _espritApplication.AfterNewDocumentOpen += OnAfterNewDocumentOpen;

            SetUiStatus(true, false);
        }

        private void SetUiStatus(bool visible, bool enabled)
        {
            foreach (IRibbonItem ribbonItem in _ribbonItems)
            {
                ribbonItem.Enabled = enabled;
                ribbonItem.Visible = visible;
            }
        }

        void OnAfterNewDocumentOpen()
        {
            SetUiStatus(true, true);
        }

        void OnAfterDocumentOpen(string FileName)
        {
            SetUiStatus(true, true);
        }

        void OnAfterDocumentClose()
        {
            SetUiStatus(true, false);
        }

        internal void CreateUI()
        {
            _ribbonItems = new List<IRibbonItem>();

            IRibbonTab tab;
            tab = _ribbon.Tabs.Add(_tabId, @"EspritTNG.ManagedUI");

            IRibbonGroup group1;
            group1 = tab.Groups.Add(Guid.NewGuid().ToString(), @"Group 1");

            _ribbonItems.Add(group1.Items.AddButton(@"EspritTNG.ManagedUI_Btn1", @"Button 1", true, _imageManager.SquareIcon));
            _ribbonItems.Add(group1.Items.AddButton(@"EspritTNG.ManagedUI_Btn2", @"Button 2", true, _imageManager.SquareIcon));
            _ribbonItems.Add(group1.Items.AddButton(@"EspritTNG.ManagedUI_Btn3", @"Button 3", true, _imageManager.SquareIcon));
            _ribbonItems.Add(group1.Items.AddButton(@"EspritTNG.ManagedUI_Btn4", @"Button 4", false, _imageManager.SquareIcon));
            _ribbonItems.Add(group1.Items.AddButton(@"EspritTNG.ManagedUI_Btn5", @"Button 5", false, _imageManager.SquareIcon));

            IRibbonGroup group2;
            group2 = tab.Groups.Add(Guid.NewGuid().ToString(), @"Group 2");

            _ribbonItems.Add(group2.Items.AddCheckBox(@"EspritTNG.ManagedUI_CheckBox1", @"Check Box 1", false));
            _ribbonItems.Add(group2.Items.AddCheckBox(@"EspritTNG.ManagedUI_CheckBox2", @"Check Box 2", false));
            _ribbonItems.Add(group2.Items.AddCheckBox(@"EspritTNG.ManagedUI_CheckBox3", @"Check Box 3", false));
        }

        void OnCheckBoxCheckChanged(object sender, CheckBoxCheckChangedEventArgs e)
        {
            switch (e.Key)
            {
                case (@"EspritTNG.ManagedUI_CheckBox1"):
                    {
                        MessageBox.Show(string.Format("Check Box changed, key = {0}, value = {1}", e.Key, e.Checked.ToString()));
                        break;
                    }
                case (@"EspritTNG.ManagedUI_CheckBox2"):
                    {
                        MessageBox.Show(string.Format("Check Box changed, key = {0}, value = {1}", e.Key, e.Checked.ToString()));
                        break;
                    }
                case (@"EspritTNG.ManagedUI_CheckBox3"):
                    {
                        MessageBox.Show(string.Format("Check Box changed, key = {0}, value = {1}", e.Key, e.Checked.ToString()));
                        break;
                    }
            }
        }

        void RibbonOnButtonClick(object sender, ButtonClickEventArgs e)
        {
            switch (e.Key)
            {
                case (@"EspritTNG.ManagedUI_Btn1"):
                    {
                        MessageBox.Show(string.Format("Button pressed key = {0}", e.Key));
                        break;
                    }
                case (@"EspritTNG.ManagedUI_Btn2"):
                    {
                        MessageBox.Show(string.Format("Button pressed key = {0}", e.Key));
                        break;
                    }
                case (@"EspritTNG.ManagedUI_Btn3"):
                    {
                        MessageBox.Show(string.Format("Button pressed key = {0}", e.Key));
                        break;
                    }
                case (@"EspritTNG.ManagedUI_Btn4"):
                    {
                        MessageBox.Show(string.Format("Button pressed key = {0}", e.Key));
                        break;
                    }
                case (@"EspritTNG.ManagedUI_Btn5"):
                    {
                        MessageBox.Show(string.Format("Button pressed key = {0}", e.Key));
                        break;
                    }
            }
        }

        public void Dispose()
        {
            if (_imageManager != null)
            {
                _imageManager.Dispose();
                _imageManager = null;
            }

            if (_ribbonItems != null)
            {
                _ribbonItems.Clear();
                _ribbonItems = null;
            }

            if (_ribbon != null)
            {
                _ribbon.Tabs.Remove(_tabId);
                _ribbon.OnButtonClick -= RibbonOnButtonClick;
                _ribbon = null;
            }

            if (_espritApplication != null)
            {
                _espritApplication.AfterDocumentClose -= OnAfterDocumentClose;
                _espritApplication.AfterDocumentOpen -= OnAfterDocumentOpen;
                _espritApplication.AfterTemplateOpen -= OnAfterDocumentOpen;
                _espritApplication.AfterNewDocumentOpen -= OnAfterNewDocumentOpen;
                _espritApplication = null;
            }
        }
    }
}
