using KalashnikovGroupApp.Servises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KalashnikovGroupApp.Models;
using KalashnikovGroupApp.Servises;

namespace KalashnikovGroupApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для EmployeesPage.xaml
    /// </summary>
    public partial class EmployeesPage : Page
    {
        private readonly ApiService _apiService;

        private List<Employees> _allEmployees;
        public EmployeesPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadEmployees();
        }
        private async Task LoadEmployees()
        {
            try
            {
                var employees = await _apiService.GetEmployees();
                EmployeesListView.ItemsSource = employees;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки продуктов: {ex.Message}");
            }
        }

        private void EmployeesClick(object sender, RoutedEventArgs e)
        {

        }

        private void PaydayClick(object sender, RoutedEventArgs e)
        {

        }

        private void DealClick(object sender, RoutedEventArgs e)
        {

        }

        private void ComponentsClick(object sender, RoutedEventArgs e)
        {

        }

        private async void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (EmployeesListView.SelectedItem is Employees selectedEmployees)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить сотрудника {selectedEmployees.surname}?", "Подтверждение удаления", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _apiService.DeleteEmployees(selectedEmployees.id_employess);
                        MessageBox.Show("Сотрудник успешно удален.");
                        await LoadEmployees();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении сотрудника: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите сотрудника для удаления.");
            }
        }

        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            if (EmployeesListView.SelectedItem is Employees selectedEmployees)
            {
                NavigationService.Navigate(new EmployeesPageAdd(selectedEmployees));
            }
            else
            {
                MessageBox.Show("Выберите сотрудника для редактирования");
            }
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EmployeesPageAdd());
        }

        private void TbSerch_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
    }
}
