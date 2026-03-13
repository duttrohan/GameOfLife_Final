namespace GameOfLifeA24
{
    // 'abstract' means this is a template for Alive and Dead cells
    public abstract class Cell
    {
        // These tell the cell where it is on the grid
        public int X { get; }
        public int Y { get; }

        // This sets the location when a cell is created
        protected Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        // Every cell must be able to answer: "Are you alive?"
        public abstract bool isAlive();

        // Every cell must be able to decide its future state
        public abstract Cell NextState(int aliveNeighbors);
    }
}