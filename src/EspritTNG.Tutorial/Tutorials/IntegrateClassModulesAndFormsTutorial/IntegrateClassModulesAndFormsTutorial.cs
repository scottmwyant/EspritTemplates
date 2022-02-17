using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using TutorialCSharp.Tutorials.Helpers;

namespace TutorialCSharp.Tutorials
{
    class clsIntegrateClassModulesAndFormsTutorial : IDisposable
    {
        private readonly Esprit.Application _espritApplication;
        public double HoleDiameter;
        public double PatternDiameter;
        public double StartAngleRad;
        public double AngleBetweenRad;
        public int NumberOfHoles;
        public Esprit.Point LocalPatternCenter;
        private Esprit.SelectionSet LocalSet;

        public clsIntegrateClassModulesAndFormsTutorial(Esprit.Application app)
        {
            _espritApplication = app;
            if (_espritApplication.Document.SystemUnit == EspritConstants.espUnitType.espInch)
            {
                HoleDiameter = 1;
                PatternDiameter = 10;
            }
            else
            {
                HoleDiameter = 25;
                PatternDiameter = 250;
            }
            NumberOfHoles = 10;
            StartAngleRad = 0;
            AngleBetweenRad = 0;
            LocalPatternCenter = null;
        }

        public void Dispose()
        {
            RemoveHoles();
        }

        private void RemoveFeatureHole()
        {
            var featureHoleName = string.Empty;
            for (var i = LocalSet.Count; i > 0; i--)
            {
                var obj = LocalSet[i] as Esprit.IGraphicObject;
                if (obj.GraphicObjectType == EspritConstants.espGraphicObjectType.espFeaturePtop)
                {
                    var feature = LocalSet[i] as Esprit.FeaturePtop;
                    featureHoleName = feature.Name;
                    LocalSet.Remove(i);
                    break;
                }
            }

            if (featureHoleName != string.Empty)
            {
                _espritApplication.Document.FeaturePtops.Remove(featureHoleName);
            }
        }

        private void RemoveHoles()
        {
            if (LocalSet == null)
                return;

            RemoveFeatureHole();

            for (var i = LocalSet.Count; i > 0; i--)
            {
                var obj = LocalSet[i] as Esprit.IGraphicObject;
                if (obj.GraphicObjectType == EspritConstants.espGraphicObjectType.espCircle)
                {
                    var circle = LocalSet[i] as Esprit.Circle;
                    LocalSet.Remove(i);
                    _espritApplication.Document.Circles.Remove(circle.Key);
                }
            }
            _espritApplication.Document.SelectionSets.Remove(LocalSet.Name);
            _espritApplication.Document.Refresh();
        }

        public Esprit.SelectionSet DrawPattern(Esprit.Point patternCenter)
        {
            Esprit.SelectionSet set = SelectionSetHelper.AddUniqueSelectionSet(_espritApplication.Document.SelectionSets, "BoltHolePattern");
            LocalSet = set;
            RemoveHoles();
            if (patternCenter == null)
            {
                return set;
            }
            LocalPatternCenter = patternCenter;
            Esprit.FeaturePtop featurePtop = null;

            for (var i = 0; i < NumberOfHoles; i++)
            {
                var angle = StartAngleRad+ i * AngleBetweenRad;
                var x = (PatternDiameter / 2.0) * Math.Cos(angle) + patternCenter.X;
                var y = (PatternDiameter / 2.0) * Math.Sin(angle) + patternCenter.Y;
                var holeCenter = _espritApplication.Document.GetPoint(x, y, 0);
                var hole = _espritApplication.Document.Circles.Add(holeCenter, HoleDiameter / 2.0);

                if (featurePtop == null)
                {
                    featurePtop = _espritApplication.Document.FeaturePtops.Add(holeCenter);
                }
                else
                {
                    featurePtop.Add(holeCenter);
                }
                set.Add(hole);
            }

            if (featurePtop != null)
            {
                set.Add(featurePtop);
            }

            var result = SelectionSetHelper.DefaultAttributesSet(set, _espritApplication.Configuration);
            _espritApplication.Document.Refresh();

            return result;
        }
    }

    class IntegrateClassModulesAndFormsTutorial : BaseTutorial
    {
        private Tutorial_IntegrateClassModulesAndForms.BoltHolePatternModel _patternModel;
        private Tutorial_IntegrateClassModulesAndForms.BoltHolePattern _boltHolePatternDialog;
        private Tutorial_IntegrateClassModulesAndForms.BoltHoleManager _boltHoleManagerDialog;

        private Esprit.Point LocalPatternCenter;
        private List<Esprit.SelectionSet> Sets;
        private Esprit.SelectionSet LocalSet;

        public IntegrateClassModulesAndFormsTutorial(Esprit.Application app): base(app)
        {
            LocalPatternCenter = null;
            LocalSet = null;
            Sets = new List<Esprit.SelectionSet>();
            
            _patternModel = new Tutorial_IntegrateClassModulesAndForms.BoltHolePatternModel();
            InitializePatternModel();
        }

        public override void Execute()
        {
            CreateManagerDialog();
        }

        private void UpdateManager()
        {
            if (Sets.Count == 0)
            {
                _boltHoleManagerDialog.UpdateButton.IsEnabled = false;
                _boltHoleManagerDialog.MoveButton.IsEnabled = false;
                _boltHoleManagerDialog.DeleteButton.IsEnabled = false;
            }
        }

        private void CreateManagerDialog()
        {
            _boltHoleManagerDialog = new Tutorial_IntegrateClassModulesAndForms.BoltHoleManager();
            _boltHoleManagerDialog.CreateButton.Click += OnBoltHoleManagerDialogCreateButtonClick;
            _boltHoleManagerDialog.UpdateButton.Click += OnBoltHoleManagerDialogUpdateButtonClick;
            _boltHoleManagerDialog.MoveButton.Click += OnBoltHoleManagerDialogMoveButtonClick;
            _boltHoleManagerDialog.DeleteButton.Click += OnBoltHoleManagerDialogDeleteButtonClick;

            UpdateManager();

            _boltHoleManagerDialog.ShowDialog();
        }

        private void InitializePatternModel()
        {
            var reg = RegistryHelper.GetRegistry();
            _patternModel.AngleBetween = double.Parse(reg.GetValue("Angle Between", 36).ToString());
            _patternModel.HoleDiameter = double.Parse(reg.GetValue("Hole Diameter", 1).ToString());
            _patternModel.NumberOfHoles = int.Parse(reg.GetValue("Number of Holes", 10).ToString());
            _patternModel.PatternDiameter = double.Parse(reg.GetValue("Pattern Diameter", 10).ToString());
            _patternModel.StartAngle = double.Parse(reg.GetValue("Start Angle", 0).ToString());
        }

        private void OnBoltHoleManagerDialogCreateButtonClick(object sender, RoutedEventArgs e)
        {
            _boltHoleManagerDialog.Close();
            InitializePatternModel();

            _boltHolePatternDialog = new Tutorial_IntegrateClassModulesAndForms.BoltHolePattern { DataContext = _patternModel };
            _boltHolePatternDialog.ApplyButton.Click += OnCreateBoltHolePatternDialogApplyButtonClick;
            _boltHolePatternDialog.ShowDialog();
        }

        private void OnBoltHoleManagerDialogUpdateButtonClick(object sender, RoutedEventArgs e)
        {
            _boltHoleManagerDialog.Close();
            InitializePatternModel();

            _boltHolePatternDialog = new Tutorial_IntegrateClassModulesAndForms.BoltHolePattern { DataContext = _patternModel };
            _boltHolePatternDialog.ApplyButton.Click += OnUpdateBoltHolePatternDialogApplyButtonClick;
            _boltHolePatternDialog.ShowDialog();
        }

        private void OnBoltHoleManagerDialogMoveButtonClick(object sender, RoutedEventArgs e)
        {
            int patternNumber = 1;
            for (var i = 0; i < Sets.Count; i++)
            {
                if (Sets[i].Name == LocalSet.Name)
                {
                    patternNumber = i + 1;
                    break;
                }
            }

            if (RequestUserInput($"Enter Pattern Number to Move (1 to {Sets.Count + 1})", "Pattern Number", ref patternNumber))
            {
                if (patternNumber > 0 && patternNumber <= Sets.Count)
                {
                    LocalSet = Sets[patternNumber - 1];
                    _boltHoleManagerDialog.Close();

                    Esprit.Point point = null;
                    try
                    {
                        point = Document.GetPoint("Pick new center point");
                    }
                    catch (System.Runtime.InteropServices.COMException)
                    {
                        // Point was not set
                        return;
                    }
                    Move(point);
                    CreateManagerDialog();
                }
            }
        }

        private void OnBoltHoleManagerDialogDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            Sets.Remove(LocalSet);
            RemoveHoles();
            Document.SelectionSets.Remove(LocalSet.Name);
            LocalSet = Sets.FirstOrDefault();
            
            UpdateManager();
        }

        private void SaveSetSettings()
        {
            var reg = RegistryHelper.GetRegistry();
            reg.SetValue("Angle Between", _patternModel.AngleBetween);
            reg.SetValue("Hole Diameter", _patternModel.HoleDiameter);
            reg.SetValue("Number of Holes", _patternModel.NumberOfHoles);
            reg.SetValue("Pattern Diameter", _patternModel.PatternDiameter);
            reg.SetValue("Start Angle", _patternModel.StartAngle);
        }

        private void OnCreateBoltHolePatternDialogApplyButtonClick(object sender, RoutedEventArgs e)
        {
            Esprit.Point centerPoint = null;
            try
            {
                centerPoint = Document.GetPoint("Pick the pattern center");
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Point was not set
                return;
            }

            SaveSetSettings();
            Sets.Add(DrawPattern(centerPoint, null));
            
            CreateManagerDialog();
            UpdateManager();
        }

        private void OnUpdateBoltHolePatternDialogApplyButtonClick(object sender, RoutedEventArgs e)
        {
            Esprit.Point centerPoint = null;
            try
            {
                centerPoint = Document.GetPoint("Pick the pattern center");
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Point was not set
                return;
            }

            SaveSetSettings();
            DrawPattern(centerPoint, LocalSet);
            CreateManagerDialog();
        }

        public void RemoveFeatureHole()
        {
            var featureHoleName = string.Empty;
            for (var i = LocalSet.Count; i > 0; i--)
            {
                var obj = LocalSet[i] as Esprit.IGraphicObject;
                if (obj.GraphicObjectType == EspritConstants.espGraphicObjectType.espFeaturePtop)
                {
                    var feature = LocalSet[i] as Esprit.FeaturePtop;
                    featureHoleName = feature.Name;
                    LocalSet.Remove(i);
                    break;
                }
            }

            if (featureHoleName != string.Empty)
            {
                Document.FeaturePtops.Remove(featureHoleName);
            }
        }

        public Esprit.SelectionSet Move(Esprit.Point point)
        {
            var x = point.X - LocalPatternCenter.X;
            var y = point.Y - LocalPatternCenter.Y;
            var z = point.Z - LocalPatternCenter.Z;

            LocalSet.Translate(x, y, z);
            LocalPatternCenter = point;
            Document.Refresh();
            return LocalSet;
        }

        public void RemoveHoles()
        {
            RemoveFeatureHole();

            for (var i = LocalSet.Count; i > 0; i--)
            {
                var obj = LocalSet[i] as Esprit.IGraphicObject;
                if (obj.GraphicObjectType == EspritConstants.espGraphicObjectType.espCircle)
                {
                    var circle = LocalSet[i] as Esprit.Circle;
                    LocalSet.Remove(i);
                    Document.Circles.Remove(circle.Key);
                }
            }

            Document.Refresh();
        }

        private Esprit.SelectionSet DrawPattern(Esprit.Point patternCenter, Esprit.SelectionSet set)
        {
            if (set == null)
            {
                set = SelectionSetHelper.AddUniqueSelectionSet(Document.SelectionSets, "BoltHolePattern");
            }

            LocalSet = set;
            RemoveHoles();
            if (patternCenter == null)
            {
                return set;
            }
            LocalPatternCenter = patternCenter;
            Esprit.FeaturePtop featurePtop = null;

            for (var i = 0; i < _patternModel.NumberOfHoles; i++)
            {
                var angle = _patternModel.StartAngle * Math.PI / 180 + i * _patternModel.AngleBetween * Math.PI/180;
                var x = (_patternModel.PatternDiameter / 2.0) * Math.Cos(angle) + patternCenter.X;
                var y = (_patternModel.PatternDiameter / 2.0) * Math.Sin(angle) + patternCenter.Y;
                var holeCenter = Document.GetPoint(x, y, 0);
                var hole = Document.Circles.Add(holeCenter, _patternModel.HoleDiameter / 2.0);

                if (featurePtop == null)
                {
                    featurePtop = Document.FeaturePtops.Add(holeCenter);
                }
                else
                {
                    featurePtop.Add(holeCenter);
                }
                set.Add(hole);
            }

            if (featurePtop != null)
            {
                set.Add(featurePtop);
            }

            var result = SelectionSetHelper.DefaultAttributesSet(set, EspritApplication.Configuration);
            Document.Refresh();

            return result;
        }

        public override string Name => "Integrating Class Modules and Forms";
        public override string HtmlPath => "html/clases_forms_integrate_tutorial.html";

    }
}
