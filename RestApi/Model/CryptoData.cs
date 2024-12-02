using Newtonsoft.Json;
using System.Collections.Generic;

namespace RestApi.Model.CryptoData
{
    /// <summary>
    /// Diese Klasse repräsentiert alle Informationen über eine Kryptowährung.
    /// </summary>
    public class CryptoData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("total_supply")]
        public decimal TotalSupply { get; set; }

        [JsonProperty("max_supply")]
        public decimal? MaxSupply { get; set; }

        [JsonProperty("beta_value")]
        public decimal BetaValue { get; set; }

        [JsonProperty("quotes")]
        public Dictionary<string, Quote> Quotes { get; set; }

        [JsonProperty("last_updated")]
        public string LastUpdated { get; set; }

        // Zusätzliche Eigenschaften für die Konsolenausgabe
        public decimal Price { get; set; }
        public decimal MarketCap { get; set; }
        public decimal Volume_24h { get; set; }
        public decimal AllTimeHigh { get; set; }
    }
}
