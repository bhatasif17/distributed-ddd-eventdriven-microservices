using System.Text.Json;
using Wakett.Common.DTOs;

namespace Wakett.Infrastructure.Clients;

public class CoinMarketCapClient
{
    public async Task<List<Cryptocurrency>> GetLatestRatesAsync()
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", "aa28d15a-4bb5-4b7f-9aca-9b22fac7d109");
        client.DefaultRequestHeaders.Add("Accepts", "application/json");
        
        var response = await client.GetAsync(
            "https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest?convert=USD");
        var jsonString = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<CryptoDataResponse>(jsonString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return data.Data.Select(crypto => new Cryptocurrency(
            crypto.Symbol,
            crypto.Quote.Usd.Price
        )).ToList();
    }
}

public record Cryptocurrency(string Symbol, decimal Price);