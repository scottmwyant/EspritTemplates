namespace TutorialCSharp.Tutorials.Helpers
{
    static class GraphicObjectHelper
    {

        //! [Code snippet custom property]

        static public EspritProperties.CustomProperty GetOrAddCustomProperty(Esprit.IGraphicObject graphicObject, string name, EspritConstants.espPropertyType type)
        {
            EspritProperties.CustomProperty customProperty = graphicObject.CustomProperties.Item(name);
            if (customProperty != null && customProperty.VariableType != type)
            {
                graphicObject.CustomProperties.Remove(name);
                customProperty = null;
            }

            if (customProperty == null)
            {
                var properties = graphicObject.CustomProperties as EspritProperties.CustomProperties;
                customProperty = (type == EspritConstants.espPropertyType.espPropertyTypeString)
                    ? properties.Add(name, name, type, "")
                    : properties.Add(name, name, type, 0);
            }

            return customProperty;
        }

        //! [Code snippet custom property]

        //! [Code snippet default attributes obj]

        public static void SetDefaultAttributes(Esprit.GraphicObject graphicObject, Esprit.Configuration config)
        {
            switch (graphicObject.GraphicObjectType)
            {
                case EspritConstants.espGraphicObjectType.espAnnotation:
                    graphicObject.Color = config.ConfigurationNotes.Color;
                    break;
                case EspritConstants.espGraphicObjectType.espArc:
                    graphicObject.Color = config.ConfigurationArcs.Color;
                    graphicObject.LineType = config.ConfigurationArcs.LineType;
                    graphicObject.LineWeight = config.ConfigurationArcs.LineWeight;
                    break;
                case EspritConstants.espGraphicObjectType.espCircle:
                    graphicObject.Color = config.ConfigurationCircles.Color;
                    graphicObject.LineType = config.ConfigurationCircles.LineType;
                    graphicObject.LineWeight = config.ConfigurationCircles.LineWeight;
                    break;
                case EspritConstants.espGraphicObjectType.espCurve:
                    graphicObject.Color = config.ConfigurationCurves.Color;
                    graphicObject.LineType = config.ConfigurationCurves.LineType;
                    graphicObject.LineWeight = config.ConfigurationCurves.LineWeight;
                    break;
                case EspritConstants.espGraphicObjectType.espDimension:
                case EspritConstants.espGraphicObjectType.espHatch:
                case EspritConstants.espGraphicObjectType.espLeader:
                    graphicObject.Color = config.ConfigurationDimensions.Color;
                    graphicObject.LineType = config.ConfigurationDimensions.LineType;
                    graphicObject.LineWeight = config.ConfigurationDimensions.LineWeight;
                    break;
                case EspritConstants.espGraphicObjectType.espFeatureChain:
                    graphicObject.Color = config.ConfigurationFeatureChains.Color;
                    graphicObject.LineType = config.ConfigurationFeatureChains.LineType;
                    graphicObject.LineWeight = config.ConfigurationFeatureChains.LineWeight;
                    break;
                case EspritConstants.espGraphicObjectType.espFeaturePtop:
                    graphicObject.Color = config.ConfigurationFeaturePtops.Color;
                    graphicObject.LineType = config.ConfigurationFeaturePtops.LineType;
                    graphicObject.LineWeight = config.ConfigurationFeaturePtops.LineWeight;
                    break;
                case EspritConstants.espGraphicObjectType.espLine:
                    graphicObject.Color = config.ConfigurationLines.Color;
                    graphicObject.LineType = config.ConfigurationLines.LineType;
                    graphicObject.LineWeight = config.ConfigurationLines.LineWeight;
                    break;
                case EspritConstants.espGraphicObjectType.espOperation:
                    graphicObject.Color = config.ConfigurationOperations.Color;
                    graphicObject.LineType = config.ConfigurationOperations.LineType;
                    graphicObject.LineWeight = config.ConfigurationOperations.LineWeight;
                    break;
                case EspritConstants.espGraphicObjectType.espPoint:
                    graphicObject.Color = config.ConfigurationPoints.Color;
                    graphicObject.LineWeight = config.ConfigurationPoints.LineWeight;
                    break;
                case EspritConstants.espGraphicObjectType.espSegment:
                    graphicObject.Color = config.ConfigurationSegments.Color;
                    graphicObject.LineType = config.ConfigurationSegments.LineType;
                    graphicObject.LineWeight = config.ConfigurationSegments.LineWeight;
                    break;
                case EspritConstants.espGraphicObjectType.espSolidModel:
                case EspritConstants.espGraphicObjectType.espSTL_Model:
                    graphicObject.Color = config.ConfigurationPoints.Color;
                    break;
            }
        }

        //! [Code snippet default attributes obj]

    }
}
