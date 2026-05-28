import requests
from urllib.parse import urljoin
import time


def post_api_endpoint(payload, endpoint="/", base_urls=None):
    start_adress = time.perf_counter()
    if base_urls is None:
        base_urls = [
            "http://backend_py:8000",
            "http://backend_cs:8000"
        ]

    for base_url in base_urls:
        try:
            url = urljoin(base_url, endpoint)
            response = requests.post(url=url, json=payload)

            if response.ok:
                stop_adress = time.perf_counter()
                print(f"frontend_py: finding adress took: {(stop_adress - start_adress) * 1000:.2f} ms")
                return response

        except requests.RequestException:
            continue

    raise Exception("All backend APIs failed")