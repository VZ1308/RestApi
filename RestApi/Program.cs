using Restapi;
using RestApi.ViewModel;

namespace RestApi
{
    // Hauptprogramm
    class Program
    {
        static async Task Main(string[] args)
        {
            // baut ein Service-Container auf, der Abh�ngigkeiten verwaltet und bereitstellt
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ILogService, LogService>()
                .AddSingleton<IRestApi, RestApiService>()
                .AddSingleton<ICryptoService, CryptoService>()
                .BuildServiceProvider();

            var cryptoService = serviceProvider.GetService<ICryptoService>(); // l�st die Abh�ngigkeit auf und stellt eine Instanz von ICryptoService zur Verf�gung

            Console.WriteLine("Abruf von Kryptow�hrungsdaten...");
            Console.Write("Geben Sie den Namen einer Kryptow�hrung ein (z. B. 'bitcoin', 'ethereum'): ");
            string input = Console.ReadLine();

            try
            {
                var cryptoData = await cryptoService.GetCryptoDataAsync(input);

                Console.WriteLine($"Kryptow�hrung: {cryptoData.Name}");
                Console.WriteLine($"Symbol: {cryptoData.Symbol}");
                Console.WriteLine($"Rang: {cryptoData.Rank}");
                Console.WriteLine($"Preis: {cryptoData.Price} USD");
                Console.WriteLine($"Marktkapitalisierung: {cryptoData.MarketCap} USD");
                Console.WriteLine($"Volumen (24h): {cryptoData.Volume_24h} USD");
                Console.WriteLine($"Allzeithoch: {cryptoData.AllTimeHigh} USD");
                Console.WriteLine($"Gesamtversorgung: {cryptoData.TotalSupply}");
                Console.WriteLine($"Maximale Versorgung: {cryptoData.MaxSupply ?? 0}");
                Console.WriteLine($"Beta-Wert: {cryptoData.BetaValue}");
                Console.WriteLine($"Letzte Aktualisierung: {cryptoData.LastUpdated}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }
        }
    }
}