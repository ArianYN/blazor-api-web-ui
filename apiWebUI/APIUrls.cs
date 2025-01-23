using apiWebUI.Classes;
using Newtonsoft.Json;

namespace apiWebUI;

public class APIUrls
{
    private readonly string APIBaseUrl;

    public APIUrls()
    {
        string secretFilePath = "/run/secrets/APIBaseUrl";
        
        if (File.Exists(secretFilePath))
        {
            APIBaseUrl = File.ReadAllText(secretFilePath);
        }
        else
        {
            throw new InvalidOperationException("APIBaseUrl secret not found.");
        }
    }

    public string GetBogusAPIUrl(string operation)
    {
        string apiUrl = $"{APIBaseUrl}Bogus/{operation}";
        return apiUrl;
    }
    public string GetPeopleAPIUrl(string operation)
    {
        string ApiUrl = $"{APIBaseUrl}People/{operation}";
        return ApiUrl;
    }

    public string GetCarsAPIUrl(string operation)
    {
        string ApiUrl = $"{APIBaseUrl}Car/{operation}";
        return ApiUrl;
    }
}