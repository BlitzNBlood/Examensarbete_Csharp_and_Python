from pydantic import BaseModel, Field

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