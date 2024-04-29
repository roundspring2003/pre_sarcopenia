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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Threading;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System.Text.RegularExpressions;
using Syncfusion.Windows.Shared;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace pre_sarcopenia.sln
{
    public partial class MainWindow : System.Windows.Window
    {
        private Coolso user;
        private Save_Data save;
        public ObservableCollection<DataItem>[] Data_Array { get; set; }
        private DispatcherTimer chart_timer;

        private DateTime startTime;
        private double chart_interval_seconds;
        private int totalseconds;

        private Camera camera;
        private VideoCapture capture;
        private Mat frame;
        private List<Mat> frame_list;
        private double cap_interval_seconds;
        private List<object[]> data_list;
        private bool button_start;
        private int counter;

        private bool Window_running;
        private Thread renderingThread;

        public MainWindow()
        {
            InitializeComponent();
            save = new Save_Data();
            user = new Coolso();
            init();
            this.Closed += MainWindow_Closed;
            Data_Array = new ObservableCollection<DataItem>[6];
            for (int i = 0; i < 6; i++)
            {
                Data_Array[i] = new ObservableCollection<DataItem>();
            }
            vertical_chart.ItemsSource = Data_Array[0];
            horizontal_chart.ItemsSource = Data_Array[1];
            Rotational_chart.ItemsSource = Data_Array[2];
            Duction_chart.ItemsSource = Data_Array[3];
            Flexion_chart.ItemsSource = Data_Array[4];
            Strength_chart.ItemsSource = Data_Array[5];
            totalseconds = 10;

            frame = new Mat();
            //相機每1秒做截圖照片
            capture = new VideoCapture(0); // 選擇相機，0代表第一個相機
            renderingThread = new Thread(() =>
            {
                try
                {
                    while (Window_running)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            CompositionTarget_Rendering(null, EventArgs.Empty);
                        });
                        // Add a delay if needed to control the frame rate
                        Thread.Sleep(16); // Adjust delay according to desired frame rate (e.g., for 60 fps, use 16ms)

                    }
                }
                catch (Exception ex)
                {
                    Window_running = false;
                    Thread.CurrentThread.Abort();
                }
            });
            renderingThread.Start();
            
            //圖表每0.01秒更新一次
            chart_timer = new DispatcherTimer();
            chart_timer.Tick += timer_Tick;
            chart_interval_seconds = 0.01;
            chart_timer.Interval = TimeSpan.FromSeconds(chart_interval_seconds);

        }
        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            capture.Read(frame);
            Mat mat = frame.Clone();
            if (!frame.Empty())
            {
                
                BitmapSource bitmapSource = frame.ToBitmapSource();
                // Use Dispatcher to update UI elements from a different thread
                Dispatcher.Invoke(() =>
                {
                    image.Source = bitmapSource;
                });
                if (button_start)                
                    counter++;
                if (counter % 15 == 0 && button_start)
                {
                    Debug.WriteLine(frame_list.Count);
                    frame_list.Add(mat);
                    data_list.Add(new object[] { user.get_VerticalRate_data(), user.get_HorizontalRate_data(), user.get_RotationRate_data(), user.get_DuctionRate_data(), user.get_FlexionRate_data(), user.get_StrengthAmplitude_data(), 0 });
                }


            }
        }
        private void start_button_click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 6; i++)
            {
                Data_Array[i].Clear();
            }
            user.activate_coolso();
            startTime = DateTime.Now;
            chart_timer.Start();
            button_start = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            TimeSpan record_seconds = DateTime.Now - startTime;
            if (record_seconds.TotalSeconds > totalseconds) {
                user.exit_coolso(); 
                chart_timer.Stop();
                button_start = false;
                List<Mat> listCopy = new List<Mat>(frame_list);
                camera = new Camera(listCopy);
                camera.ShowDialog();
                save.add_item(data_list ,camera.get_strings());
                init();
            }
            else
            {   
                Data_Array[0].Add(new DataItem { Data = user.get_VerticalRate_data(), Time = record_seconds.TotalSeconds });
                Data_Array[1].Add(new DataItem { Data = user.get_HorizontalRate_data(), Time = record_seconds.TotalSeconds });
                Data_Array[2].Add(new DataItem { Data = user.get_RotationRate_data(), Time = record_seconds.TotalSeconds });
                Data_Array[3].Add(new DataItem { Data = user.get_DuctionRate_data(), Time = record_seconds.TotalSeconds });
                Data_Array[4].Add(new DataItem { Data = user.get_FlexionRate_data(), Time = record_seconds.TotalSeconds });
                Data_Array[5].Add(new DataItem { Data = user.get_StrengthAmplitude_data(), Time = record_seconds.TotalSeconds });
            }
            
        }
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            chart_timer.Stop();
            Window_running = false;
        }
        private void init()
        {   
            Window_running = true;
            button_start = false;
            counter = 0;
            frame_list = new List<Mat>();
            data_list = new List<object[]>();

        }
    }
   
    public class DataItem : INotifyPropertyChanged
    {
        private double _data;
        public double Data
        {
            get { return _data; }
            set
            {
                if (_data != value)
                {
                    _data = value;
                    OnPropertyChanged("Data");
                }
            }
        }

        private double _time;
        public double Time
        {
            get { return _time; }
            set
            {
                if (_time != value)
                {
                    _time = value;
                    OnPropertyChanged("Time");
                }
            }
        }

        public event PropertyChangedEventHandler ?PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}