using Caliburn.Micro;
using System.Windows;
using ThirdCaliburnApp.ViewModels;

namespace ThirdCaliburnApp
{
    public class bootstrapper : BootstrapperBase
    {
        public bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }
    }
}
