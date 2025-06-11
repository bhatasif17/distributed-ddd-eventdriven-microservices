using System.Text.Json.Serialization;

namespace Wakett.Common.DTOs;

public class CryptoDataResponse
{
    public Status Status { get; set; }
    public List<CoinData> Data { get; set; }
}

public class Status
{
    public DateTime Timestamp { get; set; }
    [JsonPropertyName("error_code")]
    public int ErrorCode { get; set; }
    [JsonPropertyName("error_message")]
    public string ErrorMessage { get; set; }
    public int Elapsed { get; set; }
    [JsonPropertyName("credit_count")]
    public int CreditCount { get; set; }
    public string Notice { get; set; }
    [JsonPropertyName("total_count")]
    public int TotalCount { get; set; }
}

public class CoinData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public string Slug { get; set; }
    [JsonPropertyName("num_market_pairs")]
    public int NumMarketPairs { get; set; }
    [JsonPropertyName("date_added")]
    public DateTime DateAdded { get; set; }
    public List<string> Tags { get; set; }
    // [JsonPropertyName("max_supply")]
    // public long? MaxSupply { get; set; }
    [JsonPropertyName("circulating_supply")]
    public double CirculatingSupply { get; set; }
    [JsonPropertyName("total_supply")]
    public double TotalSupply { get; set; }
    [JsonPropertyName("infinite_supply")]
    public bool InfiniteSupply { get; set; }
    public object Platform { get; set; }
    [JsonPropertyName("cmc_rank")]
    public int CmcRank { get; set; }
    [JsonPropertyName("self_reported_circulating_supply")]
    public object SelfReportedCirculatingSupply { get; set; }
    [JsonPropertyName("self_reported_market_cap")]
    public object SelfReportedMarketCap { get; set; }
    [JsonPropertyName("tvl_ratio")]
    public object TvlRatio { get; set; }
    [JsonPropertyName("last_updated")]
    public DateTime LastUpdated { get; set; }
    public Quote Quote { get; set; }
}

public class Quote
{
    [JsonPropertyName("USD")]
    public UsdData Usd { get; set; }
}

public class UsdData
{
    public decimal Price { get; set; }
    [JsonPropertyName("volume_24h")]
    public double Volume24h { get; set; }
    [JsonPropertyName("volume_change_24h")]
    public double VolumeChange24h { get; set; }
    [JsonPropertyName("percent_change_1h")]
    public double PercentChange1h { get; set; }
    [JsonPropertyName("percent_change_24h")]
    public double PercentChange24h { get; set; }
    [JsonPropertyName("percent_change_7d")]
    public double PercentChange7d { get; set; }
    [JsonPropertyName("percent_change_30d")]
    public double PercentChange30d { get; set; }
    [JsonPropertyName("percent_change_60d")]
    public double PercentChange60d { get; set; }
    [JsonPropertyName("percent_change_90d")]
    public double PercentChange90d { get; set; }
    [JsonPropertyName("market_cap")]
    public double MarketCap { get; set; }
    [JsonPropertyName("market_cap_dominance")]
    public double MarketCapDominance { get; set; }
    [JsonPropertyName("fully_diluted_market_cap")]
    public double FullyDilutedMarketCap { get; set; }
    public object Tvl { get; set; }
    [JsonPropertyName("last_updated")]
    public DateTime LastUpdated { get; set; }
}