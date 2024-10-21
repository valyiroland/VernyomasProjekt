using System;
using System.Collections.Generic;
using System.IO;
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

namespace vérnyomás
{
    public partial class MainWindow : Window
    {
        private List<Vernyomas> records = new List<Vernyomas>();
        private const string FilePath = "blood_pressure_data.txt";

        public MainWindow()
        {
            InitializeComponent();
            DateTextBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void SaveDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(SystolicTextBox.Text, out int systolic) &&
                int.TryParse(DiastolicTextBox.Text, out int diastolic) &&
                int.TryParse(PulseTextBox.Text, out int pulse) &&
                !string.IsNullOrWhiteSpace(UserTextBox.Text))
            {
                var record = new Vernyomas
                {
                    Date = DateTextBox.Text,
                    Systolic = systolic,
                    Diastolic = diastolic,
                    Pulse = pulse,
                    User = UserTextBox.Text,
                    Type = DetermineBloodPressureType(systolic, diastolic)
                };

                records.Add(record);
                SaveRecordToFile(record);
                UpdateDataGrid();

                ResultTextBlock.Text = $"A vérnyomás típusa: {record.Type}";

                // Szövegdobozok ürítése
                SystolicTextBox.Clear();
                DiastolicTextBox.Clear();
                PulseTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Kérlek, érvényes adatokat adj meg!");
            }
        }

        private string DetermineBloodPressureType(int systolic, int diastolic)
        {
            if (systolic < 90 && diastolic < 60) return "Alacsony";
            if (systolic < 120 && diastolic < 80) return "Normál";
            if (systolic < 130 && diastolic < 85) return "Prehypertension";
            if (systolic < 140 && diastolic < 90) return "1. Hipertónia";
            return "2. Hipertónia";
        }

        private void SaveRecordToFile(Vernyomas record)
        {
            var line = $"{record.User};{record.Date};{record.Systolic};{record.Diastolic};{record.Pulse};{record.Type}";
            File.AppendAllLines(FilePath, new[] { line });
        }

        private void UpdateDataGrid()
        {
            DataGrid.ItemsSource = null;
            DataGrid.ItemsSource = records;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}