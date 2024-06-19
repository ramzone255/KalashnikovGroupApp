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
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace KalashnikovGroupApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для PaydayPage.xaml
    /// </summary>
    public partial class PaydayPage : Page
    {
        private readonly ApiService _apiService;

        private List<Payday> _allPayday;
        public PaydayPage()
        {
            InitializeComponent(); 
            _apiService = new ApiService();
            LoadPayday();
        }
        private async Task LoadPayday()
        {
            try
            {
                _allPayday = await _apiService.GetPaydayCollection();
                PaydayListView.ItemsSource = _allPayday;
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
            NavigationService.Navigate(new ComponentsPage());
        }

        private void DealClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DealPage());
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

            worksheet.Cells[2][indexRows] = "К выплате";
            worksheet.Cells[3][indexRows] = "Дата начала";
            worksheet.Cells[4][indexRows] = "Дата конца";

            var printItems = PaydayListView.Items;

            foreach (Payday item in printItems)
            {
                worksheet.Cells[1][indexRows + 1] = indexRows;
                worksheet.Cells[2][indexRows + 1] = item.paycheck;
                worksheet.Cells[3][indexRows + 1] = item.start_date;
                worksheet.Cells[4][indexRows + 1] = item.end_date;

                indexRows++;
            }
            Excel.Range range = worksheet.Range[worksheet.Cells[2][indexRows + 1],
                    worksheet.Cells[4][indexRows + 1]];

            range.ColumnWidth = 20;

            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

            ExcelApp.Visible = true;
        }

        private async void PDFClick(object sender, RoutedEventArgs e)
        {
            _allPayday = await _apiService.GetPaydayCollection();
            var PaydayInPDF = _allPayday;

            var PaydayApplicationPDF = new Word.Application();

            Word.Document document = PaydayApplicationPDF.Documents.Add();

            Word.Paragraph empParagraph = document.Paragraphs.Add();
            Word.Range empRange = empParagraph.Range;
            empRange.Text = "Payday";
            empRange.Font.Bold = 4;
            empRange.Font.Italic = 4;
            empRange.Font.Color = Word.WdColor.wdColorBlack;
            empRange.InsertParagraphAfter();

            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table paymentsTable = document.Tables.Add(tableRange, PaydayInPDF.Count() + 1, 3);
            paymentsTable.Borders.InsideLineStyle = paymentsTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            paymentsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            Word.Range cellRange;

            cellRange = paymentsTable.Cell(1, 1).Range;
            cellRange.Text = "К выплате";
            cellRange = paymentsTable.Cell(1, 2).Range;
            cellRange.Text = "Дата начала";
            cellRange = paymentsTable.Cell(1, 3).Range;
            cellRange.Text = "Дата конца";


            paymentsTable.Rows[1].Range.Bold = 1;
            paymentsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            for (int i = 0; i < PaydayInPDF.Count(); i++)
            {
                var ProductCurrent = PaydayInPDF[i];

                cellRange = paymentsTable.Cell(i + 2, 1).Range;
                cellRange.Text = ProductCurrent.paycheck.ToString();

                cellRange = paymentsTable.Cell(i + 2, 2).Range;
                cellRange.Text = ProductCurrent.end_date.ToString();

                cellRange = paymentsTable.Cell(i + 2, 3).Range;
                cellRange.Text = ProductCurrent.start_date.ToString();
            }

            PaydayApplicationPDF.Visible = true;

            document.SaveAs2(@"C:\Users\User\OneDrive\Desktop\KalashnikovGroupApp\KalashnikovGroupApp\Files\Payday.pdf", Word.WdExportFormat.wdExportFormatPDF);
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PaydayPageAdd());
        }

        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            if (PaydayListView.SelectedItem is Payday selectedPayday)
            {
                NavigationService.Navigate(new PaydayPageAdd(selectedPayday));
            }
            else
            {
                MessageBox.Show("Выберите зарплату для редактирования");
            }
        }

        private async void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (PaydayListView.SelectedItem is Payday selectedPayday)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить зарплату с {selectedPayday.start_date}?", "Подтверждение удаления", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _apiService.DeletePayday(selectedPayday.id_payday);
                        MessageBox.Show("Зарплата успешно удален.");
                        await LoadPayday();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении зарплаты: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите зарплату для удаления.");
            }
        }

        private void TbSerch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            var filteredPayday = _allPayday.AsEnumerable();
            if (int.TryParse(TbSerch.Text, out var minQuality))
            {
                filteredPayday = filteredPayday.Where(p => p.paycheck >= minQuality);
            }
            PaydayListView.ItemsSource = filteredPayday.ToList();
        }
    }
}
