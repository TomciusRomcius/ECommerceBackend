import os

API_BASE_URL = os.environ.get("API_BASE_URL", "http://reverse-proxy")
KEYCLOAK_TOKEN_URL = os.environ.get(
    "KEYCLOAK_TOKEN_URL",
    "http://keycloak:8080/realms/ecommerce-api/protocol/openid-connect/token",
)
KEYCLOAK_CLIENT_ID = os.environ.get("KEYCLOAK_CLIENT_ID", "ecommerce-api")
KEYCLOAK_CLIENT_SECRET = os.environ.get("KEYCLOAK_CLIENT_SECRET", "secret")

STORE_COUNT = int(os.environ.get("STORE_COUNT", "3"))
PRODUCTS_PER_STORE = int(os.environ.get("PRODUCTS_PER_STORE", "6"))
REQUEST_TIMEOUT_SEC = int(os.environ.get("REQUEST_TIMEOUT_SEC", "30"))

SEED_STORE_PREFIX = "Seed Store"

CATEGORY_NAMES = ["Shoes", "Electronics", "Home"]
MANUFACTURER_NAMES = ["Acme Corp", "Globex", "Initech"]
