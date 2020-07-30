using Caliburn.Micro;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ThirdCaliburnApp.Models;

namespace ThirdCaliburnApp.ViewModels
{
    class MainViewModel : Conductor<object>, IHaveDisplayName
    {
        //EmployeesModel employeesModel;

        int id;

        public int Id
        {
            get => id;
            set
            {
                id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }
        string empName;

        public string EmpName 
        { 
            get=>empName;
            set 
            {
                empName = value;
                NotifyOfPropertyChange(() => EmpName);
            } 
        }
        decimal salary;

        public decimal Salary 
        { 
            get=>salary; 
            set
            {
                salary = value;
                NotifyOfPropertyChange(() => Salary);
            } 
        }
        string deptName;

        public string DeptName 
        { 
            get=>deptName; 
            set
            {
                deptName = value;
                NotifyOfPropertyChange(() => DeptName);
            }
                }
        string destination;

        public string Destination 
        { 
            get=>destination;
            set 
            {
                destination = value;
                NotifyOfPropertyChange(() => Destination);
            }
         }

        //전체 Employees리스트
        BindableCollection<EmployeesModel> employees;

        public BindableCollection<EmployeesModel> Employees 
        {
            get=>employees;
            set 
            {
                employees = value;
                NotifyOfPropertyChange(() => Employees);
            } 
        }
        
        //리스트 중 선택된 하나의 Employee
        EmployeesModel selectedEmployee;

        public EmployeesModel SelectedEmployee
        {
            get => selectedEmployee;
            set
            {
                selectedEmployee = value;

                Id = value.Id;
                EmpName = value.EmpName;
                Salary = value.Salary;
                DeptName = value.DeptName;
                Destination = value.Destination;

                NotifyOfPropertyChange(() => SelectedEmployee);
                //NotifyOfPropertyChange(() => Id);
            }
        }
        //SelectedEmployee
        public MainViewModel()
        {
            GetEmployees();
        }

        public void NewEmployee()
        {
            Id = 0;
            Salary = 0;
            DeptName = Destination = EmpName = string.Empty;
        }

        public void GetEmployees()
        {
            using(MySqlConnection conn = new MySqlConnection(Commons.CONNSTRING))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(EmployeesTbl.SELECT_EMPLOYEES,conn);
                //cmd.Connection = conn;
                MySqlDataReader reader = cmd.ExecuteReader();
                Employees = new BindableCollection<EmployeesModel>();
                while(reader.Read())
                {
                    var temp = new EmployeesModel
                    {
                        Id = (int)reader["id"],
                        EmpName = reader["EmpName"].ToString(),
                        Salary = (decimal)reader["Salary"],
                        DeptName = reader["DeptName"].ToString(),
                        Destination = reader["Destination"].ToString()
                    };
                    Employees.Add(temp);
                }
            }
        }

        //public bool CanSaveEmployee
        //{
        //    get
        //    {
        //        return !((Id == 0) && (string.IsNullOrEmpty(EmpName))
        //            && (Salary == 0) && (string.IsNullOrEmpty(DeptName))
        //            && (string.IsNullOrEmpty(Destination)));
        //    }
        //}

        public void SaveEmployee()
        {
            int resultRow = 0;

            using (MySqlConnection conn = new MySqlConnection(Commons.CONNSTRING))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(EmployeesTbl.UPDATE_EMPLOYEE, conn);
                MySqlParameter paramEmpName = new MySqlParameter("@EmpName", MySqlDbType.VarChar, 45);
                paramEmpName.Value = EmpName;
                cmd.Parameters.Add(paramEmpName);

                MySqlParameter paramSalary = new MySqlParameter("@Salary", MySqlDbType.Decimal);
                paramSalary.Value = Salary;
                cmd.Parameters.Add(paramSalary);

                MySqlParameter paramDeptName = new MySqlParameter("@DeptName", MySqlDbType.VarChar, 45);
                paramDeptName.Value = DeptName;
                cmd.Parameters.Add(paramDeptName);

                MySqlParameter paramDestination = new MySqlParameter("@Destination", MySqlDbType.VarChar, 45);
                paramDestination.Value = Destination;
                cmd.Parameters.Add(paramDestination);

                MySqlParameter paramId = new MySqlParameter("@id", MySqlDbType.Int32);
                paramId.Value = Id;
                cmd.Parameters.Add(paramId);

                resultRow = cmd.ExecuteNonQuery();
            }

            if(resultRow>0)
            {
                MessageBox.Show("저장했습니다.");

                GetEmployees();
            }
        }
    }
}
