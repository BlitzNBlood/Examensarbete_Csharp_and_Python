import pandas as pd
from pprint import pprint
from pydantic import BaseModel, Field

df = pd.read_csv("taxi_trip_pricing.csv")
for columns in df:
    if (columns == 'Base_Fare' or columns == 'Per_Km_Rate' or columns == 'Per_Minute_Rate'):
        loop_count = 0
        for rows in df[columns]:
            if (pd.isnull(df[columns].iloc[loop_count]) == True):
                df.loc[loop_count, columns] = 0.0
            loop_count = loop_count + 1
    else:
        df = df[df[columns].isnull() == False]
        df = df.reset_index(drop=True)

class CostData:
    def __init__(self):
        self.df = df

    def to_json(self):
        return self.df.to_dict(orient="records")

class UserInput(BaseModel):
    Trip_Distance_km: float = Field(6)
    Time_of_Day: str = Field("Morning")
    Day_of_Week: str = Field("Weekday")
    Passenger_Count: float = Field(1)
    Traffic_Conditions: str = Field("Low")
    Weather: str = Field("Clear")
    Base_Fare: float = Field(4)
    Per_Km_Rate: float = Field(1)
    Per_Minute_Rate: float = Field(0.5)
    Trip_Duration_Minutes: float = Field(50)

class PredictionOutput(BaseModel):
    predicted_cost: float

if __name__ == "__main__":
    data_explorer = CostData()
    pprint(data_explorer.json_response())