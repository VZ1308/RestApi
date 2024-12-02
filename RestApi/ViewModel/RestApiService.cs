using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestApi.ViewModel
{
    public interface IRestApi
    {
        Task<HttpResponseMessage> GetAsync(string cryptoId); // Holt die Daten einer Kryptowährung anhand der ID
        Task<string> GetCryptoIdAsync(string query); // Findet die ID zu einem Namen
    }

    public class RestApiService : IRestApi
    {
        // Entferne den Zeilenumbruch \r\n, damit die URL korrekt ist
        private const string BASICENDPOINT = "https://api.coinpaprika.com/v1";
        private readonly ILogService _logService;

        public RestApiService(ILogService logService)
        {
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        /// <summary>
        /// Findet die ID zum eingegebenen Namen und wird dann weiterverarbeitet.
        /// </summary>
        /// <param name="query">Der Suchbegriff, z.B. der Name einer Kryptowährung.</param>
        /// <returns>Die ID der Kryptowährung als Zeichenfolge.</returns>
        /// <exception cref="Exception">Wird ausgelöst, wenn keine Kryptowährung gefunden wird oder ein Fehler auftritt.</exception>
        public async Task<string> GetCryptoIdAsync(string query)
        {
            try
            {
                using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
                var searchUrl = $"{BASICENDPOINT}/search?q={query}&c=currencies"; // URL korrekt zusammensetzen
                Console.WriteLine($"Suche nach Kryptowährungs-ID unter: {searchUrl}");
                var response = await client.GetAsync(searchUrl);

                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                // Deserialisierung des JSON-Strings in ein dynamisches Objekt
                dynamic jsonResponse = JsonConvert.DeserializeObject(content);

                // Prüfe, ob "currencies" vorhanden ist und nicht leer
                if (jsonResponse?.currencies == null || jsonResponse.currencies.Count == 0)
                {
                    throw new Exception("Keine passende Kryptowährung gefunden.");
                }

                // Rückgabe der ID der ersten gefundenen Währung
                return jsonResponse.currencies[0].id;
            }
            catch (Exception ex)
            {
                _logService.SaveLogFile($"Fehler beim Suchen der Kryptowährungs-ID: {ex.Message}");
                throw new Exception("Fehler beim Abrufen der Kryptowährungs-ID.");
            }
        }

        /// <summary>
        /// Führt eine GET-Anfrage an die API durch, um die vollständigen Daten einer Kryptowährung zu erhalten.
        /// </summary>
        /// <param name="endpoint">Die API-Endpunkt-ID der Kryptowährung, z.B. "btc-bitcoin".</param>
        /// <returns>Die HTTP-Antwort mit den angeforderten Kryptowährungsdaten.</returns>
        /// <exception cref="HttpRequestException">Wird ausgelöst, wenn ein HTTP-Fehler auftritt.</exception>
        /// <exception cref="Exception">Wird ausgelöst, wenn ein allgemeiner Fehler auftritt.</exception>
        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            try
            {
                using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
                var url = $"{BASICENDPOINT}/tickers/{endpoint}"; // URL korrekt zusammensetzen
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
