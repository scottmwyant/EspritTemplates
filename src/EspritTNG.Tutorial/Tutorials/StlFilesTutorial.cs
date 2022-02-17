using System;
using System.Windows;
using System.Windows.Forms;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class StlFilesTutorial : BaseTutorial
    {
        public StlFilesTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet Import]

        private bool ImportStlFile()
        {
            var filePath = string.Empty;
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "STL file (*.stl)|*.stl";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Title = "Open STL File";
                openFileDialog.ShowDialog();
                filePath = openFileDialog.FileName;
            }

            if (!filePath.EndsWith(".stl"))
            {
                return false;
            }

            Document.MergeFile(filePath);
            return true;
        }

        //! [Code snippet Import]
        //! [Code snippet Smash]

        private void SmashStlObjects()
        {
            var selectionSet = SelectionSetHelper.GetSelectionSet(Document, "Temp");
            selectionSet.RemoveAll();

            foreach (Esprit.IGraphicObject graphicObject in Document.GraphicsCollection)
            {
                if (graphicObject.GraphicObjectType == EspritConstants.espGraphicObjectType.espSTL_Model)
                {
                    selectionSet.Add(graphicObject);
                }
            }

            var tolerance = (Document.SystemUnit == EspritConstants.espUnitType.espInch)
                ? 0.01
                : 0.25;

            try
            {
                selectionSet.Smash(true, false, false, EspritConstants.espWireFrameElementType.espWireFrameElementAll, tolerance, 0);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

            Document.Refresh();
            Document.Windows.ActiveWindow.Fit();
            Document.Windows.ActiveWindow.Refresh();

            Document.SelectionSets.Remove(selectionSet.Name);
        }

        //! [Code snippet Smash]

        public override void Execute()
        {
            if (ImportStlFile())
            {
                SmashStlObjects();
            }
            else
            {
                System.Windows.MessageBox.Show("This macro is only for STL files.", "StlFilesTutorial", MessageBoxButton.OK);
            }
        }

        public override string Name => "Introduction to STL Files";
        public override string HtmlPath => "html/stl_files_tutorial.html";

    }
}
