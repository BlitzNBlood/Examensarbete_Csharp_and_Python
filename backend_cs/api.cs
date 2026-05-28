using Microsoft.ML;
using System.Diagnostics;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var mlContext = new MLContext();

ITransformer model = mlContext.Model.Load(Path.Combine(AppContext.BaseDirectory, "taxi_price_guesser_cs.zip"), out var schema);
var predictionEngine = mlContext.Model.CreatePredictionEngine<TaxiInput, TaxiOutput>(model);

app.MapPost("/api/predict", (TaxiInput Input) =>
{
    var timefullbackend_cs = Stopwatch.StartNew();
    var prediction = predictionEngine.Predict(Input);
    
    timefullbackend_cs.Stop();
    Console.WriteLine($"Backend_cs: full backend operation time: {timefullbackend_cs.ElapsedMilliseconds} ms");
    return Results.Ok(new
    {
        predicted_cost = prediction.Score
    });
});
app.MapGet("/health", () => Results.Ok(new
{
    status = "ok"
}));

app.Run();