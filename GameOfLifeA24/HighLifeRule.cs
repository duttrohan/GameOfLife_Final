using GameOfLifeA24.Cells;

namespace GameOfLifeA24
{
    public class HighLifeRule : IRule
    {
        public Grid CreateNextGeneration(Grid currentGrid)
        {
            ICellFactory factory = currentGrid.GetFactory();
            Grid nextGrid = new Grid(currentGrid.Width, currentGrid.Height, this, factory);

            for (int x = 0; x < currentGrid.Width; x++)
            {
                for (int y = 0; y < currentGrid.Height; y++)
                {
                    int neighbors = CountAliveNeighbors(currentGrid, x, y);
                    Cell currentCell = currentGrid.GetCell(x, y);

                    // HighLife: Birth on 3 or 6 neighbors
                    if (!currentCell.IsAlive() && (neighbors == 3 || neighbors == 6))
                    {
                        nextGrid.SetCell(x, y, factory.CreateCell(x, y, true));
                    }
                    else
                    {
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
                    if (i == 0 && j == 0)
                        continue;

                    int nX = (x + i + grid.Width) % grid.Width;
                    int nY = (y + j + grid.Height) % grid.Height;

                    if (grid.GetCell(nX, nY).IsAlive())
                        count++;
                }
            }

            return count;
        }
    }
}
