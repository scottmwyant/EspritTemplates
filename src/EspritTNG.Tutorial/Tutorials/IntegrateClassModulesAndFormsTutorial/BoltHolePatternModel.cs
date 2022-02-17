using System;

namespace TutorialCSharp.Tutorials.Tutorial_IntegrateClassModulesAndForms
{
    class BoltHolePatternModel : Notifier
    {
        private double _angleBetween = 0;
        private int _numberOfHoles = 1;

        public double AngleBetween
        {
            get => _angleBetween; 
            set
            { 
                if (Math.Abs(_angleBetween - value) < 0.001)
                {
                    return;
                }

                if (value == 0)
                {
                    NumberOfHoles = 1;
                    _angleBetween = 0;
                }
                else
                {
                    NumberOfHoles = (int)(360.0 / value);
                    _angleBetween = (360.0 / value) / NumberOfHoles * Math.Sign(value);
                }
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(NumberOfHoles));
            }
        }
        public int NumberOfHoles
        {
            get => _numberOfHoles;
            set
            {
                if (_numberOfHoles == Math.Abs(value))
                {
                    return;
                }

                _numberOfHoles = Math.Abs(value);
                if (_numberOfHoles == 0)
                {
                    _numberOfHoles = 1;
                }

                _angleBetween = (AngleBetween == 0)
                  ? (360.0 / value) / NumberOfHoles
                  : (360.0 / value) / NumberOfHoles * Math.Sign(value);

                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(AngleBetween));
            }
        }
        public double HoleDiameter { get; set; }
        public double StartAngle { get; set; }
        public double PatternDiameter { get; set; }
    }
}
