using System.Net.Http;
using System.Net.Http.Json;
var payload = new Payload;
public class TaxiValues
{
    public double Trip_Distance_km { get; set; } = 6.0;
    public string Time_of_Day { get; set; } = "Morning";
    public string Day_of_Week { get; set; } = "Weekday";
    public int Passenger_Count { get; set; } = 1;
    public string Traffic_Conditions { get; set; } = "Low";
    public string Weather { get; set; } = "Clear";
    public double Base_Fare { get; set; } = 4.0;
    public double Per_Km_Rate { get; set; } = 1.0;
    public double Per_Minute_Rate { get; set; } = 0.5;
    public int Trip_Duration_Minutes { get; set; } = 50;
}
public class ApiService
{
    private readonly HttpClient _http;

    public ApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Dictionary<string, object>>> GetRawData()
    {
        return await _http.GetFromJsonAsync<List<Dictionary<string, object>>>("/api");
    }

    public async Task<double?> Predict(TaxiTripInput input)
    {
        var response = await _http.PostAsJsonAsync("/api/predict", input);

        var result = await response.Content
            .ReadFromJsonAsync<Dictionary<string, double>>();

        return result?["predicted_cost"];
    }
}