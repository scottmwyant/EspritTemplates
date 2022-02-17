namespace TutorialCSharp.Tutorials
{
    class SwapLayersTutorial : BaseTutorial
    {
        public SwapLayersTutorial(Esprit.Application app): base(app)
        {
        }

        //! [Code snippet]

        public void SwapLayers()
        {
            foreach(Esprit.Layer layer in Document.Layers)
            {
                layer.Visible = !layer.Visible;
            }
        }

        //! [Code snippet]

        public override void Execute()
        {
            SwapLayers();
        }

        public override string Name => "Swap Layers";
        public override string HtmlPath => "html/swap_layers_tutorial.html";

    }
}
