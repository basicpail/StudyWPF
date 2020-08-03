using MahApps.Metro.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows.Threading;

namespace WPFPrac
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        SerialPort serial;
        private short xCount = 200;
        private short maxPhotoVal = 1023;
        List<SensorData> photoDatas = new List<SensorData>();

        string strConnString = "Server=localhost;Port=3306;" +
            "Database=iot_sensordata;Uid=root;Pwd=mysql_p@ssw0rd";

        public bool IsSimulation { get; set; }

        //Timer timer = new Timer();
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

        private void CboSerialPort_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var portName = CboSerialPort.SelectedItem.ToString();
            //var portName = "com1";
            serial = new SerialPort(portName);
            serial.DataReceived += Serial_DataReceived;

            BtnConnect.IsEnabled = true;
        }

        private void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string sVal = serial.ReadLine();
            this.BeginInvoke(new Action(delegate { DisplayValue(sVal); }));
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

        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }

            return DateTime.Now;
        }

        private void DisplayValue(string sVal)
        {
            try
            {
                ushort v = ushort.Parse(sVal);
                if (v < 0 || v > maxPhotoVal)
                    return;

                SensorData data = new SensorData(DateTime.Now, v);
                photoDatas.Add(data);
                InsertDataToDB(data);

                TxtSensorCount.Text = photoDatas.Count.ToString();
                PgbPhotoRegistor.Value = v;
                LblPhotoRegistor.Text = v.ToString();

                string item = $"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}\t{v}";

                RtbLog.AppendText($"{item}\n");
                RtbLog.ScrollToEnd(); //ScrollToCaret()

                for (int i = 0; i < 200; i++)
                {
                    var x = new string[] { };
                    x.Append(v.ToString());
                    var y = new string[] { };
                    y.Append($"{DateTime.Now.ToString("hh:mm:ss")}");
                    linegraph.Plot(x, y);
                    //y[i] = 3.1415 * i / (y.Length - 1);
                    //x[i] = DateTime.Now.AddMinutes(-i).ToOADate();
                    Delay(100);
                    linegraph.Plot(x, y);

                }

                

                //ChtSensorValues.Series[0].Points.Add(v);

                //ChtSensorValues.ChartAreas.AxisX.Minimum = 0;
                //ChtSensorValues.ChartAreas[0].AxisX.Maximum =
                //    (photoDatas.Count >= xCount) ? photoDatas.Count : xCount;

                //if (photoDatas.Count > xCount)
                //    ChtSensorValues.ChartAreas[0].AxisX.ScaleView.Zoom(
                //        photoDatas.Count - xCount, photoDatas.Count);
                //else
                //    ChtSensorValues.ChartAreas[0].AxisX.ScaleView.Zoom(0, xCount);

                //if (IsSimulation == false)
                //    BtnPortValue.Text = $"{serial.PortName}\n{sVal}";
                //else
                //    BtnPortValue.Text = $"{sVal}";
            }
            catch (Exception ex)
            {
                RtbLog.AppendText($"Error : {ex.Message}\n");
                RtbLog.ScrollToEnd();
            }
        }



        private void InsertDataToDB(SensorData data)
        {
            string strQuery = "INSERT INTO sensordatatbl " +
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

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            //serial.Open();
            LblConnectionTime.Text = $"연결시간 : {DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}";
            BtnConnect.IsEnabled = false;
            BtnDisconnect.IsEnabled = true;
        }

        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            //serial.Close();
            BtnConnect.IsEnabled = true;
            BtnDisconnect.IsEnabled = false;
        }

       

        private void MenuSubItemExit_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void MenuSubItemStart_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            IsSimulation = true;
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += Timer_Tick;
            timer.Start();

            // serial통신 끊기
            BtnDisconnect_Click(sender, e);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ushort value = (ushort)rand.Next(1, 1024);
            DisplayValue(value.ToString());
        }

        private void MenuSubItemStop_Click(object sender, System.Windows.RoutedEventArgs e)
        { 
            timer.Stop();
            IsSimulation = false;

            // serial 통신 재시작
            BtnConnect_Click(sender, e);
        }

       
    }
}
