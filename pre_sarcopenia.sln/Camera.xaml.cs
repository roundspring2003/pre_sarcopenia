using Mysqlx.Notice;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace pre_sarcopenia.sln
{
   
    public partial class Camera : System.Windows.Window
    {

        private List<Mat> frame_list;
        private List<string> strings;
        private int number;
        public Camera(List<Mat> list)
        {
            InitializeComponent();
            frame_list = list;
            strings = new List<string>();
            
            number = 0;
            image.Source = frame_list[number].ToBitmapSource();
            box.KeyDown += new KeyEventHandler(next_picture); ;   
        }
        private void next_picture(object sender, KeyEventArgs e)
        {   
            if (e.Key == Key.Enter)
            {   
                if (string.IsNullOrEmpty(box.Text)) {
                    System.Windows.MessageBox.Show("Please input number！", "Warning", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }
                number++;
                string input = box.Text;
                strings.Add(input);
                box.Clear();
                if (number < frame_list.Count)
                    image.Source = frame_list[number].ToBitmapSource();
                else
                {
                    Close();
                }
            }
        }
        public List<string> get_strings()
        {
            return strings;
        }
    }
}
