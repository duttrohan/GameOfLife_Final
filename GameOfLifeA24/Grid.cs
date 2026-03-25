using GameOfLifeA24.Cells;

namespace GameOfLifeA24
{
    public class Grid
    {
        private Cell[,] _cells;
        private ICellFactory _cellFactory;
        private IRule _rule;

        public int Width { get; }
        public int Height { get; }

        // Constructor with rule + factory
        public Grid(int width, int height, IRule rule, ICellFactory factory)
        {
            Width = width;
            Height = height;
            _rule = rule;
            _cellFactory = factory;

            _cells = new Cell[width, height];
            InitializeDefaultGrid();
        }

        // Allows rules to access the factory
        public ICellFactory GetFactory()
        {
            return _cellFactory;
        }

        private void InitializeDefaultGrid()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _cells[x, y] = _cellFactory.CreateCell(x, y, false);
                }
            }
        }

        public void SetCell(int x, int y, Cell cell)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                _cells[x, y] = cell;
            }
        }

        public Cell GetCell(int x, int y)
        {
            return _cells[x, y];
        }
    }
}
