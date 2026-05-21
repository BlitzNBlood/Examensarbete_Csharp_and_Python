constants import DATA_PATH
dataframe df = read_csv(DATA_PATH / "taxi_trip_pricing.csv");
for columns in df
    {
        if (columns == "Base_Fare" || columns == "Per_Km_Rate" || columns == "Per_Minute_Rate")
        {
            int loopCount = 0;
            for rows in df[columns]
            {
                df.loc[loopCount, columns] = 0.0;
            }
            loopCount = loopCount + 1;
        }
        else
        {
            df = df[df[columns].isnull() == False]
            df = df.reset_index(drop=True)
        }
    }
class CostData
    {
        def __init__(self)
        {
            self.df = df
        }
        def to_json(self):
        {
            return self.df.to_dict(orient="records")
        }
    }

    class PredictionOutput(BaseModel):
    predicted_cost: float