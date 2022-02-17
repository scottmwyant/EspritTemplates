using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorialCSharp.Tutorials.Helpers
{
    public static class LayerHelpers
    {
        //! [Code snippet get empty layer]

        public static Esprit.Layer GetEmptyLayer(Esprit.Document document, string name)
        {
            foreach (Esprit.Layer l in document.Layers)
            {
                if (l.Name == name)
                {
                    document.Layers.Remove(name);
                    break;
                }
            }

            var layer = document.Layers.Add(name);

            return layer;
        }

        //! [Code snippet get empty layer]
    }
}
