using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace apiWebUI.APIHandlers;

public class BogusAPIService
{
    private readonly HttpClient _httpClient;

    public BogusAPIService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> CreateData(string operation, int amount)
    {
        APIUrls apiUrls = new APIUrls();
        
        string baseUrl = apiUrls.GetBogusAPIUrl(operation);
        string urlParms = $"?count={amount}";
        string constructedUrl = $"{baseUrl}{urlParms}";
        
        HttpResponseMessage apiHttpResponse = await _httpClient.PostAsync(constructedUrl, null);
        
        if (!apiHttpResponse.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {apiHttpResponse.StatusCode}");
            string errorDetails = await apiHttpResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Details: {errorDetails}");
        }
        
        string apiResponse = apiHttpResponse.Content.ReadAsStringAsync().Result;
        return apiResponse;
    }
}