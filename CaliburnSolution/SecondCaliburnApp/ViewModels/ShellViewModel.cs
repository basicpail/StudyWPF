using Caliburn.Micro;
using MySql.Data.MySqlClient;
using SecondCaliburnApp.Helpers;
using SecondCaliburnApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondCaliburnApp.ViewModels
{
    class ShellViewModel : Conductor<object>, IHaveDisplayName
    {
        public override string DisplayName { get; set; }

        string firstName;

        public string FirstName 
        {
            get=>firstName; 
            set
            {
                firstName = value;
                NotifyOfPropertyChange(() => FirstName);
                NotifyOfPropertyChange(() => FullName);
                NotifyOfPropertyChange(() => CanClearName);
            }
        }

        string lastName;

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                NotifyOfPropertyChange(() => lastName);
                NotifyOfPropertyChange(() => FullName);
                NotifyOfPropertyChange(() => CanClearName);
            }
        }

        public string FullName
        {
            get => $"{FirstName} {LastName}";
        }

        public ShellViewModel()
        {
            DisplayName = "Second Cliburn App";
            //FirstName = "SG";
            //LastName = "SONG";

            InitCombobox();
            //People = new BindableCollection<PersonModel>();
            //People.Add(new PersonModel { LastName = "SONG", FirstName = "SG" });
            //People.Add(new PersonModel { LastName = "Gates", FirstName = "Bill" });
            //People.Add(new PersonModel { LastName = "Jobs", FirstName = "Steave" });
            //People.Add(new PersonModel { LastName = "Musk", FirstName = "Elon" });
        }

        private void InitCombobox()
        {
            using (MySqlConnection conn = new MySqlConnection(Commons.STRCONNSTRING))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(Commons.SELECTPEOPLEQUERY, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    var temp = new PersonModel
                    {
                        FirstName = reader["firstname"].ToString(),
                        LastName = reader["lastname"].ToString()
                    };
                    People.Add(temp);
                }
             }
        }

        //콤보박스에 들어갈 사람 리스트
        public BindableCollection<PersonModel> People { get; set; }

        PersonModel selectedPerson;

        public PersonModel SelectedPerson
        {
            get => selectedPerson;
            set
            {
                selectedPerson = value;
                this.LastName = selectedPerson.LastName;
                this.FirstName = selectedPerson.FirstName;
                NotifyOfPropertyChange(() => SelectedPerson);
                NotifyOfPropertyChange(() => CanClearName);
            }
        }
        public void ClearName()
        {
            this.FirstName = this.LastName = string.Empty;
        }

        //public bool CanClearName(string firstName, string lastName)
        //{
        //    if(string.IsNullOrEmpty(firstName)&&string.IsNullOrEmpty(lastName))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
        public bool CanClearName
        {
            get=> !(string.IsNullOrEmpty(FirstName)
                    && string.IsNullOrEmpty(LastName));
        }

        public void LoadPageOne()
        {//UserControl FirstChildView
            ActivateItem(new FirstChildViewModel());

        }

        public void LoadPageTwo()
        {//UserControl SecondChildView
            ActivateItem(new SecondChildViewModel());
        }
    }
}
