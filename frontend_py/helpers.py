import requests
from urllib.parse import urljoin
import time


def post_api_endpoint(payload, endpoint="/", base_url=None):
    #byt ut "http://backend_cs:8000" med "http://backend_py:8000" eller tvärt om beroende på vilken backend du vill använda.
    base_url = "http://backend_cs:8000"
    url = urljoin(base_url, endpoint)
    response = requests.post(url=url, json=payload)
    return response
    #start_adress = time.perf_counter() ## detta användes för problemlösning medans projektet kodades.
    #if base_urls is None:
    #    base_urls = [
    #        "http://backend_py:8000",
    #        "http://backend_cs:8000"
    #    ]

    #for base_url in base_urls:
    #    try:
    #        url = urljoin(base_url, endpoint)
    #        response = requests.post(url=url, json=payload)

    #        if response.ok:
    #            stop_adress = time.perf_counter()
    #            print(f"frontend_py: finding adress took: {(stop_adress - start_adress) * 1000:.2f} ms")
    #            return response
    #
    #    except requests.RequestException:
    #        continue

    #raise Exception("All backend APIs failed")