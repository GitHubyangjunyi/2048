using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static private int n = 4;
        private long[,] grids = new long[n, n];
        private Label[,] lbls = new Label[n, n];
        private int endx = 0;
        private int endy = 0;
        private long score = 0;
        private long record = 0;
        private bool gameStatus = false;
        public MainWindow()
        {
            InitializeComponent();
            drawLB();
            this.PreviewKeyDown += new KeyEventHandler(OnFormPKD);
            init();
        }



        private void OnFormPKD(object sender, KeyEventArgs e)
        {
            if (gameStatus)
                if (sld1.IsEnabled)
                    sld1.IsEnabled = false;
            switch (e.Key)
            {
                case Key.Down:
                    down();
                    genRand();
                    e.Handled = true;
                    break;
                case Key.Up:
                    up();
                    genRand();
                    e.Handled = true;
                    break;
                case Key.Left:
                    left();
                    genRand();
                    e.Handled = true;
                    break;
                case Key.Right:
                    right();
                    genRand();
                    e.Handled = true;
                    break;
                case Key.Escape:
                    gameStatus = false;
                    if (score > record)
                    {
                        record = score;
                        lbl_record.Content = record.ToString();
                    }
                    e.Handled = true;
                    init();
                    break;
            }
        }


        string[] colors = {
            "#ffffff",
            "#ffff00",
            "#9aCD32",
            "#2faa2f" ,
            "#20B2AA" ,
            "#1088fF" ,
            "#7b68ee" ,
            "#9932cc" ,
            "#800080",
            "#8b0000" ,
            "#333333"
        };
        private void paint(int x, int y)
        {
            if (grids[x, y] == 0)
            {
                lbls[x, y].Content = "";
                lbls[x, y].Background = new SolidColorBrush(Color.FromRgb(0xFE, 0xFE, 0xEE));
            }
            else
            {
                lbls[x, y].Content = grids[x, y].ToString();
                int lg = (int)Math.Log(grids[x, y], 2);
                if (lg > 10)
                    lg = 10;
                var bc = new BrushConverter();
                lbls[x, y].Background = (Brush)bc.ConvertFrom(colors[lg]);
                Brush fore;
                if (lg > 5)
                    fore = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                else
                    fore = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                lbls[x, y].Foreground = fore;
            }
        }

        private void gameover()
        {
            gameStatus = false;
            if (score > record)
            {
                record = score;
                lbl_record.Content = record.ToString();
            }
            topLabel.Visibility = Visibility.Visible;
            sld1.IsEnabled = true;
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {
                timer.Stop();
                init();
            };
        }

        private void init()
        {
            topLabel.Visibility = Visibility.Hidden;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    grids[i, j] = 0;
                    paint(i, j);
                }
            score = 0;
            gameStatus = true;
            lbl_score.Content = score.ToString();
            endx = 0;
            endy = 0;
            genRand();
            genRand();
        }

        private void genRand()
        {
            List<coord> empty = new List<coord>();
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    if (grids[i, j] == 0)
                        empty.Add(new coord(i, j));
                }
            if (empty.Count != 0)
            {
                int len = empty.Count;
                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                int num = rnd.Next(0, len);
                grids[empty[num].x, empty[num].y] = 2;
                paint(empty[num].x, empty[num].y);
            }
        }
        private class coord
        {
            public int x = 0;
            public int y = 0;
            public coord(int a, int b)
            {
                x = a;
                y = b;
            }
        }

        private void down()
        {
            int move = 0;
            for (int x = 0; x < n; x++)
            {
                List<long> column = new List<long>();
                int y;
                for (y = 0; y < n; y++)
                {
                    if (grids[x, y] != 0)
                    {
                        if (column.Count > 0 && grids[x, y] == column[column.Count - 1])
                        {
                            move = 1;
                            column[column.Count - 1] += grids[x, y];
                            score += column[column.Count - 1];
                            lbl_score.Content = score.ToString();
                        }
                        else
                        {
                            column.Add(grids[x, y]);
                        }
                    }
                    else
                        move = 1;
                }
                for (y = 0; y < n - column.Count; y++)
                {
                    grids[x, y] = 0;
                    paint(x, y);
                }
                for (y = n - 1; y >= n - column.Count; y--)
                {
                    grids[x, y] = column[y - n + column.Count];
                    paint(x, y);
                }
            }
            if (move == 0)
            {
                endy = 1;
                if (endx == 1)
                    gameover();
            }
        }
        private void up()
        {
            int move = 0;
            for (int x = 0; x < n; x++)
            {
                List<long> column = new List<long>();
                int y;
                for (y = 0; y < n; y++)
                {
                    if (grids[x, y] != 0)
                    {
                        if (column.Count > 0 && grids[x, y] == column[column.Count - 1])
                        {
                            move = 1;
                            column[column.Count - 1] += grids[x, y];
                            score += column[column.Count - 1];
                            lbl_score.Content = score.ToString();
                        }
                        else
                        {
                            column.Add(grids[x, y]);
                        }
                    }
                    else
                        move = 1;
                }
                for (y = 0; y < column.Count; y++)
                {
                    grids[x, y] = column[y];
                    paint(x, y);
                }
                for (y = column.Count; y < n; y++)
                {
                    grids[x, y] = 0;
                    paint(x, y);
                }
            }
            if (move == 0)
            {
                endy = 1;
                if (endx == 1)
                    gameover();
            }
        }

        private void right()
        {
            int move = 0;
            for (int y = 0; y < n; y++)
            {
                List<long> column = new List<long>();
                int x;
                for (x = 0; x < n; x++)
                {
                    if (grids[x, y] != 0)
                    {
                        if (column.Count > 0 && grids[x, y] == column[column.Count - 1])
                        {
                            move = 1;
                            column[column.Count - 1] += grids[x, y];
                            score += column[column.Count - 1];
                            lbl_score.Content = score.ToString();
                        }
                        else
                        {
                            column.Add(grids[x, y]);
                        }
                    }
                    else
                        move = 1;
                }
                for (x = 0; x < n - column.Count; x++)
                {
                    grids[x, y] = 0;
                    paint(x, y);
                }
                for (x = n - 1; x >= n - column.Count; x--)
                {
                    grids[x, y] = column[x - n + column.Count];
                    paint(x, y);
                }
            }
            if (move == 0)
            {
                endx = 1;
                if (endy == 1)
                    gameover();
            }
        }

        private void left()
        {
            int move = 0;
            for (int y = 0; y < n; y++)
            {
                List<long> column = new List<long>();
                int x;
                for (x = 0; x < n; x++)
                {
                    if (grids[x, y] != 0)
                    {
                        if (column.Count > 0 && grids[x, y] == column[column.Count - 1])
                        {
                            move = 1;
                            column[column.Count - 1] += grids[x, y];
                            score += column[column.Count - 1];
                            lbl_score.Content = score.ToString();
                        }
                        else
                        {
                            column.Add(grids[x, y]);
                        }
                    }
                    else
                        move = 1;
                }
                for (x = 0; x < column.Count; x++)
                {
                    grids[x, y] = column[x];
                    paint(x, y);
                }
                for (x = column.Count; x < n; x++)
                {
                    grids[x, y] = 0;
                    paint(x, y);
                }
            }
            if (move == 0)
            {
                endx = 1;
                if (endy == 1)
                    gameover();
            }
        }

        private void drawLB()
        {
            for (int x = 0; x < n; x++)
                for (int y = 0; y < n; y++)
                {
                    lbls[x, y] = new Label
                    {
                        Content = "",
                        Width = 100,
                        Height = 100,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(0, 0, 0, 0),
                        FontSize = 50,
                        BorderThickness = new Thickness(2),
                        BorderBrush = Brushes.BurlyWood
                    };
                    mainGrid.Children.Add(lbls[x, y]);
                    Grid.SetRow(lbls[x, y], y);
                    Grid.SetColumn(lbls[x, y], x);
                }
        }

        private void sld1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            n = (int)sld1.Value;
            int oldSize = mainGrid.RowDefinitions.Count;
            if (n != oldSize)
            {
                mainGrid.Height = n * 100;
                mainGrid.Width = n * 100;
                this.Height = mainGrid.Height + 145;
                this.Width = mainGrid.Width + 15;
                Grid.SetRowSpan(fm1, n);
                Grid.SetColumnSpan(fm1, n);
                grids = new long[n, n];
                lbls = new Label[n, n];
                if (n > oldSize)
                {
                    for (int i = oldSize; i < n; i++)
                    {
                        mainGrid.RowDefinitions.Add(new RowDefinition());
                        mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    }
                }
                else
                {
                    mainGrid.RowDefinitions.RemoveRange(n, oldSize - n);
                    mainGrid.ColumnDefinitions.RemoveRange(n, oldSize - n);
                }
            }
            drawLB();
            init();
        }
    }

}
