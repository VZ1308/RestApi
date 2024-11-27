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

        public async Task<CryptoData> GetCryptoDataAsync(string cryptoName)
        {
            try
            {
                // Abruf der Kryptowährungs-ID
                string cryptoId = await _restApi.GetCryptoIdAsync(cryptoName);
                var response = await _restApi.GetAsync(cryptoId);
                var data = await response.Content.ReadAsStringAsync();

                var cryptoData = JsonConvert.DeserializeObject<CryptoData>(data) ?? throw new Exception("Ungültige Datenstruktur von der API.");
                if (cryptoData.Quotes != null && cryptoData.Quotes.ContainsKey("USD"))
                {
                    var usdQuote = cryptoData.Quotes["USD"];
                    cryptoData.Price = usdQuote.Price;
                    cryptoData.MarketCap = usdQuote.MarketCap;
                    cryptoData.Volume_24h = usdQuote.Volume_24h;
                    cryptoData.AllTimeHigh = usdQuote.AllTimeHigh;
                }

                return cryptoData;
            }
            catch (Exception ex)
            {
                _logService.SaveLogFile($"Fehler beim Abrufen der Kryptowährungsdaten: {ex.Message}");
                throw;
            }
        }
    }
}