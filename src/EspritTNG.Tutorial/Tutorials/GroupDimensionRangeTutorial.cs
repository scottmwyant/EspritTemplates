using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TutorialCSharp.Tutorials
{
    class GroupDimensionRangeTutorial : BaseTutorial
    {
        public GroupDimensionRangeTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void GroupDimensionRange()
        {
            if (Document.Annotations.Count == 0)
            {
                MessageBox.Show("There are no Annotations", "GroupDimensionRangeTutorial");
                return;
            }

            var lowerLimit = 0.0;
            if (!RequestUserInput("Enter Lower Dimension Limit for Group", "Lower Limit", ref lowerLimit))
            {
                return;
            }
            var upperLimit = lowerLimit;

            if (!RequestUserInput("Enter Upper Dimension Limit for Group", "Upper Limit", ref upperLimit))
            {
                return;
            }

            var count = 0;
            foreach(Esprit.Annotation annotation in Document.Annotations)
            {
                if (annotation.AnnotationType != EspritConstants.espAnnotationType.espAnnotationDimension)
                {
                    continue;
                }

                count++;

                var annotationDimension = annotation as Esprit.Dimension;
                var firstLeterCode = Encoding.ASCII.GetBytes(annotationDimension.Notes.Item[1].String).First();
                int valueNote = ((firstLeterCode == 82) || (firstLeterCode == 216))
                    ? 2
                    : 1;

                var currentValue = 0.0;
                if (annotationDimension.Notes.Count == 1 + valueNote)
                {
                    var firstLeterOfSecondLineCode = Encoding.ASCII.GetBytes(annotationDimension.Notes.Item[2].String).First();
                    if (firstLeterOfSecondLineCode == 177)
                    {
                        double.TryParse(annotationDimension.Notes.Item[valueNote].String, out currentValue);
                    }
                    else
                    {
                        double.TryParse(annotationDimension.Notes.Item[valueNote].String, out var value1);
                        double.TryParse(annotationDimension.Notes.Item[valueNote + 1].String, out var value2);
                        currentValue = (value1 + value2) / 2.0;
                    }
                }
                else
                {
                    double.TryParse(annotationDimension.Notes.Item[valueNote].String, out currentValue);
                }

                if ((lowerLimit <= currentValue) && (currentValue <= upperLimit))
                {
                    annotationDimension.Grouped = true;
                }
            }

            if (count == 0)
            {
                System.Windows.MessageBox.Show("There are no Dimension Annotations", "GroupDimensionRangeTutorial");
            }
            else
            {
                Document.Refresh();
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            GroupDimensionRange();
        }

        public override string Name => "Group Dimension Range";
        public override string HtmlPath => "html/group_dimension_range_tutorial.html";

    }
}
