FROM python:3.13-slim

RUN pip install uv

WORKDIR /app

COPY . .

RUN uv sync --no-dev

CMD ["uv", "run", "uvicorn", "api:app", "--host", "0.0.0.0", "--port", "8000", "--reload"]

