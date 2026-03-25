namespace GameOfLifeA24
{
    public class Game
    {
        public Grid CurrentGrid { get; private set; }
        private IRule _rule;
        private ICellFactory _factory;

        public Game(int width, int height, IRule rule, ICellFactory factory)
        {
            _rule = rule;
            _factory = factory;

            CurrentGrid = new Grid(width, height, _rule, _factory);
        }

        public void Step()
        {
            CurrentGrid = _rule.CreateNextGeneration(CurrentGrid);
        }
    }
}
