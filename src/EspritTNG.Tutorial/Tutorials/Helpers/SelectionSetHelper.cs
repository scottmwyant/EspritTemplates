using System;
using Esprit;

namespace TutorialCSharp.Tutorials.Helpers
{
    public static class SelectionSetHelper
    {
        //! [Code snippet get selectionset]

        public static Esprit.SelectionSet GetSelectionSet(Esprit.Document document, string name)
        {
            var set = document.SelectionSets[name];
            if (set == null)
            {
                set = document.SelectionSets.Add(name);
            }

            return set;
        }

        //! [Code snippet get selectionset]

        //! [Code snippet unique selectionset]

        public static SelectionSet AddUniqueSelectionSet(ISelectionSets selectionSets, string baseName)
        {
            var suffix = 0;
            foreach (SelectionSet set in selectionSets)
            {
                var setName = set.Name;
                if (setName.Length > baseName.Length && setName.StartsWith(baseName) && setName[baseName.Length] == '_')
                {
                    if (Int32.TryParse(setName.Substring(baseName.Length + 1), out var index))
                    {
                        if (index >= suffix)
                        {
                            suffix = index + 1;
                        }
                    }
                }
            }

            var name = $"{baseName}_{suffix}";
            return selectionSets.Add(name);
        }

        //! [Code snippet unique selectionset]

        //! [Code snippet getoradd selectionset]

        public static SelectionSet GetOrAddSelectionSet(ISelectionSets selectionSets, string name)
        {
            var set = selectionSets[name] ?? selectionSets.Add(name);

            return set;
        }

        //! [Code snippet getoradd selectionset]

        //! [Code snippet default attributes set]

        public static SelectionSet DefaultAttributesSet(SelectionSet set, Configuration config)
        {
            for (var i = 1; i <= set.Count; i++)
            {
                GraphicObjectHelper.SetDefaultAttributes(set[i], config);
            }

            return set;
        }

        //! [Code snippet default attributes set]

    }
}
