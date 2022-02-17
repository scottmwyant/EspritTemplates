using System.Collections.ObjectModel;
using TutorialCSharp.Tutorials;

namespace TutorialCSharp.ViewModels
{
    class PaneViewModel : Notifier
    {
        private TutorialModel _selectedItem;

        private readonly Esprit.Application _espritApplication;

        public PaneViewModel(Esprit.Application espritApp)
        {
            _espritApplication = espritApp;
            Tutorials = new ObservableCollection<TutorialModel>();
        }
        
        public ObservableCollection<TutorialModel> Tutorials { get; set; }
        public TutorialModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
            }
        }

        public void Initialize()
        {
            var geometry = new TutorialModel("Geometry", null);
            var tutorialSquareCorners = new TutorialModel(new SquareCornersTutorial(_espritApplication));
            var tutorialSimpleSquare = new TutorialModel(new SimpleSquareTutorial(_espritApplication));
            var tutorialSimpleSquareAnywhere = new TutorialModel(new SimpleSquareAnywhereTutorial(_espritApplication));
            var tutorialCenterLineCircles = new TutorialModel(new CenterLineCirclesTutorial(_espritApplication));
            geometry.Tutorials.Add(tutorialSquareCorners);
            geometry.Tutorials.Add(tutorialSimpleSquare);
            geometry.Tutorials.Add(tutorialSimpleSquareAnywhere);
            geometry.Tutorials.Add(tutorialCenterLineCircles);
            Tutorials.Add(geometry);

            var feature = new TutorialModel("Feature", null);
            var tutorialSquareFeaturePtop = new TutorialModel(new SquareFeaturePtopTutorial(_espritApplication));
            var tutorialSquareFeatureChain = new TutorialModel(new SquareFeatureChainTutorial(_espritApplication));
            var tutorialRectFeatureChain = new TutorialModel(new RectFeatureChainTutorial(_espritApplication));
            var tutorialFeatureChainCircles = new TutorialModel(new FeatureChainCirclesTutorial(_espritApplication));
            var tutorialCustomFeatureHoles = new TutorialModel(new CreateCustomFeatureHolesTutorial(_espritApplication));
            var tutorialScanFeatureHoles = new TutorialModel(new ScanFeatureHolesTutorial(_espritApplication));
            feature.Tutorials.Add(tutorialSquareFeaturePtop);
            feature.Tutorials.Add(tutorialSquareFeatureChain);
            feature.Tutorials.Add(tutorialRectFeatureChain);
            feature.Tutorials.Add(tutorialFeatureChainCircles);
            feature.Tutorials.Add(tutorialCustomFeatureHoles);
            feature.Tutorials.Add(tutorialScanFeatureHoles);
            Tutorials.Add(feature);

            var layers = new TutorialModel("Layers", null);
            var tutorialSwapLayers = new TutorialModel(new SwapLayersTutorial(_espritApplication));
            var tutorialCenterLineVisibleCircles = new TutorialModel(new CenterLineVisibleCirclesTutorial(_espritApplication));
            var tutorialFeatureChainVisibleCircles = new TutorialModel(new FeatureChainVisibleCirclesTutorial(_espritApplication));
            var tutorialFeatureChainVisibleCirclesArcs = new TutorialModel(new FeatureChainVisibleCirclesArcsTutorial(_espritApplication));
            layers.Tutorials.Add(tutorialSwapLayers);
            layers.Tutorials.Add(tutorialCenterLineVisibleCircles);
            layers.Tutorials.Add(tutorialFeatureChainVisibleCircles);
            layers.Tutorials.Add(tutorialFeatureChainVisibleCirclesArcs);
            Tutorials.Add(layers);

            var windows = new TutorialModel("Windows", null);
            var tutorialFitAllWindows = new TutorialModel(new FitAllWindowsTutorial(_espritApplication));
            windows.Tutorials.Add(tutorialFitAllWindows);
            Tutorials.Add(windows);

            var graphicsGeneral = new TutorialModel("Graphics (General)", null);
            var tutorialMoveP0CircleCenter = new TutorialModel(new MoveP0CircleCenterTutorial(_espritApplication));
            var tutorialMoveP0AnyElementCenter = new TutorialModel(new MoveP0AnyElementCenterTutorial(_espritApplication));
            var tutorialMoveP0PointArcCircleCenter = new TutorialModel(new MoveP0PointArcCircleCenterTutorial(_espritApplication));
            var tutorialScanGraphicsCollection = new TutorialModel(new ScanGraphicsCollectionTutorial(_espritApplication));
            graphicsGeneral.Tutorials.Add(tutorialMoveP0CircleCenter);
            graphicsGeneral.Tutorials.Add(tutorialMoveP0AnyElementCenter);
            graphicsGeneral.Tutorials.Add(tutorialMoveP0PointArcCircleCenter);
            graphicsGeneral.Tutorials.Add(tutorialScanGraphicsCollection);
            Tutorials.Add(graphicsGeneral);

            var tools = new TutorialModel("Tools", null);
            var tutorialCreateTools = new TutorialModel(new CreateToolsTutorial(_espritApplication));
            var tutorialScanTools = new TutorialModel(new ScanToolsTutorial(_espritApplication));
            var tutorialScanToolParameters = new TutorialModel(new ScanToolParametersTutorial(_espritApplication));
            var tutorialWriteToolParametersToFile = new TutorialModel(new WriteToolParametersToFileTutorial(_espritApplication));
            var tutorialReadToolParametersFromFile = new TutorialModel(new ReadToolParametersFromFileTutorial(_espritApplication));
            var tutorialToolAssembly = new TutorialModel(new ToolAssemblyTutorial(_espritApplication));
            tools.Tutorials.Add(tutorialCreateTools);
            tools.Tutorials.Add(tutorialScanTools);
            tools.Tutorials.Add(tutorialScanToolParameters);
            tools.Tutorials.Add(tutorialToolAssembly);
            tools.Tutorials.Add(tutorialWriteToolParametersToFile);
            tools.Tutorials.Add(tutorialReadToolParametersFromFile);
            Tutorials.Add(tools);

            var oprations = new TutorialModel("Operations (General)", null);
            var tutorialScanOperations = new TutorialModel(new ScanOperationsTutorial(_espritApplication));
            var tutorialCalcOperationCycleTimes = new TutorialModel(new CalcOperationCycleTimesTutorial(_espritApplication));
            var tutorialReorderingOperations = new TutorialModel(new ReorderingOperationsTutorial(_espritApplication));
            var tutorialCustomCycles = new TutorialModel(new CustomCyclesTutorial(_espritApplication));
            oprations.Tutorials.Add(tutorialScanOperations);
            oprations.Tutorials.Add(tutorialCalcOperationCycleTimes);
            oprations.Tutorials.Add(tutorialReorderingOperations);
            oprations.Tutorials.Add(tutorialCustomCycles);
            Tutorials.Add(oprations);

            var autoRunProcedures = new TutorialModel("Auto Run Procedures", null);
            var tutorialAutoRunProcedures = new TutorialModel(new AutoRunProceduresTutorial(_espritApplication));
            var tutorialTechnologiesAndOperation = new TutorialModel(new TechnologiesAndOperationsTutorial(_espritApplication));
            autoRunProcedures.Tutorials.Add(tutorialAutoRunProcedures);
            autoRunProcedures.Tutorials.Add(tutorialTechnologiesAndOperation);
            Tutorials.Add(autoRunProcedures);

            var millingOperations = new TutorialModel("Milling Operations", null);
            var tutorialUserForms = new TutorialModel(new UserFormsTutorial(_espritApplication));
            var tutorialUserFormsMore = new TutorialModel(new UserFormsMoreTutorial(_espritApplication));
            var tutorialUserFormsAdvanced = new TutorialModel(new UserFormsAdvancedTutorial(_espritApplication));
            var tutorialTraditionalMilling = new TutorialModel(new TraditionalMillingTutorial(_espritApplication));
            var tutorialRotaryMilling = new TutorialModel(new RotaryMillingTutorial(_espritApplication));

            millingOperations.Tutorials.Add(tutorialUserForms);
            millingOperations.Tutorials.Add(tutorialUserFormsMore);
            millingOperations.Tutorials.Add(tutorialUserFormsAdvanced);
            millingOperations.Tutorials.Add(tutorialTraditionalMilling);
            millingOperations.Tutorials.Add(tutorialRotaryMilling);
            Tutorials.Add(millingOperations);

            var turningOperations = new TutorialModel("Turning Operations", null);
            var tutorialBalancedTurning = new TutorialModel(new BalancedTurningTutorial(_espritApplication));
            turningOperations.Tutorials.Add(tutorialBalancedTurning);
            Tutorials.Add(turningOperations);

            var group = new TutorialModel("Group", null);
            var tutorialGroup = new TutorialModel(new GroupTutorial(_espritApplication));
            var tutorialFeatureChainGroupedCirclesAndArc = new TutorialModel(new FeatureChainGroupedCirclesAndArcsTutorial(_espritApplication));
            var tutorialGroupSolidEntities = new TutorialModel(new GroupSolidEntitiesTutorial(_espritApplication));
            var tutorialNamingRecallingGroups = new TutorialModel(new NamingRecallingGroupsTutorial(_espritApplication));
            var tutorialGroupManagerDialog = new TutorialModel(new GroupManagerDialogTutorial(_espritApplication));
            group.Tutorials.Add(tutorialGroup);
            group.Tutorials.Add(tutorialFeatureChainGroupedCirclesAndArc);
            group.Tutorials.Add(tutorialGroupSolidEntities);
            group.Tutorials.Add(tutorialNamingRecallingGroups);
            group.Tutorials.Add(tutorialGroupManagerDialog);
            Tutorials.Add(group);

            var selectionSetsAndManipulations = new TutorialModel("SelectionSets and Manipulations", null);
            var tutorialXYSymmetry = new TutorialModel(new XYSymmetryTutorial(_espritApplication));
            var tutorialGridTranslation = new TutorialModel(new GridTranslationTutorial(_espritApplication));
            selectionSetsAndManipulations.Tutorials.Add(tutorialXYSymmetry);
            selectionSetsAndManipulations.Tutorials.Add(tutorialGridTranslation);
            Tutorials.Add(selectionSetsAndManipulations);

            var annotations = new TutorialModel("Annotations", null);
            var tutorialScanAnnotations = new TutorialModel(new ScanAnnotationsTutorial(_espritApplication));
            var tutorialScanDimensionAndNotes = new TutorialModel(new ScanDimensionAndNotesTutorial(_espritApplication));
            var tutorialGroupDimensionRange = new TutorialModel(new GroupDimensionRangeTutorial(_espritApplication));
            var tutorialCreateCenterLinesAsLeaders = new TutorialModel(new CreateCenterLinesAsLeadersTutorial(_espritApplication));
            var tutorialBreakLeader = new TutorialModel(new BreakLeaderTutorial(_espritApplication));
            var tutorialCreateNotesBlock = new TutorialModel(new CreateNotesBlockTutorial(_espritApplication));
            var tutorialCreateHoleProcessingInfo = new TutorialModel(new CreateHoleProcessingInfoTutorial(_espritApplication));
            var tutorialModifyNoteText = new TutorialModel(new ModifyNoteTextTutorial(_espritApplication));
            annotations.Tutorials.Add(tutorialScanAnnotations);
            annotations.Tutorials.Add(tutorialScanDimensionAndNotes);
            annotations.Tutorials.Add(tutorialGroupDimensionRange);
            annotations.Tutorials.Add(tutorialCreateCenterLinesAsLeaders);
            annotations.Tutorials.Add(tutorialBreakLeader);
            annotations.Tutorials.Add(tutorialCreateNotesBlock);
            annotations.Tutorials.Add(tutorialCreateHoleProcessingInfo);
            annotations.Tutorials.Add(tutorialModifyNoteText);
            Tutorials.Add(annotations);

            var advancedTopics = new TutorialModel("Advanced Topics", null);
            var tutorialFunctions = new TutorialModel(new FunctionsTutorial(_espritApplication));
            var tutorialFunctionsAndSelectionSets = new TutorialModel(new FunctionsAndSelectionSetsTutorial(_espritApplication));
            var tutorialSaveValuesToRegistry = new TutorialModel(new SaveValuesToRegistryTutorial(_espritApplication));
            var tutorialRegistryValuesWithUserForms = new TutorialModel(new RegistryValuesWithUserFormsTutorial(_espritApplication));
            var tutorialUndo = new TutorialModel(new UndoTutorial(_espritApplication));
            advancedTopics.Tutorials.Add(tutorialFunctions);
            advancedTopics.Tutorials.Add(tutorialFunctionsAndSelectionSets);
            advancedTopics.Tutorials.Add(tutorialSaveValuesToRegistry);
            advancedTopics.Tutorials.Add(tutorialRegistryValuesWithUserForms);
            advancedTopics.Tutorials.Add(tutorialUndo);
            Tutorials.Add(advancedTopics);

            var configuration = new TutorialModel("Configuration", null);
            var tutorialConfigurationObject = new TutorialModel(new ConfigurationObjectTutorial(_espritApplication));
            var tutorialGridBackgroundLanguage = new TutorialModel(new GridBackgroundTutorial(_espritApplication));
            var tutorialFilesAndFoldersLocation = new TutorialModel(new FilesAndFoldersLocationTutorial(_espritApplication));
            var tutorialFileImportExport = new TutorialModel(new FileImportExportTutorial(_espritApplication));
            var tutorialDimensioningParameters = new TutorialModel(new DimensioningParametersTutorial(_espritApplication));
            var tutorialConfigurationManager = new TutorialModel(new ConfigurationManagerTutorial(_espritApplication));
            configuration.Tutorials.Add(tutorialConfigurationObject);
            configuration.Tutorials.Add(tutorialGridBackgroundLanguage);
            configuration.Tutorials.Add(tutorialFilesAndFoldersLocation);
            configuration.Tutorials.Add(tutorialFileImportExport);
            configuration.Tutorials.Add(tutorialDimensioningParameters);
            configuration.Tutorials.Add(tutorialConfigurationManager);
            Tutorials.Add(configuration);

            var planes = new TutorialModel("Planes", null);
            var tutorialScanPlanes = new TutorialModel(new ScanPlanesTutorial(_espritApplication));
            var tutorialCreatePlanes = new TutorialModel(new CreatePlanesTutorial(_espritApplication));
            var tutorialPlaneManipulationMethods = new TutorialModel(new PlaneManipulationMethodsTutorial(_espritApplication));
            var tutorialPlaneManipulations = new TutorialModel(new PlaneManipulationsTutorial(_espritApplication));
            var tutorialCustomIsoViewDialog = new TutorialModel(new CustomIsoViewDialogTutorial(_espritApplication));
            planes.Tutorials.Add(tutorialCreatePlanes);
            planes.Tutorials.Add(tutorialScanPlanes);
            planes.Tutorials.Add(tutorialPlaneManipulationMethods);
            planes.Tutorials.Add(tutorialPlaneManipulations);
            planes.Tutorials.Add(tutorialCustomIsoViewDialog);
            Tutorials.Add(planes);

            var featuresAdvanced = new TutorialModel("Features (Advanced)", null);
            var tutorialExtractFeatureInfo = new TutorialModel(new ExtractFeatureInfoTutorial(_espritApplication));
            var tutorialInsertPointsIntoFeaturePtop = new TutorialModel(new InsertPointsIntoFeaturePtopTutorial(_espritApplication));
            var tutorialOptimizeFeaturePtopObjects = new TutorialModel(new OptimizeFeaturePtopObjectsTutorial(_espritApplication));
            var tutorialFeatureChainIntoSpiral = new TutorialModel(new FeatureChainIntoSpiralTutorial(_espritApplication));
            var tutorialZigzagDownFeatureChain = new TutorialModel(new ZigzagDownFeatureChainTutorial(_espritApplication));
            featuresAdvanced.Tutorials.Add(tutorialExtractFeatureInfo);
            featuresAdvanced.Tutorials.Add(tutorialInsertPointsIntoFeaturePtop);
            featuresAdvanced.Tutorials.Add(tutorialOptimizeFeaturePtopObjects);
            featuresAdvanced.Tutorials.Add(tutorialFeatureChainIntoSpiral);
            featuresAdvanced.Tutorials.Add(tutorialZigzagDownFeatureChain);
            Tutorials.Add(featuresAdvanced);

            var featureRecognition = new TutorialModel("Feature Recognition", null);
            var tutorialFeatureRecognitionIntro = new TutorialModel(new FeatureRecognitionIntroTutorial(_espritApplication));
            var tutorialFeatureRecognitionAccessingResults = new TutorialModel(new FeatureRecognitionAccessingResultsTutorial(_espritApplication));
            var tutorialAutoRecognizeFeatureStartPoints = new TutorialModel(new AutoRecognizeFeatureStartPointsTutorial(_espritApplication));
            var tutorialFeatureRecognitionPartProfiles = new TutorialModel(new FeatureRecognitionPartProfilesTutorial(_espritApplication));
            var tutorialFeatureRecognitionFaceProfileFeatures = new TutorialModel(new FeatureRecognitionFaceProfileFeaturesTutorial(_espritApplication));
            var tutorialMachiningFaceProfiles = new TutorialModel(new MachiningFaceProfilesTutorial(_espritApplication));
            var tutorialFeatureRecognitionPocketFeatures = new TutorialModel(new FeatureRecognitionPocketFeaturesTutorial(_espritApplication));
            var tutorialFeatureHolesRecognition = new TutorialModel(new FeatureHolesRecognitionTutorial(_espritApplication));
            featureRecognition.Tutorials.Add(tutorialFeatureRecognitionIntro);
            featureRecognition.Tutorials.Add(tutorialFeatureRecognitionAccessingResults);
            featureRecognition.Tutorials.Add(tutorialAutoRecognizeFeatureStartPoints);
            featureRecognition.Tutorials.Add(tutorialFeatureRecognitionPartProfiles);
            featureRecognition.Tutorials.Add(tutorialFeatureRecognitionFaceProfileFeatures);
            featureRecognition.Tutorials.Add(tutorialMachiningFaceProfiles);
            featureRecognition.Tutorials.Add(tutorialFeatureRecognitionPocketFeatures);
            featureRecognition.Tutorials.Add(tutorialFeatureHolesRecognition);
            Tutorials.Add(featureRecognition);

            var featureSets = new TutorialModel("Feature Sets", null);
            var tutorialExaminingFeatureSets = new TutorialModel(new ExaminingFeatureSetsTutorial(_espritApplication));
            featureSets.Tutorials.Add(tutorialExaminingFeatureSets);
            Tutorials.Add(featureSets);

            var classModules = new TutorialModel("Class Modules", null);
            var tutorialClassModules = new TutorialModel(new ClassModulesTutorial(_espritApplication));
            var tutorialBoltHolePattern = new TutorialModel(new BoltHolePatternTutorial(_espritApplication));
            var tutorialBoltHolePatternExpand = new TutorialModel(new BoltHolePatternExpandTutorial(_espritApplication));
            var tutorialRespondToEspritEvents = new TutorialModel(new RespondToEspritEventsTutorial(_espritApplication));
            var tutorialDocumentEvents = new TutorialModel(new DocumentEventsTutorial(_espritApplication));
            var tutorialAutoSave = new TutorialModel(new AutoSaveTutorial(_espritApplication));
            var tutorialPartsProgramming = new TutorialModel(new PartsProgrammingTutorial(_espritApplication));
            classModules.Tutorials.Add(tutorialClassModules);
            classModules.Tutorials.Add(tutorialBoltHolePattern);
            classModules.Tutorials.Add(tutorialBoltHolePatternExpand);
            classModules.Tutorials.Add(tutorialRespondToEspritEvents);
            classModules.Tutorials.Add(tutorialDocumentEvents);
            classModules.Tutorials.Add(tutorialAutoSave);
            classModules.Tutorials.Add(tutorialPartsProgramming);
            Tutorials.Add(classModules);

            var customProperties = new TutorialModel("Custom Properties", null);
            var tutorialCustomProperties = new TutorialModel(new CustomPropertiesTutorial(_espritApplication));
            var tutorialGraphicsEvents = new TutorialModel(new GraphicsEventsTutorial(_espritApplication));
            var tutorialRespondingToCustomPropertyChanges = new TutorialModel(new RespondingToCustomPropertyChangesTutorial(_espritApplication));
            customProperties.Tutorials.Add(tutorialCustomProperties);
            customProperties.Tutorials.Add(tutorialGraphicsEvents);
            customProperties.Tutorials.Add(tutorialRespondingToCustomPropertyChanges);
            Tutorials.Add(customProperties);

            var toolpath = new TutorialModel("Toolpath", null);
            var tutorialExaminingToolpath = new TutorialModel(new ExaminingToolpathTutorial(_espritApplication));
            var tutorialSmashingToolpathToGeometry = new TutorialModel(new SmashingToolpathToGeometryTutorial(_espritApplication));
            toolpath.Tutorials.Add(tutorialExaminingToolpath);
            toolpath.Tutorials.Add(tutorialSmashingToolpathToGeometry);
            Tutorials.Add(toolpath);

            var simulation = new TutorialModel("Simulation", null);
            var tutorialSimulation = new TutorialModel(new SimulationTutorial(_espritApplication));
            simulation.Tutorials.Add(tutorialSimulation);
            Tutorials.Add(simulation);

            var сurves = new TutorialModel("Curves", null);
            var tutorialThrownForCurve = new TutorialModel(new ThrownForCurveTutorial(_espritApplication));
            var tutorialCreateCurvesUsingFeaturePtop = new TutorialModel(new CreateCurvesUsingFeaturePtopTutorial(_espritApplication));
            var tutorialExponentialAndSineWaveCurves = new TutorialModel(new ExponentialAndSineWaveCurvesTutorial(_espritApplication));
            var tutorialCreateCurvesFromAnyEquation = new TutorialModel(new CreateCurvesFromAnyEquationTutorial(_espritApplication));
            var tutorialCrossSectionSurfaceMeshCurves = new TutorialModel(new CrossSectionSurfaceMeshCurvesTutorial(_espritApplication));
            сurves.Tutorials.Add(tutorialThrownForCurve);
            сurves.Tutorials.Add(tutorialCreateCurvesUsingFeaturePtop);
            сurves.Tutorials.Add(tutorialExponentialAndSineWaveCurves);
            сurves.Tutorials.Add(tutorialCreateCurvesFromAnyEquation);
            сurves.Tutorials.Add(tutorialCrossSectionSurfaceMeshCurves);
            Tutorials.Add(сurves);

            var dataTransfer = new TutorialModel("Data Transfer", null);
            var tutorialExportImportPointData = new TutorialModel(new ExportImportPointDataTutorial(_espritApplication));
            dataTransfer.Tutorials.Add(tutorialExportImportPointData);
            Tutorials.Add(dataTransfer);

            var machineSetup = new TutorialModel("Machine Setup", null);
            var tutorialExportMachineSetupData = new TutorialModel(new ExportMachineSetupDataTutorial(_espritApplication));
            var tutorialScanMachineSetup = new TutorialModel(new ScanMachineSetupTutorial(_espritApplication));
            var tutorialCreateMachineSetup = new TutorialModel(new CreateMachineSetupTutorial(_espritApplication));
            machineSetup.Tutorials.Add(tutorialScanMachineSetup);
            machineSetup.Tutorials.Add(tutorialCreateMachineSetup);
            machineSetup.Tutorials.Add(tutorialExportMachineSetupData);
            Tutorials.Add(machineSetup);

            var partSetup = new TutorialModel("Part Setup", null);
            var tutorialScanPartSetup = new TutorialModel(new ScanPartSetupTutorial(_espritApplication));
            var tutorialCreatePartSetupWorkpiece = new TutorialModel(new CreatePartSetupWorkpieceTutorial(_espritApplication));
            partSetup.Tutorials.Add(tutorialScanPartSetup);
            partSetup.Tutorials.Add(tutorialCreatePartSetupWorkpiece);
            Tutorials.Add(partSetup);

            var other = new TutorialModel("Other", null);
            var tutorialParameterObject = new TutorialModel(new ParameterObjectTutorial(_espritApplication));
            var tutorialFindToolParams = new TutorialModel(new FindToolParamsTutorial(_espritApplication));
            var tutorialToolingCustomSetupSheet = new TutorialModel(new ToolingCustomSetupSheetTutorial(_espritApplication));
            var tutorialCustomOperationSetupSheet = new TutorialModel(new CustomOperationSetupSheetTutorial(_espritApplication));
            var tutorialCheckingSecurity = new TutorialModel(new CheckingSecurityTutorial(_espritApplication));
            var tutorialStlFiles = new TutorialModel(new StlFilesTutorial(_espritApplication));
            var tutorialSwarfCycle = new TutorialModel(new SwarfCycleTutorial(_espritApplication));
            var tutorialAdditiveDED = new TutorialModel(new AdditiveDEDTutorial(_espritApplication));
            other.Tutorials.Add(tutorialParameterObject);
            other.Tutorials.Add(tutorialFindToolParams);
            other.Tutorials.Add(tutorialToolingCustomSetupSheet);
            other.Tutorials.Add(tutorialCustomOperationSetupSheet);
            other.Tutorials.Add(tutorialCheckingSecurity);
            other.Tutorials.Add(tutorialStlFiles);
            other.Tutorials.Add(tutorialSwarfCycle);
            other.Tutorials.Add(tutorialAdditiveDED);
            Tutorials.Add(other);
        }
    }
}
