using System;
using System.Windows;
using RestApi.ViewModel; 
using RestApi.Model.CryptoData;
using Newtonsoft.Json;

namespace MyCryptoWpfApp
{
    public partial class MainWindow : Window
    {
        private readonly IRestApi _apiService;

        public MainWindow()
        {
            InitializeComponent();
            _apiService = new RestApiService(new LogService()); 
        }

        private async void FetchDataButton_Click(object sender, RoutedEventArgs e)
        {
            string cryptoName = CryptoNameInput.Text.Trim();

            if (string.IsNullOrEmpty(cryptoName))
            {
                MessageBox.Show("Bitte geben Sie einen Namen für die Kryptowährung ein.", "Eingabefehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // API-ID abrufen
                string cryptoId = await _apiService.GetCryptoIdAsync(cryptoName);

                // API-Daten abrufen
                var response = await _apiService.GetAsync(cryptoId);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                var cryptoData = JsonConvert.DeserializeObject<CryptoData>(jsonResponse);

                // Anzeige in der ListBox
                DataDisplay.Items.Clear();
                DataDisplay.Items.Add($"Name: {cryptoData.Name}");
                DataDisplay.Items.Add($"Symbol: {cryptoData.Symbol}");
                DataDisplay.Items.Add($"Rang: {cryptoData.Rank}");
                DataDisplay.Items.Add($"Preis: {cryptoData.Quotes["USD"].Price} USD");
                DataDisplay.Items.Add($"Marktkapitalisierung: {cryptoData.Quotes["USD"].MarketCap} USD");
                DataDisplay.Items.Add($"Volumen (24h): {cryptoData.Quotes["USD"].Volume_24h} USD");
                DataDisplay.Items.Add($"Allzeithoch: {cryptoData.Quotes["USD"].AllTimeHigh} USD");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        }
}

