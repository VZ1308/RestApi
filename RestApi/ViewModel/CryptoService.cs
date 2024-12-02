using Newtonsoft.Json;
using RestApi.Model.CryptoData;
using RestApi.ViewModel;

namespace Restapi
{
    public interface ICryptoService
    {
        Task<CryptoData> GetCryptoDataAsync(string cryptoId);
    }

    public class CryptoService : ICryptoService
    {
        private readonly IRestApi _restApi;
        private readonly ILogService _logService;


        public CryptoService(IRestApi restApi, ILogService logService)
        {
            _restApi = restApi ?? throw new ArgumentNullException(nameof(restApi));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }


        /// <summary>
        /// Ruft die Daten einer Kryptowährung basierend auf ihrem Namen ab.
        /// </summary>
        /// <param name="cryptoName">Der Name der Kryptowährung (z.B. "Bitcoin").</param>
        /// <returns>Ein <see cref="CryptoData"/>-Objekt mit den aktuellen Marktdaten der Kryptowährung.</returns>
        /// <exception cref="Exception">Wird ausgelöst, wenn beim Abrufen oder Verarbeiten der Daten ein Fehler auftritt.</exception>
        public async Task<CryptoData> GetCryptoDataAsync(string cryptoName)
        {
            try
            {
                // Abruf der Kryptowährungs-ID
                string cryptoId = await _restApi.GetCryptoIdAsync(cryptoName);
                var response = await _restApi.GetAsync(cryptoId);
                var data = await response.Content.ReadAsStringAsync(); // iest den gesamten Inhalt der Antwort und gibt ihn als reinen String zurück
                // wandelt den JSON-String data in ein CryptoData-Objekt um
                var cryptoData = JsonConvert.DeserializeObject<CryptoData>(data) ?? throw new Exception("Ungültige Datenstruktur von der API."); // stellt sicher, dass ungültige oder leere Daten erkannt werden
                if (cryptoData.Quotes != null && cryptoData.Quotes.ContainsKey("USD"))
                {
                    // Zuweisung der USD-spezifischen Werte
                    var usdQuote = cryptoData.Quotes["USD"];
                    cryptoData.Price = usdQuote.Price;
                    cryptoData.MarketCap = usdQuote.MarketCap;
                    cryptoData.Volume_24h = usdQuote.Volume_24h;
                    cryptoData.AllTimeHigh = usdQuote.AllTimeHigh;
                }

                return cryptoData; // Rückgabe des vollständigen Objekts
            }
            catch (Exception ex)
            {
                _logService.SaveLogFile($"Fehler beim Abrufen der Kryptowährungsdaten: {ex.Message}");
                throw;
            }
        }
    }
}