using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMChartApp.ViewModels
{
    internal class MainViewModel : Conductor<object>
    {
        public void ExitProgram()
        {
            Environment.Exit(0);
        }

        public void LoadLineChart()
        {
            ActivateItem(new LineChartViewModel()); //ActiveItem은 속성 ActivateItem은 속성
        }

        public void LoadGaugeChart()
        {
            ActivateItem(new GaugeChartViewModel());
        }
    }
}
