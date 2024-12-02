using Newtonsoft.Json;

namespace RestApi.Model
{
    /// <summary>
    /// Diese Klasse repräsentiert die Preisangaben in einer bestimmten Währung.
    /// </summary>
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
}
