using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Math;

namespace DiagramCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            List<Func<double, double>> functions = new List<Func<double, double>>() 
            {
                x => Pow(25 - Pow(x, 2), 0.5),
                x => -Pow(25 - Pow(x, 2), 0.5)
            };

            Random random = new Random();
            double thickness = 0.1;
            Brush markColor = Brushes.Black;
            Brush backgroungMarksColor = Brushes.LightGray;
            double fontSize = 0.4;
            double markThickness = 0.1;
            double calculationStep = 0.05;
            int width = 50;
            int height = 50;
            double dashHeight = 0.1;

            BitmapSource bitmapSource = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, new byte[256 * 256 * 4], 256 * 4);
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawImage(bitmapSource, new Rect(0, 0, width, height));
                                
                for (int i = 0; i < width; i++) // разметка
                {
                    drawingContext.DrawLine(
                        new Pen(backgroungMarksColor, markThickness / 2),
                        new Point(i, 0),
                        new Point(i, height)
                        );
                    drawingContext.DrawLine(
                        new Pen(backgroungMarksColor, markThickness / 2),
                        new Point(0, i),
                        new Point(width, i)
                        );

                    drawingContext.DrawText(
                        new FormattedText((-width / 2 + i).ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(""), fontSize, markColor),
                        new Point(i, height / 2 + 0.1 + markThickness) 
                        );
                    drawingContext.DrawText(
                        new FormattedText((height / 2 - i).ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(""), fontSize, markColor),
                        new Point(width / 2 + 0.1 + markThickness, i)
                        );
                    drawingContext.DrawLine(
                        new Pen(markColor, markThickness),
                        new Point(i , height / 2 + dashHeight),
                        new Point(i , height / 2 - dashHeight)
                        );
                    drawingContext.DrawLine(
                        new Pen(markColor, markThickness),
                        new Point(width / 2 + dashHeight, i),
                        new Point(width / 2 - dashHeight, i)
                        );
                }

                drawingContext.DrawLine(
                        new Pen(markColor, markThickness),
                        new Point(width / 2, 0),
                        new Point(width / 2, height)
                        ); // Oy
                drawingContext.DrawLine(
                        new Pen(markColor, markThickness),
                        new Point(0, height / 2),
                        new Point(width, height / 2)
                        ); // Ox

                foreach (var function in functions)
                {
                    Brush brush = new SolidColorBrush(Color.FromArgb(255, (byte)random.Next(50, 255), (byte)random.Next(50, 255), (byte)random.Next(50, 255))); 

                    for (double x = -(width / 2); x < width / 2; x += calculationStep)
                    {
                        double x1 = x + width / 2;
                        double x2 = x + calculationStep + width / 2;

                        double y1 = height / 2 + (function(x) * -1);
                        double y2 = height / 2 + (function(x + calculationStep) * -1);

                        if (y1 <= height && y1 >= 0 && y2 <= height && y2 >= 0)
                        {
                            drawingContext.DrawLine(
                                new Pen(brush, thickness),
                                new Point(x1, y1),
                                new Point(x2, y2)
                                );
                        }
                    }
                }
            }
            var image = new DrawingImage(visual.Drawing);
            img.Source = image;
        }
    }
}
