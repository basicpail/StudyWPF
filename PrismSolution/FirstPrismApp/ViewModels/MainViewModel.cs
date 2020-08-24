using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FirstPrismApp.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private string dispalyName;
        public string DispalyName
        {
            get => dispalyName;
            set => SetProperty(ref dispalyName, value);
        }

        //IsEnabled,ExcuteCommand,UpdateText
        private bool isEnabled;

        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                SetProperty(ref isEnabled, value);
                ExecuteCommand.RaiseCanExecuteChanged();
            }
        }

        private string updateText;

        public string UpdateText
        {
            get => updateText;
            set => SetProperty(ref updateText, value);
        }

        public DelegateCommand ExecuteCommand { get; set; }

        public MainViewModel()
        {
            DispalyName = "Prism App";

            ExecuteCommand = new DelegateCommand(Execute, CanExecute);
        }

        private void Execute()
        {
            UpdateText = $"Updated:{DateTime.Now}";
        }

        private bool CanExecute()
        {
            return IsEnabled;
        }
    }
}
