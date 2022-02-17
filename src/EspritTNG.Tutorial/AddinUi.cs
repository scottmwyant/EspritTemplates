using System;
using System.Windows.Forms;
using System.Collections.Generic;
using TutorialCSharp.Images;
using ESPRIT.NetApi.Ribbon;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.Integration;

using TutorialCSharp.ViewModels;

namespace TutorialCSharp
{
    sealed class AddinUi : IDisposable
    {
        private const string _paneKey = "CSharpTutorial";
        private const string _buttonKey = "TutorialCSharp";

        private List<IRibbonItem> _ribbonItems;
        private Esprit.Application _espritApplication;
        private ESPRIT.NetApi.Ribbon.IRibbon _ribbon;
        private ImageManager _imageManager;
        private readonly string _tabId;
        private readonly string _groupId;
        private PaneViewModel _viewModel;
        private PaneControl _paneControl;

        //Must be the same as ETNG Vb.Net Tutorial
        private const string _sid = "3f72497b-188f-4d3a-92a1-c7432cfae62a";

        private Esprit.Pane _pane;

        internal AddinUi(Esprit.Application espApp)
        {
            _tabId = _sid;
            _groupId = Guid.NewGuid().ToString();

            _imageManager = new ImageManager();

            _espritApplication = espApp;

            _ribbon = _espritApplication.Ribbon as ESPRIT.NetApi.Ribbon.IRibbon;

            _ribbon.OnButtonClick += RibbonOnButtonClick;

            CreateUi();

            _espritApplication.AfterDocumentClose += OnAfterDocumentClose;
            _espritApplication.AfterDocumentOpen += OnAfterDocumentOpen;
            _espritApplication.AfterTemplateOpen += OnAfterDocumentOpen;
            _espritApplication.AfterNewDocumentOpen += OnAfterNewDocumentOpen;

            SetUiStatus(true, (_espritApplication.Document != null));
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

        void OnAfterDocumentOpen(string fileName)
        {
            SetUiStatus(true, true);
        }

        void OnAfterDocumentClose()
        {
            SetUiStatus(true, false);
        }

        internal void CreateUi()
        {
            _ribbonItems = new List<IRibbonItem>();

            IRibbonTab tab;
            if (_ribbon.Tabs.Contains(_tabId))
            {
                tab = _ribbon.Tabs.Item(_tabId);
            }
            else
                tab = _ribbon.Tabs.Add(_tabId, @"Tutorial");

            var buttonDescription = "C# Tutorials";
            IRibbonGroup group1 = tab.Groups.Add(_groupId, buttonDescription);

            var button = group1.Items.AddButton(_buttonKey, buttonDescription, true, _imageManager.SquareIcon);
            _ribbonItems.Add(button);
        }

        void RibbonOnButtonClick(object sender, ESPRIT.NetApi.Ribbon.ButtonClickEventArgs e)
        {
            switch (e.Key)
            {
                case (_buttonKey):
                    {
                        if (_pane == null)
                        {
                            _pane = _espritApplication.Application.Panes.Add(_paneKey);
                            _pane.Caption = "C# Tutorials";
                            _pane.SetIcon(_imageManager.SquareIcon.Handle.ToInt32());

                            _viewModel = new PaneViewModel(_espritApplication);
                            _viewModel.Initialize();
                            _paneControl = new PaneControl { DataContext = _viewModel };
                            _paneControl.Execute.Click += ExecuteClick;
                            
                            var host = new ElementHost { AutoSize = true, Dock = DockStyle.Fill, Child = _paneControl, BackColor = Color.White };
                            _pane.SetControl(host);
                            _pane.Visible = true;
                        }
                        else
                        {
                            _pane.Visible = !_pane.Visible;
                        }

                        break;
                    }
            }
        }

        private void ExecuteClick(object sender, System.Windows.RoutedEventArgs e)
        {
            var tutorial = _viewModel.SelectedItem?.Tutorial;

            if (tutorial != null)
            {
                tutorial.Execute();
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

            if (_pane != null)
            {
                _espritApplication.Application.Panes.RemoveByKey(_pane.Key);
                _pane = null;
            }

            if (_ribbon != null)
            {
                if (_ribbon.Tabs.Contains(_tabId))
                {
                    var item = _ribbon.Tabs.Item(_tabId);
                    if (item.Groups.Contains(_groupId))
                    {
                        item.Groups.Remove(_groupId);
                    }

                    if (!item.Groups.Any())
                    {
                        _ribbon.Tabs.Remove(_tabId);
                    }
                }
                    
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
