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
    /// Логика взаимодействия для DealPageAdd.xaml
    /// </summary>
    public partial class DealPageAdd : Page
    {
        private readonly ApiService _apiService;
        private readonly Deal _deal;
        internal DealPageAdd(Deal deal = null)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _deal = deal ?? new Deal();

            TBoxdate.Text = _deal.date.ToString();
            TBoxquality.Text = _deal.quality.ToString();
            TBoxtotal_amount.Text = _deal.total_amount.ToString();
            TBoxOperationsid_operations.Text = _deal.Operationsid_operations.ToString();
            TBoxEmployeesid_employess.Text = _deal.Employeesid_employess.ToString();
        }

        private async void DealSaveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _deal.date = DateTime.Parse(TBoxdate.Text);
                _deal.quality = int.Parse(TBoxquality.Text);
                _deal.total_amount = float.Parse(TBoxtotal_amount.Text);
                _deal.Operationsid_operations = int.Parse(TBoxOperationsid_operations.Text);
                _deal.Employeesid_employess = int.Parse(TBoxEmployeesid_employess.Text);

                if (_deal.id_deal == 0)
                {
                    await _apiService.CreateDeal(_deal);
                    MessageBox.Show("Дело успешно добавлен");
                }
                else
                {
                    await _apiService.UpdateDeal(_deal);
                    MessageBox.Show("Дело успешно обновлен");
                }
                NavigationService.Navigate(new DealPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранеии дела: {ex.Message}");
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DealPage());
        }
    }
}
