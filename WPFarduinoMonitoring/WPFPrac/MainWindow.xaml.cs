using MahApps.Metro.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace WPFPrac
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        SerialPort serial;
        private short maxPhotoVal = 1023;
        List<SensorData> photoDatas = new List<SensorData>();
        List<int> x = new List<int>();
        List<int> y = new List<int>();
        int t = 0;

        string strConnString = " Server=localhost;Port=3306; " +
                               " Database=iot_sensordata; " +
                               " Uid=root;Pwd=mysql_p@ssw0rd ";

        public bool IsSimulation { get; set; }

        DispatcherTimer timer = new DispatcherTimer();
        Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();
            InitControls();
        }

        private void InitControls()
        {
            foreach (var item in SerialPort.GetPortNames())
            {
                CboSerialPort.Items.Add(item);
            }
            CboSerialPort.Text = "Select Port";

            PgbPhotoRegistor.Minimum = 0;
            PgbPhotoRegistor.Maximum = maxPhotoVal;

            BtnConnect.IsEnabled = BtnDisconnect.IsEnabled = false;

        }

        private void CboSerialPort_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var portName = CboSerialPort.SelectedItem.ToString();
            serial = new SerialPort(portName);
            serial.BaudRate = 9600;
            serial.DataReceived += Serial_DataReceived;

            BtnConnect.IsEnabled = true;
        }

        private void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string sVal = serial.ReadLine();
            this.BeginInvoke(new Action(delegate { DisplayValue(sVal); }));
        }


        private void DisplayValue(string sVal)
        {
            try
            {
                Int32 v = Int32.Parse(sVal);
                if (v < 0 || v > maxPhotoVal)
                    return;

                SensorData data = new SensorData(DateTime.Now, v);
                photoDatas.Add(data);
                InsertDataToDB(data);

                TxtSensorCount.Text = photoDatas.Count.ToString()+" sec";
                PgbPhotoRegistor.Value = v;
                LblPhotoRegistor.Text = v.ToString();

                string item = $"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}\t{v}";

                RtbLog.AppendText($"{item}\n");
                RtbLog.ScrollToEnd(); //ScrollToCaret()

                x.Add(t);
                y.Add(v);
               
                if(x.Count == 8)
                {
                    x.RemoveAt(0);
                    y.RemoveAt(0);
                }
               
                linegraph.Plot(x, y);

                if (IsSimulation == false)
                    BtnPortValue.Content = $"{CboSerialPort.SelectedItem}\n{sVal}";
                else
                    BtnPortValue.Content = $"{sVal}";
            }
            catch (Exception ex)
            {
                RtbLog.AppendText($"Error : {ex.Message}\n");
                RtbLog.ScrollToEnd();
            }
        }


        private void InsertDataToDB(SensorData data)
        {
            string strQuery = "INSERT INTO sensordatatbl_2 " +
                              " (Date, Value) " +
                              " VALUES " +
                              " (@Date, @Value) ";

            using (MySqlConnection conn = new MySqlConnection(strConnString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(strQuery, conn);
                MySqlParameter paramDate = new MySqlParameter("@Date", MySqlDbType.DateTime)
                {
                    Value = data.Date
                };
                cmd.Parameters.Add(paramDate);
                MySqlParameter paramValue = new MySqlParameter("@Value", MySqlDbType.Int32)
                {
                    Value = data.Value
                };
                cmd.Parameters.Add(paramValue);
                cmd.ExecuteNonQuery();
            }
        }

        private void BtnConnect_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            serial.Open();
            LblConnectionTime.Text = $"연결시간 : {DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}";
            BtnConnect.IsEnabled = false;
            BtnDisconnect.IsEnabled = true;

            if (IsSimulation == true)
            {
                IsSimulation = false;
                timer.Start();
            }
            else
            {
                IsSimulation = false;
                timer.Interval = TimeSpan.FromMilliseconds(800);
                timer.Tick += Timer_Tick2;
                timer.Start();
            }
            
        }


        private void BtnDisconnect_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            serial.Close();
            timer.Stop();
            BtnConnect.IsEnabled = true;
            BtnDisconnect.IsEnabled = false;
        }


        private void MenuSubItemExit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MenuSubItemStart_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            IsSimulation = true;
            timer.Interval = TimeSpan.FromMilliseconds(800);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            t++;
            ushort value = (ushort)rand.Next(20, 21);
            DisplayValue(value.ToString());
        }

        private void Timer_Tick2(object sender, EventArgs e)
        {
            t++;
        }

        private void MenuSubItemStop_Click(object sender, System.Windows.RoutedEventArgs e)
        { 
            //IsSimulation = false;
            timer.Stop();
        }
    }
}
