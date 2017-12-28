using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        private long _score = 0;
        private long _record = 0;
        private Random rnd = new Random(Guid.NewGuid().GetHashCode());
        private bool move = false;
        private Task[] task;
        private bool gameOn = false;

        private long score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
                lbl_score.Content = value.ToString();
                if (value > record)
                    record = value;
            }
        }

        private long record
        {
            get
            {
                return _record;
            }
            set
            {
                _record = value;
                lbl_record.Content = value.ToString();
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            drawLB();
            PreviewKeyDown += new KeyEventHandler(OnFormPKD);
            init();
        }

        private void OnFormPKD(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (gameOn)
                    {
                        down();
                        Task.Factory.StartNew(genRand);
                    }
                    e.Handled = true;
                    break;
                case Key.Up:
                    if (gameOn)
                    {
                        up();
                        Task.Factory.StartNew(genRand);
                    }
                    e.Handled = true;
                    break;
                case Key.Left:
                    if (gameOn)
                    {
                        left();
                        Task.Factory.StartNew(genRand);
                    }
                    e.Handled = true;
                    break;
                case Key.Right:
                    if (gameOn)
                    {
                        right();
                        Task.Factory.StartNew(genRand);
                    }
                    e.Handled = true;
                    break;
                case Key.Escape:
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
            "#aa0000",
            "#333333"
        };

        private void quickPaint(int x, int y)
        {
            if (grids[x, y] == 0)
            {
                lbls[x, y].Content = "";
                lbls[x, y].Background = new SolidColorBrush(Color.FromRgb(0xFE, 0xFE, 0xEE));
            }
            else
            {
                string strNum = grids[x, y].ToString();
                lbls[x, y].Content = strNum;
                int fontSize;
                if (strNum.Length > 3)
                    fontSize = 150 / strNum.Length;
                else
                    fontSize = 50;
                lbls[x, y].FontSize = fontSize;
                int lg = (int)Math.Log(grids[x, y], 2);
                if (lg > 11)
                    lg = 11;
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

        private async Task paint(int x, int y)
        {
            quickPaint(x, y);
            lbls[x, y].Refresh();
            await Task.Delay(50 / n);
        }

        private void gameover()
        {
            gameOn = false;
            topLabel.Visibility = Visibility.Visible;
            sld1.IsEnabled = true;
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {
                timer.Stop();
                if (!gameOn)
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
                    quickPaint(i, j);
                }
            score = 0;
            task = new Task[n];
            move = true;
            //drawTestGrids();    //debug
            genRand();
            genRand();
            gameOn = true;
        }

        //For debug only
        private void drawTestGrids()
        {
            grids[0, 0] = 64;
            quickPaint(0, 0);
            grids[0, 1] = 128;
            quickPaint(0, 1);
            grids[0, 2] = 1024;
            quickPaint(0, 2);
        }

        private void genRand()
        {
            if (task[0] != null)
                lock (task)
                    Task.WaitAll(task);
            List<coord> empty = new List<coord>();
            for (int x = 0; x < n; x++)
                for (int y = 0; y < n; y++)
                    if (grids[x, y] == 0)
                        empty.Add(new coord(x, y));
            if (empty.Count == 0 && !move)
            {
                Dispatcher.BeginInvoke((Action)gameover);
                return;
            }
            if (empty.Count != 0 && move)
            {
                int len = empty.Count;
                int i = rnd.Next(0, len);
                lock (grids)
                    grids[empty[i].x, empty[i].y] = 2;
                Dispatcher.BeginInvoke((Action)(() => quickPaint(empty[i].x, empty[i].y)));
            }
        }

        private void down()
        {
            move = false;
            for (int x = 0; x < n; x++)
                task[x] = _down(x);
        }
        private void up()
        {
            move = false;
            for (int x = 0; x < n; x++)
                task[x] = _up(x);
        }
        private void right()
        {
            move = false;
            for (int y = 0; y < n; y++)
                task[y] = _right(y);
        }
        private void left()
        {
            move = false;
            for (int y = 0; y < n; y++)
                task[y] = _left(y);
        }

        private async Task _down(int x)
        {
            for (int y = n - 1; y > 0; y--)
            {
                int y1 = y - 1;
                while (y1 >= 0 && grids[x, y1] == 0)
                    y1--;
                if (y1 >= 0 && grids[x, y] != 0 && grids[x, y] == grids[x, y1])
                {
                    await moveGrid(x, y1, x, y);
                    score += grids[x, y];
                }
            }
            for (int y = n - 1; y > 0; y--)
            {
                int y1 = y - 1;
                while (y1 >= 0 && grids[x, y1] == 0)
                    y1--;
                if (y1 >= 0)
                {
                    if (grids[x, y] == 0)
                        await moveGrid(x, y1, x, y);
                    else if (y1 != y - 1)
                        await moveGrid(x, y1, x, y - 1);
                }
            }
        }

        private async Task _up(int x)
        {
            for (int y = 0; y < n - 1; y++)
            {
                int y1 = y + 1;
                while (y1 < n && grids[x, y1] == 0)
                    y1++;
                if (y1 < n && grids[x, y] != 0 && grids[x, y] == grids[x, y1])
                {
                    await moveGrid(x, y1, x, y);
                    score += grids[x, y];
                }
            }
            for (int y = 0; y < n - 1; y++)
            {
                int y1 = y + 1;
                while (y1 < n && grids[x, y1] == 0)
                    y1++;
                if (y1 < n)
                {
                    if (grids[x, y] == 0)
                        await moveGrid(x, y1, x, y);
                    else if (y1 != y + 1)
                        await moveGrid(x, y1, x, y + 1);
                }
            }
        }

        private async Task _right(int y)
        {
            for (int x = n - 1; x > 0; x--)
            {
                int x1 = x - 1;
                while (x1 >= 0 && grids[x1, y] == 0)
                    x1--;
                if (x1 >= 0 && grids[x, y] != 0 && grids[x1, y] == grids[x, y])
                {
                    await moveGrid(x1, y, x, y);
                    score += grids[x, y];
                }
            }
            for (int x = n - 1; x > 0; x--)
            {
                int x1 = x - 1;
                while (x1 >= 0 && grids[x1, y] == 0)
                    x1--;
                if (x1 >= 0)
                {
                    if (grids[x, y] == 0)
                        await moveGrid(x1, y, x, y);
                    else if (x1 != x - 1)
                        await moveGrid(x1, y, x - 1, y);
                }
            }
        }

        private async Task _left(int y)
        {
            for (int x = 0; x < n - 1; x++)
            {
                int x1 = x + 1;
                while (x1 < n && grids[x1, y] == 0)
                    x1++;
                if (x1 < n && grids[x, y] != 0 && grids[x1, y] == grids[x, y])
                {
                    await moveGrid(x1, y, x, y);
                    score += grids[x, y];
                }
            }
            for (int x = 0; x < n - 1; x++)
            {
                int x1 = x + 1;
                while (x1 < n && grids[x1, y] == 0)
                    x1++;
                if (x1 < n)
                {
                    if (grids[x, y] == 0)
                        await moveGrid(x1, y, x, y);
                    else if (x1 != x + 1)
                        await moveGrid(x1, y, x + 1, y);
                }
            }
        }


        //animated move effect
        private async Task moveGrid(int x1, int y1, int x2, int y2)
        {
            if (x1 == x2 && y1 == y2)
                return;
            int xincre, yincre;
            if (x2 > x1)
                xincre = 1;
            else if (x2 < x1)
                xincre = -1;
            else
                xincre = 0;
            if (y2 > y1)
                yincre = 1;
            else if (y2 < y1)
                yincre = -1;
            else
                yincre = 0;
            int x = x1, y = y1;
            do
            {
                do
                {
                    grids[x + xincre, y + yincre] += grids[x, y];
                    grids[x, y] = 0;
                    await paint(x, y);
                    await paint(x + xincre, y + yincre);
                    y += yincre;
                    move = true;
                } while (y != y2);
                x += xincre;
            } while (x != x2);
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
                this.Width = mainGrid.Width + 20;
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

        private struct coord
        {
            public int x;
            public int y;
            public coord(int a, int b)
            {
                x = a;
                y = b;
            }
            public override int GetHashCode()
            {
                return x << 16 + y;
            }
        }
    }

    public static class ExtensionMethods
    {
        private static Action EmptyDelegate = delegate () { };
        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }
}
