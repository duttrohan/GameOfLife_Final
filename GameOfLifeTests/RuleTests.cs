using Xunit;
using GameOfLifeA24;

namespace GameOfLifeTests
{
    public class RuleTests
    {
        [Fact]
        public void StandardRule_DeadCellWithThreeNeighbors_ShouldBeBorn()
        {
            ICellFactory factory = new CellFactory();
            var rule = new StandardRule();

            var grid = new Grid(3, 3, rule, factory);

            grid.SetCell(0, 0, factory.CreateCell(0, 0, true));
            grid.SetCell(1, 0, factory.CreateCell(1, 0, true));
            grid.SetCell(2, 0, factory.CreateCell(2, 0, true));

            var nextGen = rule.CreateNextGeneration(grid);

            // FIXED HERE
            Assert.True(nextGen.GetCell(1, 1).IsAlive());
        }
    }
}
