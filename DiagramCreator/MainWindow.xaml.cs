﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

            double thickness = 0.1;
            Brush brush = Brushes.Red;

            double step = 0.05;

            int width = 50;
            int height = 50;

            BitmapSource bitmapSource = BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, null, new byte[256 * 256 * 4], 256 * 4);
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawImage(bitmapSource, new Rect(0, 0, width, height));

                drawingContext.DrawLine(
                        new Pen(Brushes.Black, 0.1),
                        new Point(width / 2, 0),
                        new Point(width / 2, height)
                        );
                drawingContext.DrawLine(
                        new Pen(Brushes.Black, 0.1),
                        new Point(0, height / 2),
                        new Point(width, height / 2)
                        );

                for (double x = -(width / 2); x < width / 2; x += step)
                {
                    drawingContext.DrawLine(
                        new Pen(brush, thickness),
                        new Point(x + width / 2, height / 2 + function(x)),
                        new Point(x + step + width / 2, height / 2 + function(x + step))
                        );
                }
            }
            var image = new DrawingImage(visual.Drawing);
            img.Source = image;
        }

        public double function(double x)
        {
            return (Math.Tan(x)) * -1;//в круглые скобки написать формулу
        }
    }
}
