using KalashnikovGroupApp.Models;
using KalashnikovGroupApp.Servises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KalashnikovGroupApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для EmployeesPageAdd.xaml
    /// </summary>
    public partial class EmployeesPageAdd : Page
    {
        private readonly ApiService _apiService;
        private readonly Employees _employees;
        internal EmployeesPageAdd(Employees employees = null)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _employees = employees ?? new Employees();

            TBoxmail.Text = _employees.mail;
            TBoxpassword.Text = _employees.password;
            TBoxname.Text = _employees.name;
            TBoxsurname.Text = _employees.surname;
            TBoxmiddlename.Text = _employees.middlename;
            TBoxwagerate.Text = _employees.wage_rate.ToString();
            TBoxpost.Text = _employees.Postid_post.ToString();

            

        }

        private async void EmployeesSaveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _employees.mail = TBoxmail.Text;
                _employees.password = TBoxpassword.Text;
                _employees.name = TBoxname.Text;
                _employees.surname = TBoxsurname.Text;
                _employees.middlename = TBoxmiddlename.Text;
                _employees.wage_rate = float.Parse(TBoxwagerate.Text);
                _employees.Postid_post = int.Parse(TBoxpost.Text);

                if (_employees.id_employess == 0)
                {
                    await _apiService.CreateEmployees(_employees);
                    MessageBox.Show("Сотрудник успешно добавлен");
                }
                else
                {
                    await _apiService.UpdateEmployees(_employees);
                    MessageBox.Show("Сотрудник успешно обновлен");
                }
                NavigationService.Navigate(new EmployeesPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранеии сотрудника: {ex.Message}");
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EmployeesPage());
        }
    }
}
