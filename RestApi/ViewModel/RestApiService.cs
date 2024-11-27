using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestApi.ViewModel
{
    public interface IRestApi
    {
        Task<HttpResponseMessage> GetAsync(string cryptoId); // holt die Daten einer Kryptowährung anhand der ID
        Task<string> GetCryptoIdAsync(string query); // findet die ID zu einem Namen
    }

    public class RestApiService : IRestApi
    {
        private const string BASICENDPOINT = "https://api.coinpaprika.com/v1/";
        private readonly ILogService _logService;

        public async Task<string> GetCryptoIdAsync(string query)
        {
            try
            {
                using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(30) }; // setzt ein Timeout von 30 Sekunden, nach dem die Anfrage abgebrochen wird, falls keine Antwort kommt
                var searchUrl = $"{BASICENDPOINT}search?q={query}&c=currencies"; // query enthält den eingegebenen Suchbegriff
                Console.WriteLine($"Suche nach Kryptowährungs-ID unter: {searchUrl}");
                var response = await client.GetAsync(searchUrl); // speichert das Ergebnis der Anfrage als HttpResponseMessage

                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                // dynamic wenn der Typ zur Laufzeit entschieden wird
                dynamic jsonResponse = JsonConvert.DeserializeObject(content); // wird verwendet, um einen JSON-String (content) in ein C#-Objekt umzuwandeln

                if (jsonResponse.currencies.Count == 0)
                {
                    throw new Exception("Keine passende Kryptowährung gefunden.");
                }

                return jsonResponse.currencies[0].id; // greift auf erstes Element in der Liste zu 
            }
            catch (Exception ex)
            {
                _logService.SaveLogFile($"Fehler beim Suchen der Kryptowährungs-ID: {ex.Message}");
                throw new Exception("Fehler beim Abrufen der Kryptowährungs-ID.");
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            try
            {
                using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
                var url = $"{BASICENDPOINT}tickers/{endpoint}";
                Console.WriteLine($"Anfrage an URL: {url}");
                var response = await client.GetAsync(url);

                response.EnsureSuccessStatusCode();
                return response;
            }
            catch (HttpRequestException httpEx)
            {
                _logService.SaveLogFile($"HTTP-Fehler: {httpEx.Message}");
                throw new Exception($"HTTP-Fehler beim Zugriff auf die API: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                _logService.SaveLogFile($"Allgemeiner Fehler: {ex.Message}");
                throw new Exception($"Fehler beim Zugriff auf die API: {ex.Message}");
            }
        }
    }
}
