using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using System.IO;
using System.Windows.Interop;

namespace pre_sarcopenia.sln
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeChart();
        }

        private void InitializeChart()
        {
            // 設置圖表的樣式和屬性

            // Create a chart
            Chart chart = new Chart();
            chart.Size = new System.Drawing.Size(600, 400);
            // Add some data to the chart
            chart.Series.Add(new Series("Data"));
            chart.Series["Data"].Points.AddXY("A", 10);
            chart.Series["Data"].Points.AddXY("B", 20);
            chart.Series["Data"].Points.AddXY("C", 30);
            chart.Series["Data"].Points.AddXY("D", 40);

            chart.SaveImage("./chart.png", ChartImageFormat.Png);

            // Load image to chartImage control
            chartImage.Source = new BitmapImage(new System.Uri("chart.png", System.UriKind.Relative));
        }
    }
}