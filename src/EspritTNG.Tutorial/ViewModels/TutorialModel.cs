using System.Collections.ObjectModel;
using System.IO;
using TutorialCSharp.Tutorials;

namespace TutorialCSharp.ViewModels
{
    class TutorialModel : Notifier
    {
        private string _htmlPath;

        private string _tutorialsHtmlBasePath => Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        public ObservableCollection<TutorialModel> Tutorials { get; set; }

        public TutorialModel(string name, string htmlpath)
        {
            Tutorials = new ObservableCollection<TutorialModel>();

            Name = name;
            HtmlPath = htmlpath;
        }

        public TutorialModel(ITutorial tutorial) : this(tutorial.Name, tutorial.HtmlPath)
        {
            Tutorial = tutorial;
        }

        public ITutorial Tutorial { get; }

        public string Name { get; }

        public string HtmlPath
        {
            get => ( _htmlPath == "" ) ? null : _htmlPath;

            set
            {
                _htmlPath = "";

                if (!System.String.IsNullOrEmpty(value))
                    _htmlPath = Path.Combine(_tutorialsHtmlBasePath, value);

                if(!File.Exists(_htmlPath))
                    _htmlPath = "";
            }
        }
    }
}
