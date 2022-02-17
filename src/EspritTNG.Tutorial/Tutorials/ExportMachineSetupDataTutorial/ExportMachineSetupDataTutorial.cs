using System;
using System.IO;
using System.Windows;
using TutorialCSharp.Tutorials.Helpers;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TutorialCSharp.Tutorials
{
    class ExportMachineSetupDataTutorial : BaseTutorial
    {
        private Tutorial_ExportMachineSetupData.MachineSetupDialog _dialog;

        private string _stlPath;

        private bool _isBinaryMode;

        public ExportMachineSetupDataTutorial(Esprit.Application app) : base(app)
        {
        }

        //! [Code snippet transform]

        private void ApplyTransformation(Esprit.MachineItem machineItem, Esprit.Layer targetLayer, string tmpLayerName)
        {
            var selectionSet = SelectionSetHelper.GetOrAddSelectionSet(Document.SelectionSets, "Temp");
            selectionSet.RemoveAll();

            var tmpPlaneName = "TempPlane";
            EspritGeometryBase.IComMatrix mat = machineItem.CumulativeTransformation;
            Esprit.Plane plane = PlaneHelper.AddUniquePlane(Document.Planes, tmpPlaneName);
            plane.X = mat.P.X;
            plane.Y = mat.P.Y;
            plane.Z = mat.P.Z;
            plane.Ux = mat.U.X;
            plane.Uy = mat.U.Y;
            plane.Uz = mat.U.Z;
            plane.Vx = mat.V.X;
            plane.Vy = mat.V.Y;
            plane.Vz = mat.V.Z;
            plane.Wx = mat.W.X;
            plane.Wy = mat.W.Y;
            plane.Wz = mat.W.Z;

            foreach (Esprit.IGraphicObject graphicObject in Document.GraphicsCollection)
            {
                if (graphicObject.Layer.Name == tmpLayerName)
                {
                    try
                    {
                        selectionSet.Add(graphicObject);
                    }
                    catch(Exception)
                    {
                        EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeWarning, "ExportMachineSetupDataTutorial", "Exception occured - failed to add graphic object to Esprit.SetectionSet");
                    }
                }
            }

            selectionSet.AlignPlane(plane, EspritConstants.espAlignPlaneMode.espAlignPlaneFromGlobalXYZ, 0);
            selectionSet.ChangeLayer(targetLayer, 0);

            Document.Planes.Remove(plane.Name);
        }

        //! [Code snippet transform]

        //! [Code snippet import]

        private void ImportFile(Esprit.MachineItem machineItem, Esprit.Layer targetLayer, IEnumerable<string> stlFilePaths)
        {
            var tmpLayerName = "TempLayer";
            var tmpLayer = Document.Layers.Add(tmpLayerName);
            Document.ActiveLayer = tmpLayer;

            foreach (var path in stlFilePaths)
            {
                if (File.Exists(path))
                {
                    Document.MergeFile(path);
                }
                
            }

            ApplyTransformation(machineItem, targetLayer, tmpLayerName);
            Document.Layers.Remove(tmpLayerName);
        }

        //! [Code snippet import]

        //! [Code snippet export]

        private IEnumerable<string> ExportMachineItem(Esprit.MachineItem machineItem, Esprit.Layer targetLayer)
        {
            List<string> stlPaths = new List<string>();

            switch (machineItem.MachineItemType)
            {
                case EspritConstants.espMachineItemType.espMachineItemTypeFixture:
                    {
                        var fixture = machineItem as Esprit.Fixture;

                        string stlFilePath = Path.Combine(_stlPath, machineItem.Name + ".STL");

                        try
                        {
                            if (_isBinaryMode)
                            {
                                fixture.ExportAsStlBinary(stlFilePath);
                            }
                            else
                            {
                                fixture.ExportAsStlText(stlFilePath);
                            }
                        }
                        catch (Exception)
                        {
                            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeWarning, "ExportMachineSetupDataTutorial", "Exception occured during fixture exporting");
                        }

                        stlPaths.Add(stlFilePath);

                    }
                    break;

                case EspritConstants.espMachineItemType.espMachineItemTypeWorkpiece:
                    {
                        var workpiece = machineItem as Esprit.WorkpieceInstance;
                        string stlPartFilePath = Path.Combine(_stlPath, machineItem.Name + "_part.STL");
                        string stlStockFilePath = Path.Combine(_stlPath, machineItem.Name + "_stock.STL");

                        try
                        {
                            if (_isBinaryMode)
                            {
                                workpiece.ExportPartAsStlBinary(stlPartFilePath);
                            }
                            else
                            {
                                workpiece.ExportPartAsStlText(stlPartFilePath);
                            }
                        }
                        catch(Exception)
                        {
                            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeWarning, "ExportMachineSetupDataTutorial", "Exception occured during workpiece part exporting, probably it does not exist in GDML");
                        }

                        try
                        {
                            if (_isBinaryMode)
                            {
                                workpiece.ExportStockAsStlBinary(stlStockFilePath);
                            }
                            else
                            {
                                workpiece.ExportStockAsStlText(stlStockFilePath);
                            }
                        }
                        catch (Exception)
                        {
                            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeWarning, "ExportMachineSetupDataTutorial", "Exception occured during workpiece stock exporting, probably it does not exist in GDML");
                        }

                        stlPaths.Add(stlPartFilePath);
                        stlPaths.Add(stlStockFilePath);
                    }
                    break;
            }
            return stlPaths;
        }

        //! [Code snippet export]

        //! [Code snippet export import]

        private void ExportMachineItemAndLoadAsGeometry(Esprit.MachineItem machineItem, Esprit.Layer targetLayer)
        {
            var stlPaths = ExportMachineItem(machineItem, targetLayer);

            ImportFile(machineItem, targetLayer, stlPaths);

            foreach (Esprit.MachineItem item in machineItem.MachineItems)
            {
                ExportMachineItemAndLoadAsGeometry(item, targetLayer);
            }
        }

        //! [Code snippet export import]

        private void OnApplyButtonClick(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(_stlPath))
            {
                System.Windows.MessageBox.Show("No such directory! Please, choose another one.");
                return;
            }

            _dialog.Close();

            _isBinaryMode = _dialog.Binary.IsChecked.Value;

        //! [Code snippet start]

            var layer = LayerHelpers.GetEmptyLayer(Document, "Exported Setup");

            if (layer == null)
            {
                return;
            }

        //! [Code snippet start]

        //! [Code snippet execute]

            foreach (Esprit.MachineItem machineItem in Document.InitialMachineSetup.MachineItems)
            {
                ExportMachineItemAndLoadAsGeometry(machineItem, layer);
            }

        //! [Code snippet execute]
        }

        private void OnSelectDirButtonClick(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog()
            {
                SelectedPath = Environment.SpecialFolder.Personal.ToString(),
                Description = "Select the directory for STL files"
            };

            var result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _stlPath = folderBrowserDialog.SelectedPath;
            }
            _dialog.DirectoryPath.Text = _stlPath;
        }

        private void CreateManagerDialog()
        {
            _dialog = new Tutorial_ExportMachineSetupData.MachineSetupDialog();
            _dialog.ApplyButton.Click += OnApplyButtonClick;
            _dialog.SelectDirButton.Click += OnSelectDirButtonClick;

            _dialog.DirectoryPath.Text = _stlPath;
            _dialog.ShowDialog();
        }

        public override void Execute()
        {
            _stlPath = "C:/Tmp/Test/";
            _isBinaryMode = false;
            CreateManagerDialog();
        }

        public override string Name => "Export Machine Setup Data And Load As Geometry";
        public override string HtmlPath => "html/export_machine_setup_data_tutorial.html";

    }
}
