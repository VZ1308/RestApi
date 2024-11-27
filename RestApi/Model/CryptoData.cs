using Newtonsoft.Json;
using System.Collections.Generic;

namespace RestApi.Model.CryptoData
{
    public class Quote
    {
        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("market_cap")]
        public decimal MarketCap { get; set; }

        [JsonProperty("volume_24h")]
        public decimal Volume_24h { get; set; }

        [JsonProperty("ath_price")]
        public decimal AllTimeHigh { get; set; }
    }

    public class CryptoData
    {
        public string id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("total_supply")]
        public decimal TotalSupply { get; set; }

        [JsonProperty("circulating_supply")]
        public decimal CirculatingSupply { get; set; }


        [JsonProperty("quotes")]
        public Dictionary<string, Quote> Quotes { get; set; }

        // Werte für die Darstellung in der Konsole
        public decimal Price { get; set; }
        public decimal MarketCap { get; set; }
        public decimal Volume_24h { get; set; }
        public decimal AllTimeHigh { get; set; }
    }
}
