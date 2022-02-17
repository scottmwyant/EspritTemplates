namespace TutorialCSharp.Tutorials
{
    //API reworked 5ae9030eaffca7795c097730d861fa5559ea1891

    class FindSyncInfoTutorial : BaseTutorial
    {
        public FindSyncInfoTutorial(Esprit.Application app): base(app)
        {
        }
        /*
        // [Code snippet]

        private void ScanSync(Esprit.Sync sync)
        {
            var nodes = sync.get_Nodes();
            for (var i = nodes.GetLowerBound(0); i<= nodes.GetUpperBound(0); i++)
            {
                var syncCodeName = (sync.SyncCodeID == 0)
                    ? "Automatic Post Processor Generated Code"
                    : sync.SyncCodeID.ToString();

                var node = (Esprit.ESPSYNCNODE)nodes.GetValue(i);
                var operation = node.pIOperation;

                var outputWindow = EspritApplication.OutputWindow;

                switch (node.SyncPlacement)
                {
                    case EspritConstants.espSyncPlacement.espSyncAfterOperation:
                        outputWindow.AppendLine($"{syncCodeName} after operation Op.Key {operation.Name}");
                        break;
                    case EspritConstants.espSyncPlacement.espSyncAfterToolChange1:
                        outputWindow.AppendLine($"{syncCodeName} after primary tool change Op.Key {operation.Name}");
                        break;
                    case EspritConstants.espSyncPlacement.espSyncAfterToolChange2:
                        outputWindow.AppendLine($"{syncCodeName} after secondary tool change Op.Key {operation.Name}");
                        break;
                    case EspritConstants.espSyncPlacement.espSyncBeforeOperation:
                        outputWindow.AppendLine($"{syncCodeName} after operation Op.Key {operation.Name}");
                        break;
                    case EspritConstants.espSyncPlacement.espSyncBeforeToolChange1:
                        outputWindow.AppendLine($"{syncCodeName} after primary tool change Op.Key {operation.Name}");
                        break;
                    case EspritConstants.espSyncPlacement.espSyncBeforeToolChange2:
                        outputWindow.AppendLine($"{syncCodeName} after secondary tool change Op.Key {operation.Name}");
                        break;
                    case EspritConstants.espSyncPlacement.espSyncDuringOperation:
                        outputWindow.AppendLine($"{syncCodeName} during operation Op.Key {operation.Name} at element {node.ElementIndex}");
                        break;
                }
                
            }
        }
        */
        public override void Execute()
        {
            EspritApplication.EventWindow.Clear();
            EspritApplication.EventWindow.Visible = true;

            for (var i = 1; i <= Document.Syncs.Count; i++)
            {
                EspritApplication.EventWindow.AddMessage(EspritConstants.espMessageType.espMessageTypeInformation, "FindSyncInfoTutorial", $"Sync {i}:");
                //ScanSync(Document.Syncs[i]);
            }
        }

        // [Code snippet]

        public override string Name => "Finding Sync Information";
        public override string HtmlPath => "html/find_syncs_info_tutorial.html";

    }
}
