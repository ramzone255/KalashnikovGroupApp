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
    /// Логика взаимодействия для PaydayPageAdd.xaml
    /// </summary>
    public partial class PaydayPageAdd : Page
    {
        private readonly ApiService _apiService;
        private readonly Payday _payday;
        internal PaydayPageAdd(Payday payday = null)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _payday = payday ?? new Payday();

            TBoxpayday.Text = _payday.paycheck.ToString();
            TBoxstart.Text = _payday.start_date.ToString();
            TBoxend.Text = _payday.end_date.ToString();
        }

        private async void PaydaySaveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _payday.paycheck = float.Parse(TBoxpayday.Text);
                _payday.start_date = DateTime.Parse(TBoxstart.Text);
                _payday.end_date = DateTime.Parse(TBoxend.Text);

                if (_payday.id_payday == 0)
                {
                    await _apiService.CreatePayday(_payday);
                    MessageBox.Show("Зарплата успешно добавлен");
                }
                else
                {
                    await _apiService.UpdatePayday(_payday);
                    MessageBox.Show("Зарплата успешно обновлен");
                }
                NavigationService.Navigate(new PaydayPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранеии зарплаты: {ex.Message}");
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PaydayPage());
        }
    }
}
