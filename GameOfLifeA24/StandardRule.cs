using GameOfLifeA24.Cells;

namespace GameOfLifeA24
{
    public class StandardRule : IRule
    {
        public Grid CreateNextGeneration(Grid currentGrid)
        {
            ICellFactory factory = currentGrid.GetFactory();
            Grid nextGrid = new Grid(currentGrid.Width, currentGrid.Height, this, factory);

            for (int x = 0; x < currentGrid.Width; x++)
            {
                for (int y = 0; y < currentGrid.Height; y++)
                {
                    Cell currentCell = currentGrid.GetCell(x, y);

                    int livingNeighbors = CountAliveNeighbors(currentGrid, x, y);

                    Cell nextCell = currentCell.NextState(livingNeighbors);

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
                    if (i == 0 && j == 0)
                        continue;

                    int neighborX = (x + i + grid.Width) % grid.Width;
                    int neighborY = (y + j + grid.Height) % grid.Height;

                    if (grid.GetCell(neighborX, neighborY).IsAlive())
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
