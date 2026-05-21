import streamlit as st
import sys
from pathlib import Path
sys.path.append(str(Path(__file__).parent.parent.resolve()))
from helpers import read_api_endpoint, post_api_endpoint
import pandas as pd

data = read_api_endpoint("/api")
df = pd.DataFrame(data.json())

def layout():
    st.markdown("# Give taxi trip information")

    with st.form("data"):
        tripdistancekm = st.number_input(
            "Trip length (km)", value=6.0, step=0.1
        )
        timeofday = st.text_input(
            "Sepal width (Morning, Afternoon, Evening, Night)", value="Morning"
        )
        dayofweek = st.text_input(
            "Day of week (Weekday, Weekend)", value="Weekday"
        )
        passangercount = st.number_input(
            "Amount of passangers", value=1, step=1
        )
        trafficcon = st.text_input(
            "Traffic conditions (Low, Medium, High)", value="Low"
        )
        weather = st.text_input(
            "Weather (Clear, Rain, Snow)", value="Clear"
        )
        basefare = st.number_input(
            "Base fare", value=4.0, step=0.1
        )
        perkmrate = st.number_input(
            "Rate per km", value=1.0, step=0.1
        )
        perminrate = st.number_input(
            "Rate per minute", value=0.5, step=0.1
        )
        tripdurationmin = st.number_input(
            "Trip duration (min)", value=50, step=1
        )

        submitted = st.form_submit_button("PREDICT COST")

    if submitted:
        payload = {
            "Trip_Distance_km": tripdistancekm,
            "Time_of_Day": timeofday,
            "Day_of_Week": dayofweek,
            "Passenger_Count": passangercount,
            "Traffic_Conditions": trafficcon,
            "Weather": weather,
            "Base_Fare": basefare,
            "Per_Km_Rate": perkmrate,
            "Per_Minute_Rate": perminrate,
            "Trip_Duration_Minutes": tripdurationmin,
        }
        response = post_api_endpoint(payload, endpoint="/api/predict")
        predicted_cost = response.json().get("predicted_cost")
        st.markdown(f"Predicted cost: {predicted_cost}")
    
    st.markdown("## Raw data")
    st.dataframe(df)

if __name__ == "__main__":
    layout()