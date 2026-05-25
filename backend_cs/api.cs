using Microsoft.ML;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.Urls.Add("http://127.0.0.1:8000");
var mlContext = new MLContext();

app.MapGet("/api/predict", (TaxiInput input) =>
{
    ITransformer model = mlContext.Model.Load("taxi_price_guesser_cs.zip", out var schema);
    var predictionEngine = mlContext.Model.CreatePredictionEngine<TaxiInput, TaxiOutput>(model);
    var prediction = predictionEngine.Predict(input);
    return Results.Ok(new
    {
        price = prediction.Trip_Price
    });
});