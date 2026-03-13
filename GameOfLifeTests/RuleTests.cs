using Xunit;
using GameOfLifeA24;

namespace GameOfLifeTests
{
    public class RuleTests
    {
        [Fact]
        public void StandardRule_DeadCellWithThreeNeighbors_ShouldBeBorn()
        {
            // 1. Arrange
            // We use ICellFactory (the interface) to avoid access errors
            // If CellFactory is still internal, you can change 'internal' to 'public' 
            // in CellFactory.cs temporarily to make this pass.
            ICellFactory factory = new CellFactory();
            var rule = new StandardRule();

            // Match the constructor in your Logic project
            var grid = new Grid(3, 3, rule, factory);

            grid.SetCell(0, 0, factory.CreateCell(0, 0, true));
            grid.SetCell(1, 0, factory.CreateCell(1, 0, true));
            grid.SetCell(2, 0, factory.CreateCell(2, 0, true));

            // 2. Act
            var nextGen = rule.CreateNextGeneration(grid);

            // 3. Assert
            Assert.True(nextGen.GetCell(1, 1).isAlive());
        }
    }
}