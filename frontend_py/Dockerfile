FROM python:3.13-slim

RUN pip install uv

WORKDIR /app

COPY . .

RUN uv sync --no-dev

CMD ["sh", "-c", "uv run streamlit run dashboard.py \
    --server.port=${PORT:-8501} \
    --server.address=0.0.0.0"]

