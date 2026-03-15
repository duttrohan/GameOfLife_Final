using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using GameOfLifeA24;
using DataLayerGameOfLife; // This namespace contains your Factory and Interface

namespace GameOfLifeUI
{
    public partial class MainWindow : Window
    {
        private Game? _game;
        private ICellFactory? _factory;
        private DispatcherTimer? _timer;
        private const int CellSize = 20;

        // 1. Requirement: Use the Interface instead of the concrete class
        // 2. Requirement: Use the Singleton Factory to get the instance
        private IInitialStateRepository _repository = RepositoryFactory.Instance.GetInitialStateRepository();

        public MainWindow()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(200);
            _timer.Tick += Timer_Tick;

            SetupGame();
        }

        private void SetupGame()
        {
            _factory = new CellFactory();
            _game = new Game(30, 30, new StandardRule(), _factory);
            DrawCanvas();
        }

        private void DrawCanvas()
        {
            if (_game == null) return;

            GameCanvas.Children.Clear();
            for (int x = 0; x < _game.CurrentGrid.Width; x++)
            {
                for (int y = 0; y < _game.CurrentGrid.Height; y++)
                {
                    var cell = _game.CurrentGrid.GetCell(x, y);
                    if (cell.isAlive())
                    {
                        Rectangle rect = new Rectangle
                        {
                            Width = CellSize - 1,
                            Height = CellSize - 1,
                            Fill = Brushes.GreenYellow
                        };
                        Canvas.SetLeft(rect, x * CellSize);
                        Canvas.SetTop(rect, y * CellSize);
                        GameCanvas.Children.Add(rect);
                    }
                }
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _game?.Step();
            DrawCanvas();
        }

        private void btnStep_Click(object sender, RoutedEventArgs e)
        {
            _game?.Step();
            DrawCanvas();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (_timer == null) return;
            if (_timer.IsEnabled)
            {
                _timer.Stop();
                btnPlay.Content = "Auto Play";
            }
            else
            {
                _timer.Start();
                btnPlay.Content = "Stop";
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            _timer?.Stop();
            btnPlay.Content = "Auto Play";
            SetupGame();
        }

        private void GameCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_game == null || _factory == null) return;

            Point clickPoint = e.GetPosition(GameCanvas);
            int x = (int)(clickPoint.X / CellSize);
            int y = (int)(clickPoint.Y / CellSize);

            if (x >= 0 && x < _game.CurrentGrid.Width && y >= 0 && y < _game.CurrentGrid.Height)
            {
                _game.CurrentGrid.SetCell(x, y, _factory.CreateCell(x, y, true));
                DrawCanvas();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_game == null) return;

            string patternName = txtPatternName.Text;

            if (string.IsNullOrWhiteSpace(patternName))
            {
                MessageBox.Show("Please enter a name for your pattern!");
                return;
            }

            var aliveCells = new List<(int x, int y)>();
            for (int x = 0; x < _game.CurrentGrid.Width; x++)
            {
                for (int y = 0; y < _game.CurrentGrid.Height; y++)
                {
                    if (_game.CurrentGrid.GetCell(x, y).isAlive())
                    {
                        aliveCells.Add((x, y));
                    }
                }
            }

            try
            {
                // Use the interface method 'Add' instead of 'SavePattern'
                _repository.Add(patternName, aliveCells);
                MessageBox.Show($"Pattern '{patternName}' successfully saved to SQL Server!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving to database: " + ex.Message);
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (_game == null || _factory == null) return;

            string patternName = txtPatternName.Text;

            try
            {
                // Use the interface method 'Get' instead of 'LoadPattern'
                var savedCells = _repository.Get(patternName);

                if (savedCells == null || savedCells.Count == 0)
                {
                    MessageBox.Show($"No saved pattern found with the name '{patternName}'.");
                    return;
                }

                SetupGame(); // Clear current grid

                foreach (var cell in savedCells)
                {
                    if (cell.x < _game.CurrentGrid.Width && cell.y < _game.CurrentGrid.Height)
                    {
                        _game.CurrentGrid.SetCell(cell.x, cell.y, _factory.CreateCell(cell.x, cell.y, true));
                    }
                }

                DrawCanvas();
                MessageBox.Show($"Pattern '{patternName}' successfully loaded!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading from database: " + ex.Message);
            }
        }
    }
}