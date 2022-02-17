namespace TutorialCSharp.Tutorials
{
    class SmashingToolpathToGeometryTutorial : BaseTutorial
    {
        //! [Code snippet smash all]

        private void SmashOperations()
        {
            if (Document.Group.Count == 0)
            {
                Esprit.PartOperation operation = null;

                try
                {
                    operation = Document.GetAnyElement("Select Operation to Smash", EspritConstants.espGraphicObjectType.espOperation) as Esprit.PartOperation;
                }
                catch(System.Runtime.InteropServices.COMException)
                {
                    // Operation was not set
                    return;
                }

                SmashOperation(operation, EspritApplication);
            }
            else
            {
                for (var i = 1; i <= Document.Group.Count; i++)
                {
                    var obj = Document.Group[i] as Esprit.IGraphicObject;
                    if (obj.GraphicObjectType == EspritConstants.espGraphicObjectType.espOperation)
                    {
                        var operation = obj as Esprit.PartOperation;
                        SmashOperation(operation, EspritApplication);
                    }
                }
            }

            Document.Refresh();
        }

        //! [Code snippet smash all]

        //! [Code snippet smash]

        public static void SmashOperation(Esprit.PartOperation operation, Esprit.Application app)
        {
            if (operation == null)
            {
                return;
            }
            for (var i = 1; i <= operation.ToolPath.Count; i++)
            {
                var toolPathItem = operation.ToolPath.ItemCopy(i);
                switch(toolPathItem.Type)
                {
                    case EspritConstants.espToolPathItemType.tpiEND_POINT:
                    case EspritConstants.espToolPathItemType.tpiSTART_POINT:
                        var point = toolPathItem.data as EspritGeometryBase.IComPoint;
                        app.Document.Points.Add(point.X, point.Y, point.Z);
                        break;

                    case EspritConstants.espToolPathItemType.tpiSEG2:
                    case EspritConstants.espToolPathItemType.tpiSEG3:
                        var segment = toolPathItem.data as EspritGeometryBase.IComSegment;
                        app.Document.Segments.Add(
                            app.Document.GetPoint(segment.StartPoint.X, segment.StartPoint.Y, segment.StartPoint.Z),
                            app.Document.GetPoint(segment.EndPoint.X, segment.EndPoint.Y, segment.EndPoint.Z)
                         );
                        break;

                    case EspritConstants.espToolPathItemType.tpiARC2:
                    case EspritConstants.espToolPathItemType.tpiARC3:
                        var arc = toolPathItem.data as EspritGeometryBase.IComArc;
                        var newArc = app.Document.Arcs.Add(
                            app.Document.GetPoint(arc.CenterPoint.X, arc.CenterPoint.Y, arc.CenterPoint.Z),
                            arc.Radius, arc.StartAngle, arc.EndAngle);

                        newArc.Ux = arc.U.X;
                        newArc.Uy = arc.U.Y;
                        newArc.Uz = arc.U.Z;
                        newArc.Vx = arc.V.X;
                        newArc.Vy = arc.V.Y;
                        newArc.Vz = arc.V.Z;
                        break;
                }
            }
        }

        //! [Code snippet smash]

        public SmashingToolpathToGeometryTutorial(Esprit.Application app): base(app)
        {
        }

        public override void Execute()
        {
            SmashOperations();
        }

        public override string Name => "Smashing Toolpath to Geometry";
        public override string HtmlPath => "html/smashing_toolpath_to_geometry_tutorial.html";

    }
}
