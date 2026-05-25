from fastapi import FastAPI
from data_processing import UserInput, PredictionOutput
import joblib
import pandas as pd 

app = FastAPI()

@app.post("/api/predict", response_model=PredictionOutput)
def predict_cost(payload: UserInput):
    data_to_predict = pd.DataFrame([payload.model_dump()])
    clf = joblib.load("taxi_price_guesser.joblib")
    prediction = clf.predict(data_to_predict)
    return {"predicted_cost": float(prediction[0])}