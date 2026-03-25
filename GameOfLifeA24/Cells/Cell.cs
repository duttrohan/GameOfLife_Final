namespace GameOfLifeA24.Cells;

public abstract class Cell
{
    public int X { get; }
    public int Y { get; }

    protected Cell(int x, int y)
    {
        X = x;
        Y = y;
    }

    public abstract bool IsAlive();
    public abstract Cell NextState(int aliveNeighbors);
}
