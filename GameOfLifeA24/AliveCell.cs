namespace GameOfLifeA24
{
    // We add ": Cell" so it follows the same blueprint as the others
    public class AliveCell : Cell
    {
        public AliveCell(int x, int y) : base(x, y) { }

        // It's an AliveCell, so this is always true
        public override bool isAlive() => true;

        // The Survival & Death Rules
        public override Cell NextState(int aliveNeighbors)
        {
            // Rule: Stay alive if you have 2 or 3 neighbors
            if (aliveNeighbors == 2 || aliveNeighbors == 3)
            {
                return new AliveCell(X, Y);
            }

            // Rule: Die if you have too few (loneliness) or too many (overpopulation)
            return new DeadCell(X, Y);
        }
    }
}