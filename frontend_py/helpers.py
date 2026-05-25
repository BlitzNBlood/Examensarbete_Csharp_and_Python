import requests
from urllib.parse import urljoin


def post_api_endpoint(payload, endpoint="/", base_url="http://127.0.0.1:8000"):
    url = urljoin(base_url, endpoint)
    response = requests.post(url=url, json=payload)

    return response