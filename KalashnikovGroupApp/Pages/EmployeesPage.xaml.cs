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
using KalashnikovGroupApp.Models;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;


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
                _allEmployees = await _apiService.GetEmployees();
                EmployeesListView.ItemsSource = _allEmployees;
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
            NavigationService.Navigate(new ComponentsPage());
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

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            var filteredEmployees = _allEmployees.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(TbSerch.Text))
            {
                var filterText = TbSerch.Text.ToLowerInvariant();
                filteredEmployees = filteredEmployees.Where(p => p.surname.ToLowerInvariant().Contains(filterText));
            }
            EmployeesListView.ItemsSource = filteredEmployees.ToList();
        }

        private void ExcelClick(object sender, RoutedEventArgs e)
        {
            var ExcelApp = new Excel.Application();

            Excel.Workbook wb = ExcelApp.Workbooks.Add();

            Excel.Worksheet worksheet = ExcelApp.Worksheets.Item[1];

            int indexRows = 1;

            worksheet.Cells[2][indexRows] = "Почта";
            worksheet.Cells[3][indexRows] = "Пароль";
            worksheet.Cells[4][indexRows] = "Имя";
            worksheet.Cells[5][indexRows] = "Фамилия";
            worksheet.Cells[6][indexRows] = "Отчество";
            worksheet.Cells[7][indexRows] = "Оклад";

            var printItems = EmployeesListView.Items;

            foreach (Employees item in printItems)
            {
                worksheet.Cells[1][indexRows + 1] = indexRows;
                worksheet.Cells[2][indexRows + 1] = item.mail;
                worksheet.Cells[3][indexRows + 1] = item.password;
                worksheet.Cells[4][indexRows + 1] = item.name;
                worksheet.Cells[5][indexRows + 1] = item.surname;
                worksheet.Cells[6][indexRows + 1] = item.middlename;
                worksheet.Cells[7][indexRows + 1] = item.wage_rate;

                indexRows++;
            }
            Excel.Range range = worksheet.Range[worksheet.Cells[2][indexRows + 1],
                    worksheet.Cells[7][indexRows + 1]];

            range.ColumnWidth = 20;

            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

            ExcelApp.Visible = true;
        }

        private async void PDFClick(object sender, RoutedEventArgs e)
        {
            _allEmployees = await _apiService.GetEmployees();
            var EmployeesInPDF = _allEmployees;

            var EmployeesApplicationPDF = new Word.Application();

            Word.Document document = EmployeesApplicationPDF.Documents.Add();

            Word.Paragraph empParagraph = document.Paragraphs.Add();
            Word.Range empRange = empParagraph.Range;
            empRange.Text = "Employees";
            empRange.Font.Bold = 4;
            empRange.Font.Italic = 4;
            empRange.Font.Color = Word.WdColor.wdColorBlack;
            empRange.InsertParagraphAfter();

            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table paymentsTable = document.Tables.Add(tableRange, EmployeesInPDF.Count() + 1, 6);
            paymentsTable.Borders.InsideLineStyle = paymentsTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            paymentsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            Word.Range cellRange;

            cellRange = paymentsTable.Cell(1, 1).Range;
            cellRange.Text = "Почта";
            cellRange = paymentsTable.Cell(1, 2).Range;
            cellRange.Text = "Пароль";
            cellRange = paymentsTable.Cell(1, 3).Range;
            cellRange.Text = "Имя";
            cellRange = paymentsTable.Cell(1, 4).Range;
            cellRange.Text = "Фамилия";
            cellRange = paymentsTable.Cell(1, 5).Range;
            cellRange.Text = "Отчество";
            cellRange = paymentsTable.Cell(1, 6).Range;
            cellRange.Text = "Оклад";


            paymentsTable.Rows[1].Range.Bold = 1;
            paymentsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            for (int i = 0; i < EmployeesInPDF.Count(); i++)
            {
                var ProductCurrent = EmployeesInPDF[i];

                cellRange = paymentsTable.Cell(i + 2, 1).Range;
                cellRange.Text = ProductCurrent.mail;

                cellRange = paymentsTable.Cell(i + 2, 2).Range;
                cellRange.Text = ProductCurrent.password;

                cellRange = paymentsTable.Cell(i + 2, 3).Range;
                cellRange.Text = ProductCurrent.name;

                cellRange = paymentsTable.Cell(i + 2, 4).Range;
                cellRange.Text = ProductCurrent.surname;

                cellRange = paymentsTable.Cell(i + 2, 5).Range;
                cellRange.Text = ProductCurrent.middlename;

                cellRange = paymentsTable.Cell(i + 2, 6).Range;
                cellRange.Text = ProductCurrent.wage_rate.ToString();
            }

            EmployeesApplicationPDF.Visible = true;

            document.SaveAs2(@"C:\Users\User\OneDrive\Desktop\KalashnikovGroupApp\KalashnikovGroupApp\Files\Employees.pdf", Word.WdExportFormat.wdExportFormatPDF);
        }
    }
}
