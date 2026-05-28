from fastapi import FastAPI
from data_processing import UserInput, PredictionOutput
import joblib
import pandas as pd 
import time

app = FastAPI()

@app.post("/api/predict", response_model=PredictionOutput)
def predict_cost(payload: UserInput):
    start = time.perf_counter()
    data_to_predict = pd.DataFrame([payload.model_dump()])
    clf = joblib.load("taxi_price_guesser.joblib")
    start_gen = time.perf_counter()
    prediction = clf.predict(data_to_predict)
    stop_gen = time.perf_counter()
    stop = time.perf_counter()
    print(f"backend_py: AI generation took {(stop_gen - start_gen) * 1000:.2f} ms")
    print(f"backend_py: loading AI and generating took {(stop - start) * 1000:.2f} ms")
    return {"predicted_cost": float(prediction[0])}

@app.get("/health")
def health():
    return {"status": "ok"}