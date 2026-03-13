namespace GameOfLifeA24
{
    public class HighLifeRule : IRule
    {
        public Grid CreateNextGeneration(Grid currentGrid)
        {
            // Fix for CS7036: Pass 'this' as the rule and the factory from the current grid
            // (Make sure you added the GetFactory() method to Grid.cs first!)
            ICellFactory factory = currentGrid.GetFactory();
            Grid nextGrid = new Grid(currentGrid.Width, currentGrid.Height, this, factory);

            for (int x = 0; x < currentGrid.Width; x++)
            {
                for (int y = 0; y < currentGrid.Height; y++)
                {
                    int neighbors = CountAliveNeighbors(currentGrid, x, y);
                    Cell currentCell = currentGrid.GetCell(x, y);

                    // HighLife Rule: Birth on 3 or 6 neighbors
                    if (!currentCell.isAlive() && (neighbors == 3 || neighbors == 6))
                    {
                        // Use the factory instead of 'new AliveCell'
                        nextGrid.SetCell(x, y, factory.CreateCell(x, y, true));
                    }
                    else
                    {
                        // Standard transitions (Survival/Death) handled by the cell itself
                        nextGrid.SetCell(x, y, currentCell.NextState(neighbors));
                    }
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
                    // Wraparound logic for torus-style grid
                    int nX = (x + i + grid.Width) % grid.Width;
                    int nY = (y + j + grid.Height) % grid.Height;
                    if (grid.GetCell(nX, nY).isAlive()) count++;
                }
            }
            return count;
        }
    }
}