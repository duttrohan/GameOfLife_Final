using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using GameOfLifeA24;
using DataLayerGameOfLife;

namespace GameOfLifeUI
{
    public partial class MainWindow : Window
    {
        private Game? _game;
        private ICellFactory? _factory;
        private DispatcherTimer? _timer;
        private const int CellSize = 20;

        private IRule _selectedRule = new StandardRule();
        private IInitialStateRepository _repository = RepositoryFactory.Instance.GetInitialStateRepository();

        private bool _isLoaded = false;

        public MainWindow()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(200);
            _timer.Tick += Timer_Tick;

            Loaded += (s, e) =>
            {
                _isLoaded = true;
                SetupGame();
            };
        }

        private void SetupGame()
        {
            _factory = new CellFactory();
            _game = new Game(30, 30, _selectedRule, _factory);
            DrawCanvas();
        }

        private void DrawCanvas()
        {
            if (_game == null || GameCanvas == null) return;

            GameCanvas.Children.Clear();

            int width = _game.CurrentGrid.Width;
            int height = _game.CurrentGrid.Height;

            // Grid lines
            for (int x = 0; x <= width; x++)
            {
                GameCanvas.Children.Add(new Line
                {
                    X1 = x * CellSize,
                    Y1 = 0,
                    X2 = x * CellSize,
                    Y2 = height * CellSize,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 0.5
                });
            }

            for (int y = 0; y <= height; y++)
            {
                GameCanvas.Children.Add(new Line
                {
                    X1 = 0,
                    Y1 = y * CellSize,
                    X2 = width * CellSize,
                    Y2 = y * CellSize,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 0.5
                });
            }

            // Alive cells
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var cell = _game.CurrentGrid.GetCell(x, y);

                    if (cell.IsAlive())
                    {
                        Rectangle rect = new Rectangle
                        {
                            Width = CellSize - 1,
                            Height = CellSize - 1,
                            Fill = new SolidColorBrush(Color.FromRgb(74, 222, 128)),
                            Effect = new System.Windows.Media.Effects.DropShadowEffect
                            {
                                Color = Colors.Lime,
                                BlurRadius = 12,
                                ShadowDepth = 0,
                                Opacity = 0.8
                            }
                        };

                        Canvas.SetLeft(rect, x * CellSize);
                        Canvas.SetTop(rect, y * CellSize);
                        GameCanvas.Children.Add(rect);
                    }
                }
            }

            UpdateStats();
        }

        private void UpdateStats()
        {
            if (_game == null) return;

            int alive = 0;
            for (int x = 0; x < _game.CurrentGrid.Width; x++)
                for (int y = 0; y < _game.CurrentGrid.Height; y++)
                    if (_game.CurrentGrid.GetCell(x, y).IsAlive())
                        alive++;

            aliveCount.Text = alive.ToString();
            speedStat.Text = _timer?.Interval.TotalMilliseconds + " ms";
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _game?.Step();
            DrawCanvas();
            genCount.Text = (int.Parse(genCount.Text) + 1).ToString();
        }

        private void btnStep_Click(object sender, RoutedEventArgs e)
        {
            _game?.Step();
            DrawCanvas();
            genCount.Text = (int.Parse(genCount.Text) + 1).ToString();
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
                btnPlay.Content = "Pause";
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            _timer?.Stop();
            btnPlay.Content = "Auto Play";
            genCount.Text = "0";
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

        // ⭐ ORIGINAL SAVE
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
                for (int y = 0; y < _game.CurrentGrid.Height; y++)
                    if (_game.CurrentGrid.GetCell(x, y).IsAlive())
                        aliveCells.Add((x, y));

            try
            {
                _repository.Add(patternName, aliveCells);
                MessageBox.Show($"Pattern '{patternName}' successfully saved!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving: " + ex.Message);
            }
        }

        // ⭐ ORIGINAL LOAD
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (_game == null || _factory == null) return;

            string patternName = txtPatternName.Text;

            try
            {
                var savedCells = _repository.Get(patternName);

                if (savedCells == null || savedCells.Count == 0)
                {
                    MessageBox.Show($"No saved pattern found with the name '{patternName}'.");
                    return;
                }

                SetupGame();

                foreach (var cell in savedCells)
                    if (cell.x < _game.CurrentGrid.Width && cell.y < _game.CurrentGrid.Height)
                        _game.CurrentGrid.SetCell(cell.x, cell.y, _factory.CreateCell(cell.x, cell.y, true));

                DrawCanvas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading: " + ex.Message);
            }
        }

        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();

            for (int x = 0; x < _game.CurrentGrid.Width; x++)
                for (int y = 0; y < _game.CurrentGrid.Height; y++)
                    _game.CurrentGrid.SetCell(x, y, _factory.CreateCell(x, y, rand.NextDouble() < 0.25));

            DrawCanvas();
        }

        // ⭐ ORIGINAL SPEED
        private void cmbSpeed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isLoaded || _timer == null) return;

            if (cmbSpeed.SelectedItem is ComboBoxItem item)
            {
                if (item.Content.ToString().Contains("Slow"))
                    _timer.Interval = TimeSpan.FromMilliseconds(500);
                else if (item.Content.ToString().Contains("Medium"))
                    _timer.Interval = TimeSpan.FromMilliseconds(200);
                else if (item.Content.ToString().Contains("Fast"))
                    _timer.Interval = TimeSpan.FromMilliseconds(50);

                speedStat.Text = _timer.Interval.TotalMilliseconds + " ms";
            }
        }

        // ⭐ REQUIRED FOR YOUR XAML
        private void cmbPatterns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPatterns.SelectedItem is not ComboBoxItem item)
                return;

            string pattern = item.Content.ToString();

            if (pattern == "Select Pattern")
                return;

            LoadPattern(pattern);
        }

        private void LoadPattern(string? patternName)
        {
            if (string.IsNullOrWhiteSpace(patternName) || _game == null || _factory == null) return;

            SetupGame();

            List<(int x, int y)> cells = patternName switch
            {
                "Glider" => new() { (1, 0), (2, 1), (0, 2), (1, 2), (2, 2) },
                "Blinker" => new() { (1, 0), (1, 1), (1, 2) },
                "Toad" => new() { (1, 1), (2, 1), (3, 1), (0, 2), (1, 2), (2, 2) },
                "Beacon" => new() { (0, 0), (1, 0), (0, 1), (3, 2), (2, 3), (3, 3) },
                "LWSS" => new() { (1, 0), (4, 0), (0, 1), (0, 2), (4, 2), (0, 3), (1, 3), (2, 3), (3, 3) },
                _ => new()
            };

            foreach (var (x, y) in cells)
                if (x < _game.CurrentGrid.Width && y < _game.CurrentGrid.Height)
                    _game.CurrentGrid.SetCell(x, y, _factory.CreateCell(x, y, true));

            DrawCanvas();
        }

        private void cmbRules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isLoaded) return;

            if (cmbRules.SelectedItem is ComboBoxItem item)
            {
                _selectedRule = item.Content.ToString() switch
                {
                    "Standard Rule" => new StandardRule(),
                    "HighLife Rule" => new HighLifeRule(),
                    _ => _selectedRule
                };

                SetupGame();
            }
        }
    }
}
