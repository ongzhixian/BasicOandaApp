using Oanda.RestApi.Models;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;

namespace Oanda.RestApi.Services;

internal partial class OandaRestApi
{
    public async Task<IEnumerable<Order>> GetOrdersAsync(string accountId)
    {
        string url = $"/v3/accounts/{accountId}/orders";

        using HttpResponseMessage? httpResponse = await httpClient.GetAsync(url);

        httpResponse.EnsureSuccessStatusCode();

        using Stream? strm = await httpResponse.Content.ReadAsStreamAsync();

        //await DumpAsync(strm);

        OrderList? result =
            await JsonSerializer.DeserializeAsync<OrderList>(strm, jsonSerializerOptions);

        if (result == null || result.Orders == null)
        {
            return new List<Order>();
        }

        return result.Orders;
    }

    public async Task AddOrderAsync(string accountId, OrderRequest orderRequest)
    {
        string url = $"/v3/accounts/{accountId}/orders";

        var tx = JsonSerializer.Serialize(
                new NewOrder(orderRequest),
                orderSerializerOptions);

        HttpContent content = new StringContent(
            JsonSerializer.Serialize(
                new NewOrder(orderRequest),
                orderSerializerOptions));

        content.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Application.Json);

        using HttpResponseMessage? httpResponse = await httpClient.PostAsync(url, content);

        //httpResponse.EnsureSuccessStatusCode();

        using Stream? strm = await httpResponse.Content.ReadAsStreamAsync();

        await DumpAsync(strm);

        //OrderList? result =
        //    await JsonSerializer.DeserializeAsync<OrderList>(strm, jsonSerializerOptions);

        //if (result == null || result.Orders == null)
        //{
        //    return new List<Order>();
        //}

        //return result.Orders;
    }


}
