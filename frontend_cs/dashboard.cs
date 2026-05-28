using System.Net.Http;
using System.Net.Http.Json;
using System.Diagnostics;
public class TaxiValues
{
    public float? Trip_Distance_km { get; set; } = 6.0f;
    public string Time_of_Day { get; set; } = "Morning";
    public string Day_of_Week { get; set; } = "Weekday";
    public float? Passenger_Count { get; set; } = 1.0f;
    public string Traffic_Conditions { get; set; } = "Low";
    public string Weather { get; set; } = "Clear";
    public float? Base_Fare { get; set; } = 4.0f;
    public float? Per_Km_Rate { get; set; } = 1.0f;
    public float? Per_Minute_Rate { get; set; } = 0.5f;
    public float? Trip_Duration_Minutes { get; set; } = 50.0f;
}
public class ApiService
{
    private readonly HttpClient _http;
    private static readonly SemaphoreSlim _lock = new(1, 1);
    private string? _activeBackend;
    public ApiService(HttpClient http)
    {
        _http = http;
    }
    private async Task InitializeBackend()
    {
        
        await _lock.WaitAsync();
        try
        {
            string[] backends =
            {
                "http://backend_py:8000",
                "http://backend_cs:8000"
            };
            foreach (var backend in backends)
            {
                try
                {
                    var response = await _http.GetAsync($"{backend}/health");

                    if (response.IsSuccessStatusCode)
                    {
                        _activeBackend = backend;
                        Console.WriteLine(_activeBackend);
                        return;
                    }
                }
                catch
                {
                }
            }

            if (_activeBackend == null)
                throw new Exception("No backend available.");
        }
        finally
        {
            _lock.Release();
        }
    }
    public async Task<double?> Predict(TaxiValues input)
    {
        
        Console.WriteLine("dahsboard.cs: initializing adress");
        var timeGetWorkingAdress_cs = Stopwatch.StartNew();
        await InitializeBackend();
        timeGetWorkingAdress_cs.Stop();
        Console.WriteLine($"dashboard.cs: got working adress in: {timeGetWorkingAdress_cs.ElapsedMilliseconds} ms");
        var timeApi_cs = Stopwatch.StartNew();
        var response = await _http.PostAsJsonAsync($"{_activeBackend}/api/predict", input);
        timeApi_cs.Stop();
        Console.WriteLine($"dashboard.cs: recieved prediction in: {timeApi_cs.ElapsedMilliseconds} ms");
        var result = await response.Content
            .ReadFromJsonAsync<Dictionary<string, double>>();
        return result?["predicted_cost"];
    }
}