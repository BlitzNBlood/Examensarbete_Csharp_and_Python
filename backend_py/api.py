from fastapi import FastAPI
from data_processing import CostData, UserInput, PredictionOutput
import joblib
import pandas as pd 

app = FastAPI()
input = CostData()

@app.get("/api")
def read_data():
    return input.to_json()

@app.post("/api/predict", response_model=PredictionOutput)
def predict_cost(payload: UserInput):
    data_to_predict = pd.DataFrame([payload.model_dump()])
    clf = joblib.load("taxi_price_guesser.joblib")
    prediction = clf.predict(data_to_predict)
    return {"predicted_cost": float(prediction[0])}