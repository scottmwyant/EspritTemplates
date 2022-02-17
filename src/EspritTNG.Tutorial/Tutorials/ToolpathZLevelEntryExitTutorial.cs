namespace TutorialCSharp.Tutorials
{
    class ToolpathZLevelEntryExitTutorial : BaseTutorial
    {
        private bool ModifyZLevelOperation(Esprit.PartOperation operation)
        {
            if (operation == null)
                return false;

            var tech = operation.Technology as EspritTechnology.Technology;
            if (tech.TechnologyType != EspritConstants.espTechnologyType.espTechMill3DZLevelFinish)
            {
                return false;
            }

            bool onBridge = false;
            Esprit.Point point1 = null;
            Esprit.Point point2 = null;

            for (var i = operation.ToolPath.Count; i > 0 ; i--)
            {
                var toolPathItem = operation.ToolPath.ItemCopy(i);
                if(toolPathItem.Type == EspritConstants.espToolPathItemType.tpiSEG2 || toolPathItem.Type == EspritConstants.espToolPathItemType.tpiSEG3)
                {
                    var segment = toolPathItem.data as EspritGeometryBase.IComSegment;
                    if (segment.StartPoint.Z != segment.EndPoint.Z)
                    {
                        if (onBridge)
                        {
                            point1 = Document.GetPoint(segment.StartPoint.X, segment.StartPoint.Y, segment.StartPoint.Z);
                        }
                        else
                        {
                            point2 = Document.GetPoint(segment.EndPoint.X, segment.EndPoint.Y, segment.EndPoint.Z);
                            onBridge = true;
                        }
                        operation.ToolPath.Remove(i);
                    }
                    else if (onBridge && point1 != null && point2 != null)
                    {
                        segment.StartPoint.SetXyz(point1.X, point1.Y, point1.Z);
                        segment.EndPoint.SetXyz(point2.X, point2.Y, point2.Z);
                        toolPathItem.data = segment;
                        operation.ToolPath.Insert(i + 1, toolPathItem);

                        segment.StartPoint.SetXyz(point2.X, point2.Y, point1.Z);
                        segment.EndPoint.SetXyz(point2.X, point2.Y, point1.Z);
                        toolPathItem.data = segment;
                        operation.ToolPath.Insert(i + 2, toolPathItem);

                        onBridge = false;
                    }
                }
            }

            return true;
        }

        private void ModifyZLevelOperations()
        {
            var modifiedGroupedOp = false;
            if (Document.Group.Count > 0)
            {
                for (var i = 1; i <= Document.Group.Count; i++)
                {
                    var obj = Document.Group[i] as Esprit.IGraphicObject;
                    if (obj.GraphicObjectType == EspritConstants.espGraphicObjectType.espOperation && ModifyZLevelOperation(obj as Esprit.PartOperation))
                    {
                        modifiedGroupedOp = true;
                    }
                }
            }

            if (!modifiedGroupedOp)
            {
                Esprit.PartOperation operation = null;

                try
                {
                    operation = Document.GetAnyElement("Select ZLevel Operation to Modify", EspritConstants.espGraphicObjectType.espOperation) as Esprit.PartOperation;
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    // Element was not set
                    return;
                }

                ModifyZLevelOperation(operation);
            }

            Document.Refresh();
        }

        public ToolpathZLevelEntryExitTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            ModifyZLevelOperations();
        }

        public override string Name => "Modifying Toolpath: ZLevel Entry and Exit";
        public override string HtmlPath => "html/toolpath_zlevel_entry_exit_tutorial.html";

    }
}
