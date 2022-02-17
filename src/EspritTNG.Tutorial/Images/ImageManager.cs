using System;
using System.Drawing;
using System.Reflection;

namespace TutorialCSharp.Images
{
    class ImageManager : IDisposable
    {
        private Icon _squareIcon;

        public Icon SquareIcon
        {
            get
            {
                try
                {
                    Assembly assembly = this.GetType().Assembly;
                    return _squareIcon ?? (_squareIcon = new Icon(assembly.GetManifestResourceStream(assembly.GetName().Name + "." + "Images" + ".CSharpEditor-grn.ico")));
                }
                catch
                {
                    return null;
                }
            }
        }

        public void Dispose()
        {
            if (_squareIcon != null)
            {
                _squareIcon.Dispose();
                _squareIcon = null;
            }
        }
    }
}
