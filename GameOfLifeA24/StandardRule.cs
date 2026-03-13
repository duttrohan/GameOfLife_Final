namespace GameOfLifeA24
{
    public class StandardRule : IRule
    {
        public Grid CreateNextGeneration(Grid currentGrid)
        {
            // Fix for CS7036: Pass 4 arguments to the Grid constructor
            // 1. Width, 2. Height, 3. The current Rule (this), 4. The Factory from the old grid
            ICellFactory factory = currentGrid.GetFactory();
            Grid nextGrid = new Grid(currentGrid.Width, currentGrid.Height, this, factory);

            for (int x = 0; x < currentGrid.Width; x++)
            {
                for (int y = 0; y < currentGrid.Height; y++)
                {
                    // 1. Get the current cell
                    Cell currentCell = currentGrid.GetCell(x, y);

                    // 2. Count how many neighbors are alive (using wraparound logic)
                    int livingNeighbors = CountAliveNeighbors(currentGrid, x, y);

                    // 3. Ask the cell what its next state should be based on the count
                    // This uses the internal logic of AliveCell/DeadCell (State Pattern)
                    Cell nextCell = currentCell.NextState(livingNeighbors);

                    // 4. Put the result in the new grid
                    nextGrid.SetCell(x, y, nextCell);
                }
            }
            return nextGrid;
        }

        private int CountAliveNeighbors(Grid grid, int x, int y)
        {
            int count = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    // Wraparound logic
                    int neighborX = (x + i + grid.Width) % grid.Width;
                    int neighborY = (y + j + grid.Height) % grid.Height;

                    if (grid.GetCell(neighborX, neighborY).isAlive())
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}