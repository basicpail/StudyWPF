using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFChart
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //private static DateTime Delay(int MS)
        //{
        //    DateTime ThisMoment = DateTime.Now;
        //    TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
        //    DateTime AfterWards = ThisMoment.Add(duration);

        //    while (AfterWards >= ThisMoment)
        //    {
        //        System.Windows.Forms.Application.DoEvents();
        //        ThisMoment = DateTime.Now;
        //    }

        //    return DateTime.Now;
        //}
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //var x = Enumerable.Range(0, 1000).Select(i => i / 10.0).ToArray();
            //var y = x.Select(v => Math.Abs(v) < 1e-10 ? 1 : Math.Sin(v) / v).ToArray();


            //var x = new int[10];
            //var y = new int[10];
            //var i=0;

            //linegraph.Plot(x, y);

            //while (i<3)
            //{
            //    x[i] = 10;
            //    y[i] = 10;
            //    i++;
            //    linegraph.Plot(x, y);

            //    Delay(1000);
            //}

            var x = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90 };
            var y = new int[] { 10, 36, 57, 32, 22, 77, 86, 55, 77 };
            linegraph.Plot(x, y);
            linegraph.
        }

        
    }
}
