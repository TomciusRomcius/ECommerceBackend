DO
$$
BEGIN
  IF NOT EXISTS(SELECT 1 FROM pg_type WHERE typname = 'addressTypeEnum') THEN
    CREATE TYPE addressTypeEnum as ENUM ('Billing', 'Shipping');
  END IF;
END
$$;

CREATE TABLE manufacturers(
  manufacturerId SERIAL PRIMARY KEY,
  name varchar(255) NOT NULL UNIQUE
);

CREATE TABLE categories(
  categoryId SERIAL PRIMARY KEY,
  name varchar(255) NOT NULL UNIQUE
);

CREATE TABLE "users"(
  userId uuid NOT NULL,
  email varchar(255) NOT NULL UNIQUE,
  passwordHash varchar(255) NOT NULL,
  firstname varchar(255) NOT NULL,
  lastname varchar(255) NOT NULL,
  PRIMARY KEY (userId)
);

CREATE TABLE addresses(
  addressId SERIAL PRIMARY KEY,
  userId uuid NOT NULL,
  addressType addressTypeEnum NOT NULL,
  country varchar(255) NOT NULL,
  city varchar(255) NOT NULL,
  location varchar(255) NOT NULL,
  zipCode varchar(15) NOT NULL,
  FOREIGN KEY (userId) REFERENCES "users"(userId)
);

CREATE TABLE paymentDetails(
  paymentDetailsId SERIAL PRIMARY KEY,
  userId uuid NOT NULL,
  paymentToken varchar(4) NOT NULL,
  cardholderName varchar(255) NOT NULL,
  cardType varchar(50) NOT NULL,
  lastFourDigits varchar(4) NOT NULL,
  expDate Date NOT NULL,
  FOREIGN KEY (userId) REFERENCES "users"(userId)
);

CREATE TABLE products(
  productId SERIAL PRIMARY KEY,
  name varchar(255) NOT NULL,
  description varchar(1000) NOT NULL,
  price DECIMAL(6, 2) NOT NULL,
  manufacturerId int NOT NULL,
  categoryId int NOT NULL,
  FOREIGN KEY (manufacturerId) REFERENCES manufacturers(manufacturerId),
  FOREIGN KEY (categoryId) REFERENCES categories(categoryId)
);

CREATE TABLE cartProducts(
  cartProductId SERIAL PRIMARY KEY,
  userId uuid NOT NULL,
  productId int NOT NULL,
  quantity int NOT NULL,
  FOREIGN KEY (userId) REFERENCES "users"(userId),
  FOREIGN KEY (productId) REFERENCES products(productId)
);