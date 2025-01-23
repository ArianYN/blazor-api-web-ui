namespace apiWebUI.APIHandlers;
using apiWebUI.Classes;
using Newtonsoft.Json;

public class PeopleAPIService
{
    private readonly HttpClient _httpClient;

    public PeopleAPIService()
    {
        _httpClient = new HttpClient();
    }

    public string UpperFirstLetter(string input)
    {
        //input = input.ToLower();
        if (input == null)
        {
            return null;
        }
        return char.ToUpper(input[0]) + input.Substring(1);
    }

    public async Task<List<Person>> GetPeopleAsync(string FirstNameInput, string LastNameInput, int pageNumber)
    {
        int pageSize = 30;
        APIUrls apiUrls = new APIUrls();
        string baseUrl = apiUrls.GetPeopleAPIUrl("read");
    
        FirstNameInput = string.IsNullOrEmpty(FirstNameInput) ? null : UpperFirstLetter(FirstNameInput);
        LastNameInput = string.IsNullOrEmpty(LastNameInput) ? null : UpperFirstLetter(LastNameInput);
    
        string urlParms = $"?{(FirstNameInput != null ? $"FirstName={FirstNameInput}&" : "")}{(LastNameInput != null ? $"LastName={LastNameInput}&" : "")}";
        urlParms = urlParms.TrimEnd('&');
    
        string urlEnd = $"&pageSize={pageSize}&pageNumber={pageNumber}";
        string constructedUrl = $"{baseUrl}{urlParms}{urlEnd}";
    
        string apiResponse = await _httpClient.GetStringAsync(constructedUrl);
        return JsonConvert.DeserializeObject<List<Person>>(apiResponse);
    }

    public async Task<string> AddPersonAsync(Person personToAdd)
    {
        APIUrls apiUrls = new APIUrls();
        
        string baseUrl = apiUrls.GetPeopleAPIUrl("create");
        string urlParms = $"?FirstName={personToAdd.FirstName}&LastName={personToAdd.LastName}&Address={personToAdd.Address}&Email={personToAdd.Email}";
        string constructedUrl = $"{baseUrl}{urlParms}";
        
        HttpResponseMessage response = await _httpClient.PostAsync(constructedUrl, null);
        
        string apiResponse = response.Content.ReadAsStringAsync().Result;
        return apiResponse;
    }

    public async Task<string> CountPeopleAsync()
    {
        APIUrls apiUrls = new APIUrls();
        string baseUrl = apiUrls.GetPeopleAPIUrl("count");
        string apiResponse = await _httpClient.GetStringAsync(baseUrl);
        return apiResponse;
    }

    public async Task<string> EditPersonAsync(Person personToEdit)
    {
        APIUrls apiUrls = new APIUrls();
        
        string baseUrl = apiUrls.GetPeopleAPIUrl("update");
        string urlParms = $"?idToUpdate={personToEdit.Id}&FirstName={personToEdit.FirstName}&LastName={personToEdit.LastName}&Address={personToEdit.Address}&Email={personToEdit.Email}";
        string constructedUrl = $"{baseUrl}{urlParms}";
        
        HttpResponseMessage response = await _httpClient.PutAsync(constructedUrl, null);
        string apiResponse = response.Content.ReadAsStringAsync().Result;
        return apiResponse;
    }
}