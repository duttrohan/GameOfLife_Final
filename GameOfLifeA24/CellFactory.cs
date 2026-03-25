using GameOfLifeA24.Cells;

namespace GameOfLifeA24
{
    public class CellFactory : ICellFactory
    {
        public Cell CreateCell(int x, int y, bool isAlive)
        {
            return isAlive ? new AliveCell(x, y) : new DeadCell(x, y);
        }
    }
}
