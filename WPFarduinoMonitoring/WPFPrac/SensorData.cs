using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFPrac
{
    public class SensorData
    {

        public DateTime Date { get; set; }
        public Int32 Value { get; set; }

        public SensorData(DateTime date, Int32 value)
        {
            Date = date;
            Value = value;
        }
    }
}
