namespace GameOfLifeA24
{
    public interface ICellFactory
    {
        Cell CreateCell(int x, int y, bool isAlive);
    }
} // Make sure this last bracket is here!