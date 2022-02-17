using System;
using System.IO;
using System.Text;
using System.Windows;

namespace TutorialCSharp.Tutorials
{
    class ToolAssemblyTutorial : BaseTutorial
    {
        public ToolAssemblyTutorial(Esprit.Application app): base(app)
        {
        }

        // [Code snippet Display methods]

        private void RetrieveAndDisplayToolAssemblies(Esprit.ToolAssemblies ta)
        {
            //Retrieve information from each TA in the ToolAssemblies container and display them
            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ToolAssembly", $"There are {ta.Count} Tool Assemblies in the document");

            var i = 0;
            foreach(Esprit.ComToolAssembly toolAssembly in ta)
            {
                i++;
                DisplayTAInformations(toolAssembly, i);
            }
        }

        private void DisplayTAInformations(Esprit.ComToolAssembly ta, int id  = -1)
        {
            //Display the TA information (with an optional ID to identify it)
            if (ta == null)
            {
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeError, "ToolAssembly", "No Tool Assembly provided.");
                return;
            }

            var message = new StringBuilder();
            message.AppendLine(
                (id == -1)
                ? "This ToolAssembly"
                : "ToolAssembly #" + id);

            var mountedToolCount = ta.AllMountedTools.Count;
            var mountedAdaptiveItemCount = ta.AllMountedAdaptiveItems.Count;

            switch (mountedAdaptiveItemCount)
            {
                case 0:
                    message.Append(" contains no Adaptive Item...");
                    break;
                case 1:
                    message.Append(" contains 1 Adaptive Item : ");
                    break;
                default:
                    message.Append(" contains " + mountedAdaptiveItemCount + " Adaptive Items : ");
                    break;
            }

            foreach (Esprit.MountedAdaptiveItem item in ta.AllMountedAdaptiveItems)
            {
                message.AppendLine(item.Name);
            }

            message.AppendLine("This ToolAssembly ");

            switch (mountedToolCount)
            {
                case 0:
                    message.Append(" contains no Tool...");
                    break;
                case 1:
                    message.Append(" contains 1 Tool : ");
                    break;
                default:
                    message.Append(" contains " + mountedToolCount + " Tools : ");
                    break;
            }

            foreach (Esprit.MountedTool mountedTool in ta.AllMountedTools)
            {
                message.AppendLine(mountedTool.Tool.Caption + " , of type : " + mountedTool.Tool.Name);
            }

            EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ToolAssembly", message.ToString());
        }

        // [Code snippet Display methods]

        // [Code snippet Manipulation]

        private Esprit.MountedAdaptiveItem MountAIOnFirstAvailableAdapter(string pathToMount, Esprit.MountedAdaptiveItem target)
        {
            //Mount on the provided AdaptiveItem. If no adapter is available, go back in hierarchy to find first available adapter.
            Esprit.MountedAdaptiveItem mountAIOnFirstAvailableAdapter = null;
            bool adaptiveItemSuccessfullyMounted = false;
            bool noMoreAdaptiveItemAdapterFound = false;
            var adaptiveItem = target;

            //Loop to find first available adapter
            while (!(adaptiveItemSuccessfullyMounted || noMoreAdaptiveItemAdapterFound))
            {
                foreach(Esprit.AdaptiveItemAdapter adapter in adaptiveItem.AdaptiveItemAdapters)
                {
                    if (adapter.IsAvailable)
                    {
                        adaptiveItemSuccessfullyMounted = true;
                        mountAIOnFirstAvailableAdapter = adapter.MountAdaptiveItem(pathToMount);
                        break;
                    }
                }

                adaptiveItem = adaptiveItem.ParentAdaptiveItem;
                if (adaptiveItem == null)
                {
                    noMoreAdaptiveItemAdapterFound = true;
                }
            }

            //Reporting
            if (adaptiveItemSuccessfullyMounted)
            {
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ToolAssembly", "The AdaptiveItem was successfully mounted.");
            }
            else
            {
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeError, "ToolAssembly", "The AdaptiveItem could not be mounted on the provided AdaptiveItem.");
            }

            return mountAIOnFirstAvailableAdapter;
        }

        private Esprit.MountedTool MountToolOnFirstAvailableAdapter(EspritTechnology.ITechnology tool, Esprit.MountedAdaptiveItem target)
        {
            //Mount Tool on the provided AdaptiveItem. If no adapter is available, go back in hierarchy to find first available adapter.
            Esprit.MountedTool mountToolOnFirstAvailableAdapter = null;
            var toolSuccessfullyMounted = false;
            var noMoreToolAdapterFound = false;
            var adaptiveItem = target;

            //Loop to find first available adapter
            while(!(toolSuccessfullyMounted || noMoreToolAdapterFound))
            {
                foreach (Esprit.ToolAdapter adapter in adaptiveItem.ToolAdapters)
                {
                    if (adapter.IsAvailable)
                    {
                        toolSuccessfullyMounted = true;
                        mountToolOnFirstAvailableAdapter = adapter.MountTool(tool);
                        break;
                    }
                }

                adaptiveItem = adaptiveItem.ParentAdaptiveItem;
                if (adaptiveItem == null)
                {
                    noMoreToolAdapterFound = true;
                }
            }

            //Reporting
            if(toolSuccessfullyMounted)
            {
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ToolAssembly", "The tool was successfully mounted.");
            }
            else
            {
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeError, "ToolAssembly", "The tool could not be mounted on the provided AdaptiveItem.");
            }

            return mountToolOnFirstAvailableAdapter;
        }

        private void SwapToolAssembliesStations(Esprit.ComToolAssembly a, Esprit.ComToolAssembly b)
        {
            var stationA = a.Mounting;
            var stationB = b.Mounting;

            a.Unmount();
            b.Unmount();

            if (stationB != null)
            {
                a.Mount(stationB);
            }
            if (stationA != null)
            {
                b.Mount(stationA);
            }
        }

        // [Code snippet Manipulation]

        //For this tutorial project ToolAssemblyAPI.esprit must be opened

        // [Code snippet Main]

        public void ToolAssemblyCreationAndManipulation()
        {
            var toolAssemblies = Document.ToolAssemblies;

            try
            {
                EspritTechnology.TechnologyUtility techUtility = Document.TechnologyUtility;
                var machineSetup = Document.InitialMachineSetup;

                var projectDirectoryPath = Path.GetDirectoryName(Document.DocumentProperties.Path);

                //STEP 1 - Mill Tool Creation
                //Create a new ToolAssembly on the specified Station with no specified Adaptive Item
                var adaptiveItemfilePath = Path.Combine(projectDirectoryPath, "HSK Holder.gdml");
                var targetStation = machineSetup.MachineDefinition.Turrets[2].Stations[4];
                var newMillToolAssembly = toolAssemblies.Add(adaptiveItemfilePath, targetStation);
                var mountedAdaptiveItem = newMillToolAssembly.RootAdaptiveItem;

                //On the Same Assembly, Mount 2 successive adaptive items (Collet and Nut)
                adaptiveItemfilePath = Path.Combine(projectDirectoryPath, "Collet.gdml");
                mountedAdaptiveItem = MountAIOnFirstAvailableAdapter(adaptiveItemfilePath, mountedAdaptiveItem);

                adaptiveItemfilePath = Path.Combine(projectDirectoryPath, "Nut.gdml");
                mountedAdaptiveItem = MountAIOnFirstAvailableAdapter(adaptiveItemfilePath, mountedAdaptiveItem);

                //On this Tool Assembly, mount a tool from technology
                var technologyfilePath = Path.Combine(projectDirectoryPath, "EM10_ToolTechnology.etl");
                var toolTechnology = techUtility.CreateTechnology(EspritConstants.espTechnologyType.espToolMillEndMill, Document.SystemUnit);
                toolTechnology.Open(technologyfilePath);
                MountToolOnFirstAvailableAdapter(toolTechnology, mountedAdaptiveItem);

                //STEP 2 - Boring Bar Creation
                //Repeat previous steps with different Adapters and tool
                adaptiveItemfilePath = Path.Combine(projectDirectoryPath, "BoringBarHolder.gdml");
                targetStation = machineSetup.MachineDefinition.Turrets[2].Stations[8];
                var newBoringBarToolAssembly = toolAssemblies.Add(adaptiveItemfilePath, targetStation);
                mountedAdaptiveItem = newBoringBarToolAssembly.RootAdaptiveItem;

                adaptiveItemfilePath = Path.Combine(projectDirectoryPath, "BoringBarShank.gdml");
                mountedAdaptiveItem = MountAIOnFirstAvailableAdapter(adaptiveItemfilePath, mountedAdaptiveItem);

                adaptiveItemfilePath = Path.Combine(projectDirectoryPath, "BoringBarInsertHolder.gdml");
                mountedAdaptiveItem = MountAIOnFirstAvailableAdapter(adaptiveItemfilePath, mountedAdaptiveItem);

                technologyfilePath = Path.Combine(projectDirectoryPath, "BoringBar_ToolTechnology.etl");
                toolTechnology = techUtility.CreateTechnology(EspritConstants.espTechnologyType.espToolLatheTurning, Document.SystemUnit);
                toolTechnology.Open(technologyfilePath);
                MountToolOnFirstAvailableAdapter(toolTechnology, mountedAdaptiveItem);

                //STEP 3 - OD Rough Creation
                //Repeat previous steps with different Adapter and tool
                adaptiveItemfilePath = Path.Combine(projectDirectoryPath, "ODTurningHolder.gdml");
                targetStation = machineSetup.MachineDefinition.Turrets[2].Stations[6];
                var newTurningToolAssembly = toolAssemblies.Add(adaptiveItemfilePath, targetStation);
                mountedAdaptiveItem = newTurningToolAssembly.RootAdaptiveItem;

                technologyfilePath = Path.Combine(projectDirectoryPath, "ODRough_ToolTechnology.etl");
                toolTechnology = techUtility.CreateTechnology(EspritConstants.espTechnologyType.espToolLatheTurning, Document.SystemUnit);
                toolTechnology.Open(technologyfilePath);
                MountToolOnFirstAvailableAdapter(toolTechnology, mountedAdaptiveItem);

                //STEP 4 - Manipulations on Tool Assemblies
                //Swap 2 Tool Assemblies
                SwapToolAssembliesStations(newBoringBarToolAssembly, newTurningToolAssembly);

                //Move ToolASsembly
                newMillToolAssembly.Unmount();
                newMillToolAssembly.Mount(machineSetup.MachineDefinition.Turrets[1].Stations[1]);

                //STEP 5 - Retrieve Assemblies Information and Display them on Event Window
                EspritApplication.EventWindow.Filter("ToolAssembly");
                EspritApplication.EventWindow.Clear();

                //Retrieve information from each TA in the ToolAssemblies container
                RetrieveAndDisplayToolAssemblies(toolAssemblies);
            }
            catch(Exception)
            {
                MessageBox.Show("Load right project for demostrating this tutorial", "ToolAssemblyTutorial", MessageBoxButton.OK);
            }

            //Search for a specific assembly by Tool Name
            var toolName = "EM10";
            var ta = toolAssemblies.FindWithMountedToolName(toolName);

            if (ta != null)
            {
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "ToolAssembly", "The ToolAssembly containing the Tool '" + toolName + "' was properly found.");
            }

            DisplayTAInformations(ta);
        }

        // [Code snippet Main]

        public override void Execute()
        {
            ToolAssemblyCreationAndManipulation();
        }

        public override string Name => "Create and manipulate tool assembly";

        public override string HtmlPath => "html/tool_assembly_tutorial.html";

    }
}
