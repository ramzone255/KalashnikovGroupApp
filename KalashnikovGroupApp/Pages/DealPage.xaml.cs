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
    /// Логика взаимодействия для DealPage.xaml
    /// </summary>
    public partial class DealPage : Page
    {
        private readonly ApiService _apiService;

        private List<Deal> _allDeal;
        public DealPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadDeal();
        }
        private async Task LoadDeal()
        {
            try
            {
                _allDeal = await _apiService.GetDeal();
                DealListView.ItemsSource = _allDeal;
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

        }

        private void PaydayClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PaydayPage());
        }

        private void ExcelClick(object sender, RoutedEventArgs e)
        {
            var ExcelApp = new Excel.Application();

            Excel.Workbook wb = ExcelApp.Workbooks.Add();

            Excel.Worksheet worksheet = ExcelApp.Worksheets.Item[1];

            int indexRows = 1;

            worksheet.Cells[2][indexRows] = "Дата";
            worksheet.Cells[3][indexRows] = "Количество";
            worksheet.Cells[4][indexRows] = "Сумма";

            var printItems = DealListView.Items;

            foreach (Deal item in printItems)
            {
                worksheet.Cells[1][indexRows + 1] = indexRows;
                worksheet.Cells[2][indexRows + 1] = item.date;
                worksheet.Cells[3][indexRows + 1] = item.quality;
                worksheet.Cells[4][indexRows + 1] = item.total_amount;

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
            _allDeal = await _apiService.GetDeal();
            var DealInPDF = _allDeal;

            var DealApplicationPDF = new Word.Application();

            Word.Document document = DealApplicationPDF.Documents.Add();

            Word.Paragraph empParagraph = document.Paragraphs.Add();
            Word.Range empRange = empParagraph.Range;
            empRange.Text = "Deal";
            empRange.Font.Bold = 4;
            empRange.Font.Italic = 4;
            empRange.Font.Color = Word.WdColor.wdColorBlack;
            empRange.InsertParagraphAfter();

            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table paymentsTable = document.Tables.Add(tableRange, DealInPDF.Count() + 1, 3);
            paymentsTable.Borders.InsideLineStyle = paymentsTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            paymentsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            Word.Range cellRange;

            cellRange = paymentsTable.Cell(1, 1).Range;
            cellRange.Text = "Дата";
            cellRange = paymentsTable.Cell(1, 2).Range;
            cellRange.Text = "Количество";
            cellRange = paymentsTable.Cell(1, 3).Range;
            cellRange.Text = "Сумма";


            paymentsTable.Rows[1].Range.Bold = 1;
            paymentsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            for (int i = 0; i < DealInPDF.Count(); i++)
            {
                var ProductCurrent = DealInPDF[i];

                cellRange = paymentsTable.Cell(i + 2, 1).Range;
                cellRange.Text = ProductCurrent.date.ToString();

                cellRange = paymentsTable.Cell(i + 2, 2).Range;
                cellRange.Text = ProductCurrent.quality.ToString();

                cellRange = paymentsTable.Cell(i + 2, 3).Range;
                cellRange.Text = ProductCurrent.total_amount.ToString();
            }

            DealApplicationPDF.Visible = true;

            document.SaveAs2(@"C:\Users\User\OneDrive\Desktop\KalashnikovGroupApp\KalashnikovGroupApp\Files\Deal.pdf", Word.WdExportFormat.wdExportFormatPDF);
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DealPageAdd());
        }

        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            if (DealListView.SelectedItem is Deal selectedDeal)
            {
                NavigationService.Navigate(new DealPageAdd(selectedDeal));
            }
            else
            {
                MessageBox.Show("Выберите дело для редактирования");
            }
        }

        private async void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (DealListView.SelectedItem is Deal selectedDeal)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить дело от {selectedDeal.date}?", "Подтверждение удаления", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _apiService.DeleteDeal(selectedDeal.id_deal);
                        MessageBox.Show("Дело успешно удален.");
                        await LoadDeal();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении дела: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите дело для удаления.");
            }
        }

        private void TbSerch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            var filteredDeal = _allDeal.AsEnumerable();
            if (int.TryParse(TbSerch.Text, out var minQuality))
            {
                filteredDeal = filteredDeal.Where(p => p.quality >= minQuality);
            }
            DealListView.ItemsSource = filteredDeal.ToList();
        }
    }
}
