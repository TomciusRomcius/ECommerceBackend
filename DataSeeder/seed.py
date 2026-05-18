#!/usr/bin/env python3
import logging
import sys

import config
from client import ApiClient

logging.basicConfig(
    level=logging.INFO,
    format="%(asctime)s %(levelname)s %(message)s",
)
logger = logging.getLogger(__name__)


def _index_by_name(items: list[dict], id_field: str, name_key: str = "name") -> dict[str, int]:
    return {
        item[name_key]: item[id_field]
        for item in items
        if item.get(name_key) and item.get(id_field) is not None
    }

# returns dict(name, id)
def _load_categories(client: ApiClient) -> dict[str, int]:
    data = client.get_json("/productservice/categories", params={"pageNumber": 0})
    if isinstance(data, list):
        return _index_by_name(data, "categoryId")
    return {}

# returns dict(name, id)
def _load_manufacturers(client: ApiClient) -> dict[str, int]:
    data = client.get_json("/productservice/manufacturer", params={"pageNumber": 0})
    if isinstance(data, list):
        return _index_by_name(data, "manufacturerId")
    return {}

# returns dict(name, id)
def _load_products(client: ApiClient) -> dict[str, int]:
    data = client.get_json(
        "/productservice/product",
        params={"pageNumber": 0, "pageSize": 100},
    )
    if isinstance(data, dict) and isinstance(data.get("data"), list):
        return _index_by_name(data["data"], "productId")
    return {}

# returns dict(name, id)
def _load_store_locations(client: ApiClient) -> dict[str, int]:
    data = client.get_json("/storeservice/storelocation", params={"pageNumber": 0})
    if not isinstance(data, list):
        return {}
    return {
        loc["displayName"]: loc["storeLocationId"]
        for loc in data
        if loc.get("displayName") and loc.get("storeLocationId") is not None
    }

# returns set(productId)
def _load_product_links(client: ApiClient, store_location_id: int) -> set[int]:
    data = client.get_json(
        "/storeservice/productstorelocation",
        params={"storeLocationId": store_location_id, "pageNumber": 0},
    )
    if not isinstance(data, list):
        return set()
    return {
        link["productId"]
        for link in data
        if link.get("productId") is not None
    }

# Returns (id, error)
def find_or_create_category(
    client: ApiClient, existing: dict[str, int], name: str
) -> tuple[int | None, bool]:
    if name in existing:
        logger.info("Category %s already exists (id=%s)", name, existing[name])
        return existing[name], False

    result = client.post_json("/productservice/categories", {"name": name})
    if result and "categoryId" in result:
        category_id = result["categoryId"]
        existing[name] = category_id
        logger.info("Created category %s (id=%s)", name, category_id)
        return category_id, False

    logger.error("Failed to create category %s", name)
    return None, True

# Returns (id, error)
def find_or_create_manufacturer(
    client: ApiClient, existing: dict[str, int], name: str
) -> tuple[int | None, bool]:
    if name in existing:
        logger.info("Manufacturer %s already exists (id=%s)", name, existing[name])
        return existing[name], False

    result = client.post_json("/productservice/manufacturer", {"name": name})
    if result and "manufacturerId" in result:
        manufacturer_id = result["manufacturerId"]
        existing[name] = manufacturer_id
        logger.info("Created manufacturer %s (id=%s)", name, manufacturer_id)
        return manufacturer_id, False

    logger.error("Failed to create manufacturer %s", name)
    return None, True

# Returns (id, error)
def find_or_create_product(
    client: ApiClient,
    existing: dict[str, int],
    name: str,
    description: str,
    price: float,
    manufacturer_id: int,
    category_id: int,
) -> tuple[int | None, bool]:
    if name in existing:
        logger.info("Product %s already exists (id=%s)", name, existing[name])
        return existing[name], False

    result = client.post_json(
        "/productservice/product",
        {
            "name": name,
            "description": description,
            "price": price,
            "manufacturerId": manufacturer_id,
            "categoryId": category_id,
        },
    )
    if result and "productId" in result:
        product_id = result["productId"]
        existing[name] = product_id
        logger.info("Created product %s (id=%s)", name, product_id)
        return product_id, False

    logger.error("Failed to create product %s", name)
    return None, True

# Returns (id, error)
def find_or_create_store_location(
    client: ApiClient, existing: dict[str, int], display_name: str, address: str
) -> tuple[int | None, bool]:
    if display_name in existing:
        logger.info(
            "Store location %s already exists (id=%s)",
            display_name,
            existing[display_name],
        )
        return existing[display_name], False

    result = client.post_json(
        "/storeservice/storelocation",
        {"displayName": display_name, "address": address},
    )
    if result and "storeLocationId" in result:
        store_location_id = result["storeLocationId"]
        existing[display_name] = store_location_id
        logger.info(
            "Created store location %s (id=%s)", display_name, store_location_id
        )
        return store_location_id, False

    logger.error("Failed to create store location %s", display_name)
    return None, True

# Returns true if succeeds
def link_product_to_store(
    client: ApiClient,
    existing_links: set[int],
    store_location_id: int,
    product_id: int,
) -> bool:
    if product_id in existing_links:
        logger.info(
            "Product %s already linked to store location %s",
            product_id,
            store_location_id,
        )
        return False

    result = client.post_json(
        "/storeservice/productstorelocation",
        {
            "storeLocationId": store_location_id,
            "productId": product_id,
            "stock": 20,
        },
    )
    if result is not None:
        existing_links.add(product_id)
        logger.info(
            "Linked product %s to store location %s", product_id, store_location_id
        )
        return False

    logger.error(
        "Failed to link product %s to store location %s",
        product_id,
        store_location_id,
    )
    return True

# returns (list(id), isError)
def seed_categories(client: ApiClient) -> tuple[list[int], bool]:
    existing = _load_categories(client)
    ids: list[int] = []
    failed = False
    for name in config.CATEGORY_NAMES:
        category_id, item_failed = find_or_create_category(client, existing, name)
        if category_id is None:
            failed = True
        else:
            ids.append(category_id)
    return ids, failed

# returns (list(id), isError)
def seed_manufacturers(client: ApiClient) -> tuple[list[int], bool]:
    existing = _load_manufacturers(client)
    ids: list[int] = []
    failed = False
    for name in config.MANUFACTURER_NAMES:
        manufacturer_id, item_failed = find_or_create_manufacturer(
            client, existing, name
        )
        if item_failed or manufacturer_id is None:
            failed = True
        else:
            ids.append(manufacturer_id)
    return ids, failed

# returns (list(id), isError)
def seed_products(
    client: ApiClient,
    category_ids: list[int],
    manufacturer_ids: list[int],
) -> tuple[list[list[int]], bool]:
    if not category_ids or not manufacturer_ids:
        return [], True

    existing = _load_products(client)
    products_by_store: list[list[int]] = []
    failed = False

    for store_num in range(1, config.STORE_COUNT + 1):
        store_products: list[int] = []
        for product_num in range(1, config.PRODUCTS_PER_STORE + 1):
            idx = (store_num + product_num - 2) % len(category_ids)
            name = f"Seed S{store_num}-P{product_num}"
            description = f"Seed product {product_num} for store {store_num}"
            price = round(9.99 + store_num + product_num * 0.5, 2)

            product_id, item_failed = find_or_create_product(
                client,
                existing,
                name,
                description,
                price,
                manufacturer_ids[idx % len(manufacturer_ids)],
                category_ids[idx % len(category_ids)],
            )
            if item_failed or product_id is None:
                failed = True
            else:
                store_products.append(product_id)

        products_by_store.append(store_products)

    return products_by_store, failed

# returns (list(id), isError)
def seed_store_locations(client: ApiClient) -> tuple[list[int], bool]:
    existing = _load_store_locations(client)
    ids: list[int] = []
    failed = False
    for store_num in range(1, config.STORE_COUNT + 1):
        display_name = f"{config.SEED_STORE_PREFIX} {store_num}"
        address = f"{100 + store_num} Seed Street, City {store_num}"
        store_location_id, item_failed = find_or_create_store_location(
            client, existing, display_name, address
        )
        if item_failed or store_location_id is None:
            failed = True
        else:
            ids.append(store_location_id)
    return ids, failed

# returns isError bool
def link_products_to_stores(
    client: ApiClient,
    store_location_ids: list[int],
    products_by_store: list[list[int]],
) -> bool:
    failed = False
    for store_idx, product_ids in enumerate(products_by_store):
        if store_idx >= len(store_location_ids):
            failed = True
            continue
        store_location_id = store_location_ids[store_idx]
        existing_links = _load_product_links(client, store_location_id)
        for product_id in product_ids:
            if link_product_to_store(
                client, existing_links, store_location_id, product_id
            ):
                failed = True
    return failed


def main() -> int:
    client = ApiClient()

    if not client.get_admin_token():
        return 1

    failed = False

    category_ids, cat_failed = seed_categories(client)

    manufacturer_ids, mfr_failed = seed_manufacturers(client)

    products_by_store, prod_failed = seed_products(
        client, category_ids, manufacturer_ids
    )

    store_location_ids, store_failed = seed_store_locations(client)
    failed |= cat_failed | mfr_failed | prod_failed | store_failed

    if link_products_to_stores(client, store_location_ids, products_by_store):
        failed = True

    total_products = config.STORE_COUNT * config.PRODUCTS_PER_STORE
    if failed:
        logger.error("Seeding finished with failures")
        return 1

    logger.info(
        "Seeding complete: %d categories, %d manufacturers, %d products, "
        "%d store locations, %d product-store links",
        len(category_ids),
        len(manufacturer_ids),
        total_products,
        len(store_location_ids),
        total_products,
    )
    return 0


if __name__ == "__main__":
    sys.exit(main())
