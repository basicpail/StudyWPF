using Caliburn.Micro;
using System.Windows;

namespace StartCaliburnApp.ViewModels
{
    class ButtonViewModel : Conductor<object>
    {
        public void ButtonClick()
        {
            MessageBox.Show("Basic Button Click");
        }
    }

 
}
