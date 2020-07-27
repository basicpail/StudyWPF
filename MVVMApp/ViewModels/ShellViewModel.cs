using MVVMApp.Models;
using System;
using System.Windows;
using System.Windows.Input;
using WpfMvvmApp.ViewModels;

namespace MVVMApp.ViewModels
{
    public class ShellViewModel:ViewModelBase
    {
        #region 멤버변수/속성영역
        string inLastName;
        string inFirstName;
        string inEmail;
        DateTime? inDate;

        string outLastName;
        string outFirstName;
        string outEmail;
        string outDate;
        string outAdult;
        string outBirthday;
        string outChnZodiac; //추가
        string outCalcZodiac;

        public string InLastName
        {
            get => inLastName;
            set
            {
                inLastName = value;
                RaisePropertyChanged("InLastName");
            }
        }
        public string InFirstName
        {
            get => inFirstName;
            set
            {
                inFirstName = value;
                RaisePropertyChanged("InFirstName");
            }
        }
        public string InEmail
        {
            get => inEmail;
            set
            {
                inEmail = value;
                RaisePropertyChanged("InEmail");
            }
        }
        public DateTime? InDate
        {
            get => inDate;
            set
            {
                inDate = value;
                RaisePropertyChanged("InDate");
            }
        }
        public string OutLastName
        {
            get => outLastName;
            set
            {
                outLastName = value;
                RaisePropertyChanged("OutLastName");
            }
        }
        public string OutFirstName
        {
            get => outFirstName;
            set
            {
                outFirstName = value;
                RaisePropertyChanged("OutFirstName");
            }
        }
        public string OutEmail
        {
            get => outEmail;
            set
            {
                outEmail = value;
                RaisePropertyChanged("OutEmail");
            }
        }
        public string OutDate
        {
            get => outDate;
            set
            {
                outDate = value;
                RaisePropertyChanged("OutDate");
            }
        }
        public string OutAdult
        {
            get => outAdult;
            set
            {
                outAdult = value;
                RaisePropertyChanged("OutAdult");
            }
        }
        public string OutBirthday
        {
            get => outBirthday;
            set
            {
                outBirthday = value;
                RaisePropertyChanged("OutBirthday");
            }
        }

        public string OutChnZodiac //20200727 16:44신규추가 : 띠 추가
        {
            get => outChnZodiac;
            set
            {
                outChnZodiac = value;
                RaisePropertyChanged("OutChnZodiac");
            }
        }
        public string OutCalcZodiac
        {
            get => outCalcZodiac;
            set
            {
                outCalcZodiac = value;
                RaisePropertyChanged("OutCalcZodiac");
            }
        }
        #endregion

        ICommand clickCommand;

        public ICommand ClickCommand => clickCommand ?? (clickCommand = new RelayCommand<Object>(o => Click(), o => IsClick()));

        private void Click()
        {
            try
            {
                DateTime date = InDate ?? DateTime.Now;
                Person person = new Person(InFirstName, inLastName, InEmail, date);

                OutLastName = person.LastName;
                OutFirstName = person.FirstName;
                OutEmail = person.Email;
                OutDate = person.Date.ToShortDateString();
                OutAdult = $"{person.IsAdult}";
                OutBirthday = $"{person.IsBirthDay}";
                OutChnZodiac = person.ChnZodiac; //추가
                OutCalcZodiac = person.CalcZodiac;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error : {ex.Message}");
            }
        }

        private bool IsClick()
        {
            return (!string.IsNullOrEmpty(InLastName)) &&
                !string.IsNullOrEmpty(InFirstName) &&
                !string.IsNullOrEmpty(InEmail) &&
                !string.IsNullOrEmpty(InDate.ToString());
        }
    }
}
