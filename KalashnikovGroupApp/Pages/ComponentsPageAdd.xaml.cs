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
    /// Логика взаимодействия для ComponentsPageAdd.xaml
    /// </summary>
    public partial class ComponentsPageAdd : Page
    {
        private readonly ApiService _apiService;
        private readonly Components _components;
        internal ComponentsPageAdd(Components components = null)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _components = components ?? new Components();

            TBoxdenomination.Text = _components.denomination;
        }

        private async void ComponentsSaveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _components.denomination = TBoxdenomination.Text;

                if (_components.id_components == 0)
                {
                    await _apiService.CreateComponents(_components);
                    MessageBox.Show("Компонент успешно добавлен");
                }
                else
                {
                    await _apiService.UpdateComponents(_components);
                    MessageBox.Show("Компонент успешно обновлен");
                }
                NavigationService.Navigate(new ComponentsPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранени компонента: {ex.Message}");
            }

        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ComponentsPage());
        }
    }
}
