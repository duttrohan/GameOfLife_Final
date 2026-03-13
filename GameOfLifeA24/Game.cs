namespace GameOfLifeA24
{
    public class Game
    {
        public Grid CurrentGrid { get; private set; }
        private IRule _rule;
        private ICellFactory _factory; // Added field to store the factory

        // Updated constructor: Now requires a factory as well
        public Game(int width, int height, IRule rule, ICellFactory factory)
        {
            _rule = rule;
            _factory = factory;

            // Fix for CS7036: Pass all 4 arguments to Grid
            CurrentGrid = new Grid(width, height, _rule, _factory);

            // The Grid's internal InitializeDefaultGrid() already 
            // handles filling the grid with dead cells using the factory, 
            // so we don't need the loop here anymore!
        }

        public void Step()
        {
            // Move the game forward by one generation
            CurrentGrid = _rule.CreateNextGeneration(CurrentGrid);
        }
    }
}