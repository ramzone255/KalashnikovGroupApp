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
    /// Логика взаимодействия для SingInPage.xaml
    /// </summary>
    public partial class SingInPage : Page
    {
        private readonly ApiService _apiService;
        public SingInPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void SingInClick(object sender, RoutedEventArgs e)
        {
            var mail = TBoxMail.Text;
            var password = TBoxPassword.Text;

            try
            {
                var employees = await _apiService.AuthenticateAsync(mail, password);
                if (employees != null)
                {
                    NavigationService.Navigate(new EmployeesPage());
                }
                else
                {
                    MessageBox.Show("Invalid mail or password");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
