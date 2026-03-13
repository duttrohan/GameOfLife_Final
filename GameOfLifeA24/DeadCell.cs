namespace GameOfLifeA24
{
    // We add ": Cell" so it inherits the X and Y coordinates
    public class DeadCell : Cell
    {
        // Constructor: passes X and Y up to the base Cell
        public DeadCell(int x, int y) : base(x, y) { }

        // Returns false because, well, it's a DeadCell!
        public override bool isAlive() => false;

        // The Birth Rule
        public override Cell NextState(int aliveNeighbors)
        {
            // If exactly 3 neighbors are alive, this cell becomes alive!
            if (aliveNeighbors == 3)
            {
                return new AliveCell(X, Y);
            }

            // Otherwise, it stays dead
            return new DeadCell(X, Y);
        }
    }
}