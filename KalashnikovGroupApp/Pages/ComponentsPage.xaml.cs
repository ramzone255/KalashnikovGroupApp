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
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace KalashnikovGroupApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ComponentsPage.xaml
    /// </summary>
    public partial class ComponentsPage : Page
    {
        private readonly ApiService _apiService;

        private List<Components> _allComponents;
        public ComponentsPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadComponents();
        }
        private async Task LoadComponents()
        {
            try
            {
                _allComponents = await _apiService.GetComponents();
                ComponentsListView.ItemsSource = _allComponents;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки продуктов: {ex.Message}");
            }
        }

        private void EmployeesClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EmployeesPage());
        }

        private void ComponentsClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void DealClick(object sender, RoutedEventArgs e)
        {

        }

        private void PaydayClick(object sender, RoutedEventArgs e)
        {

        }

        private void ExcelClick(object sender, RoutedEventArgs e)
        {
            var ExcelApp = new Excel.Application();

            Excel.Workbook wb = ExcelApp.Workbooks.Add();

            Excel.Worksheet worksheet = ExcelApp.Worksheets.Item[1];

            int indexRows = 1;

            worksheet.Cells[2][indexRows] = "Наименование";

            var printItems = ComponentsListView.Items;

            foreach (Components item in printItems)
            {
                worksheet.Cells[1][indexRows + 1] = indexRows;
                worksheet.Cells[2][indexRows + 1] = item.denomination;

                indexRows++;
            }
            Excel.Range range = worksheet.Range[worksheet.Cells[2][indexRows + 1],
                    worksheet.Cells[2][indexRows + 1]];

            range.ColumnWidth = 20;

            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

            ExcelApp.Visible = true;
        }

        private async void PDFClick(object sender, RoutedEventArgs e)
        {
            _allComponents = await _apiService.GetComponents();
            var ComponentsInPDF = _allComponents;

            var ComponentsApplicationPDF = new Word.Application();

            Word.Document document = ComponentsApplicationPDF.Documents.Add();

            Word.Paragraph empParagraph = document.Paragraphs.Add();
            Word.Range empRange = empParagraph.Range;
            empRange.Text = "Components";
            empRange.Font.Bold = 4;
            empRange.Font.Italic = 4;
            empRange.Font.Color = Word.WdColor.wdColorBlack;
            empRange.InsertParagraphAfter();

            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table paymentsTable = document.Tables.Add(tableRange, ComponentsInPDF.Count() + 1, 1);
            paymentsTable.Borders.InsideLineStyle = paymentsTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            paymentsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            Word.Range cellRange;

            cellRange = paymentsTable.Cell(1, 1).Range;
            cellRange.Text = "Наименование";


            paymentsTable.Rows[1].Range.Bold = 1;
            paymentsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            for (int i = 0; i < ComponentsInPDF.Count(); i++)
            {
                var ProductCurrent = ComponentsInPDF[i];

                cellRange = paymentsTable.Cell(i + 2, 1).Range;
                cellRange.Text = ProductCurrent.denomination;
            }

            ComponentsApplicationPDF.Visible = true;

            document.SaveAs2(@"C:\Users\User\OneDrive\Desktop\KalashnikovGroupApp\KalashnikovGroupApp\Files\Components.pdf", Word.WdExportFormat.wdExportFormatPDF);
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ComponentsPageAdd());
        }

        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            if (ComponentsListView.SelectedItem is Components selectedComponents)
            {
                NavigationService.Navigate(new ComponentsPageAdd(selectedComponents));
            }
            else
            {
                MessageBox.Show("Выберите компонент для редактирования");
            }
        }

        private async void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (ComponentsListView.SelectedItem is Components selectedComponents)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить компонент {selectedComponents.denomination}?", "Подтверждение удаления", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _apiService.DeleteСomponents(selectedComponents.id_components);
                        MessageBox.Show("Компонент успешно удален.");
                        await LoadComponents();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении компонент: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите компонент для удаления.");
            }
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            var filteredComponents = _allComponents.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(TbSerch.Text))
            {
                var filterText = TbSerch.Text.ToLowerInvariant();
                filteredComponents = filteredComponents.Where(p => p.denomination.ToLowerInvariant().Contains(filterText));
            }
            ComponentsListView.ItemsSource = filteredComponents.ToList();
        }

        private void TbSerch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
