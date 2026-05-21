var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.Urls.Add("http://127.0.0.1:8000");

app.MapGet("/api", () =>
{
    return input.to_json()
});
app.MapGet("/api/predict", responseModel=PredictionOutput)
def predictCost