using Microsoft.ML;
using Microsoft.Data.Analysis;
using System.Globalization;
using System.Text.Json;

class MLTEST
{
    static void Main(string[] args)
    {
        var ml = new MLContext();

        var df = DataFrame.LoadCsv("taxi_trip_pricing.csv");
        foreach (string columns in df.Columns.Select(column => column.Name))
            {
                if (columns == "Base_Fare" || columns == "Per_Km_Rate" || columns == "Per_Minute_Rate")
                {
                    int loopCount = 0;
                    foreach (var rows in df.Rows)
                    {
                        if (df.Columns[columns][loopCount] == null)
                        {
                            df.Columns[columns][loopCount] = 0.0;
                        }
                        loopCount = loopCount + 1;
                    }
                }
                else
                {
                    int loopCount = 0;
                    while (true)
                    {
                        if (loopCount >= df.Rows.Count)
                        {
                            break;
                        }
                        if (df.Columns[columns][loopCount] == null)
                        {
                            df = df[Enumerable.Range(0, (int)df.Rows.Count).Where(i => i != loopCount)];
                        }
                        else
                        {
                            loopCount = loopCount + 1;
                        }
                    }
                }
            }
            
        var cell = df.Columns[0][0];

        Console.WriteLine(cell?.GetType());
        var dataList = new List<DataShape>();
        for (long i = 0; i < df.Rows.Count; i++)
        {
            float.TryParse(df["Trip_Distance_km"][i]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var a);
            float.TryParse(df["Passenger_Count"][i]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var b);
            float.TryParse(df["Base_Fare"][i]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var c);
            float.TryParse(df["Per_Km_Rate"][i]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var d);
            float.TryParse(df["Per_Minute_Rate"][i]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var e);
            float.TryParse(df["Trip_Duration_Minutes"][i]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var f);
            float.TryParse(df["Trip_Price"][i]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var g);
            
            if (a == 0 || b == 0 || g == 0)
            {
                Console.WriteLine("Possible parsing failure row");
            };
            dataList.Add(new DataShape
            {
                Trip_Distance_km = a,
                Time_of_Day = df["Time_of_Day"][i].ToString(),
                Day_of_Week = df["Day_of_Week"][i].ToString(),
                Passenger_Count = b,
                Traffic_Conditions = df["Traffic_Conditions"][i].ToString(),
                Weather = df["Weather"][i].ToString(),
                Base_Fare = c,
                Per_Km_Rate = d,
                Per_Minute_Rate = e,
                Trip_Duration_Minutes = f,
                Trip_Price = g,
            });
        }
        IDataView data = ml.Data.LoadFromEnumerable(dataList);
        var pipeline = ml.Transforms.Categorical.OneHotEncoding(
                "Time_of_Day_Encoded",
                nameof(DataShape.Time_of_Day)
            )
            .Append(
                ml.Transforms.Categorical.OneHotEncoding(
                    "Day_of_Week_Encoded",
                    nameof(DataShape.Day_of_Week)
                )
            )
            .Append(
                ml.Transforms.Categorical.OneHotEncoding(
                    "Traffic_Conditions_Encoded",
                    nameof(DataShape.Traffic_Conditions)
                )
            )
            .Append(
                ml.Transforms.Categorical.OneHotEncoding(
                    "Weather_Encoded",
                    nameof(DataShape.Weather)
                )
            )
            .Append(
            ml.Transforms.Concatenate(
                "Features",
                nameof(DataShape.Trip_Distance_km),
                "Time_of_Day_Encoded",
                "Day_of_Week_Encoded",
                nameof(DataShape.Passenger_Count),
                "Traffic_Conditions_Encoded",
                "Weather_Encoded",
                nameof(DataShape.Base_Fare),
                nameof(DataShape.Per_Km_Rate),
                nameof(DataShape.Per_Minute_Rate),
                nameof(DataShape.Trip_Duration_Minutes)
            ))
            .Append(ml.Regression.Trainers.FastTree(
            labelColumnName: nameof(DataShape.Trip_Price),
            featureColumnName: "Features"));
        var model = pipeline.Fit(data);
        var predictions = model.Transform(data);

        var metrics = ml.Regression.Evaluate(
            predictions,
            labelColumnName: nameof(DataShape.Trip_Price),
            scoreColumnName: "Score"
        );
        Console.WriteLine(metrics.RSquared);
        ml.Model.Save(model, data.Schema, "taxi_price_guesser_cs.zip");
    }
}