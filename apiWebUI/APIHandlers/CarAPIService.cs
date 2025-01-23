using apiWebUI.Classes;
using Newtonsoft.Json;

namespace apiWebUI.APIHandlers;

public class CarAPIService
{
    private readonly HttpClient _httpClient;

    private Dictionary<int, string> carImageLinks = new Dictionary<int, string>()
    {
        { 1, "brokenCar1.png" },
        { 2, "brokenCar2.png" },
        { 3, "brokenCar3.png" },
        { 4, "brokenCar4.png" },
        { 5, "brokenCar5.png" },
        { 6, "brokenCar6.png" },
        { 7, "brokenCar7.png" },
        { 8, "brokenCar8.png" },
        { 9, "seatmii.png" },
        { 10, "jackpot.png" },
    };

    public CarAPIService()
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

    public async Task<Person> GetOwner(APIUrls apiUrl, int personId)
    {
        string baseUrl = apiUrl.GetPeopleAPIUrl("findById");
        string urlParms = $"?id={personId}";
        string constructedUrl = $"{baseUrl}{urlParms}";
        
        string apiResponse = await _httpClient.GetStringAsync(constructedUrl);
        return JsonConvert.DeserializeObject<Person>(apiResponse);
    }

    public async Task<List<Car>> GetCarsAsync(string Manufacturer, string Model, int pageNumber)
    {
        int pageSize = 30;
        APIUrls apiUrls = new APIUrls();
        string baseUrl = apiUrls.GetCarsAPIUrl("read");
    
        Manufacturer = string.IsNullOrEmpty(Manufacturer) ? null : UpperFirstLetter(Manufacturer);
        Model = string.IsNullOrEmpty(Model) ? null : UpperFirstLetter(Model);
    
        string urlParms = $"?{(Manufacturer != null ? $"Manufacturer={Manufacturer}&" : "")}{(Model != null ? $"Model={Model}&" : "")}";
        urlParms = urlParms.TrimEnd('&');
    
        string urlEnd = $"&pageSize={pageSize}&pageNumber={pageNumber}";
        string constructedUrl = $"{baseUrl}{urlParms}{urlEnd}";
    
        string apiResponse = await _httpClient.GetStringAsync(constructedUrl);
        List<Car> cars = JsonConvert.DeserializeObject<List<Car>>(apiResponse);
        
        Random rng = new Random();
        foreach (Car car in cars)
        {
            Person carOwner = await GetOwner(apiUrls, car.OwnerId);
            car.Owner = carOwner;
            car.carImage = carImageLinks[rng.Next(1, carImageLinks.Count+1)];
        }
        return cars;
    }
    
    public async Task<string> AddCarAsync(Car carToAdd)
    {
        APIUrls apiUrls = new APIUrls();
        
        carToAdd.Vin = UpperFirstLetter(carToAdd.Vin);
        carToAdd.Manufacturer = UpperFirstLetter(carToAdd.Manufacturer);
        carToAdd.Model = UpperFirstLetter(carToAdd.Model);
        carToAdd.Type = UpperFirstLetter(carToAdd.Type);
        carToAdd.Fuel = UpperFirstLetter(carToAdd.Fuel);
        
        string baseUrl = apiUrls.GetCarsAPIUrl("create");
        string urlParms = $"?Vin={carToAdd.Vin}&Manufacturer={carToAdd.Manufacturer}&Model={carToAdd.Model}&Type={carToAdd.Type}&Fuel={carToAdd.Fuel}&OwnerId={carToAdd.OwnerId}";
        string constructedUrl = $"{baseUrl}{urlParms}";
        
        HttpResponseMessage response = await _httpClient.PostAsync(constructedUrl, null);
        string apiResponse = response.Content.ReadAsStringAsync().Result;
        return apiResponse;
    }
    
    public async Task<string> CountCarsAsync()
    {
        APIUrls apiUrls = new APIUrls();
        string baseUrl = apiUrls.GetCarsAPIUrl("count");
        string apiResponse = await _httpClient.GetStringAsync(baseUrl);
        return apiResponse;
    }

    public async Task<string> DeleteCarAsync(Car carToRemove)
    {
        APIUrls apiUrls = new APIUrls();
        string baseUrl = apiUrls.GetCarsAPIUrl("delete");
        string urlParms = $"?id={carToRemove.Id}";
        string constructedUrl = $"{baseUrl}{urlParms}";
        HttpResponseMessage response = await _httpClient.DeleteAsync(constructedUrl);
        string apiResponse = response.Content.ReadAsStringAsync().Result;
        return apiResponse;
    }
}