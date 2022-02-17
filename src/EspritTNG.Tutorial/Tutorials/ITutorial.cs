using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorialCSharp.Tutorials
{
    interface ITutorial
    {
        string Name { get; }
        string HtmlPath { get; }
        void Execute();
    }
}
