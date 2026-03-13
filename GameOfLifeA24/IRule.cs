namespace GameOfLifeA24
{
    public interface IRule
    {
        // This 'contract' says every rule must be able to move the grid 
        // from the current generation to the next one.
        Grid CreateNextGeneration(Grid currentGrid);
    }
}