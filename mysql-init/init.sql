DO
$$
BEGIN
  IF NOT EXISTS(SELECT 1 FROM pg_type WHERE typname = 'addressTypeEnum') THEN
    CREATE TYPE addressTypeEnum as ENUM ('Billing', 'Shipping');
  END IF;
END
$$;

CREATE TABLE manufacturer(
  manufacturerId SERIAL PRIMARY KEY,
  name varchar(255) NOT NULL UNIQUE
);

CREATE TABLE category(
  categoryId SERIAL PRIMARY KEY,
  name varchar(255) NOT NULL UNIQUE
);

CREATE TABLE "user"(
  userId SERIAL PRIMARY KEY,
  name varchar(255) NOT NULL,
  lastname varchar(255) NOT NULL,
  email varchar(255) NOT NULL UNIQUE,
  password varchar(255) NOT NULL
);

CREATE TABLE address(
  addressId SERIAL PRIMARY KEY,
  userId int NOT NULL,
  addressType addressTypeEnum NOT NULL,
  country varchar(255) NOT NULL,
  city varchar(255) NOT NULL,
  location varchar(255) NOT NULL,
  zipCode varchar(15) NOT NULL,
  FOREIGN KEY (userId) REFERENCES "user"(userId)
);

CREATE TABLE paymentDetails(
  paymentDetailsId SERIAL PRIMARY KEY,
  userId int NOT NULL,
  paymentToken varchar(4) NOT NULL,
  cardholderName varchar(255) NOT NULL,
  cardType varchar(50) NOT NULL,
  lastFourDigits varchar(4) NOT NULL,
  expDate Date NOT NULL,
  FOREIGN KEY (userId) REFERENCES "user"(userId)
);

CREATE TABLE product(
  productId SERIAL PRIMARY KEY,
  name varchar(255) NOT NULL,
  description varchar(1000) NOT NULL,
  price DECIMAL(6, 2) NOT NULL,
  manufacturerId int NOT NULL,
  categoryId int NOT NULL,
  FOREIGN KEY (manufacturerId) REFERENCES manufacturer(manufacturerId),
  FOREIGN KEY (categoryId) REFERENCES category(categoryId)
);

CREATE TABLE cartProduct(
  cartProductId SERIAL PRIMARY KEY,
  userId int NOT NULL,
  productId int NOT NULL,
  quantity int NOT NULL,
  FOREIGN KEY (userId) REFERENCES "user"(userId),
  FOREIGN KEY (productId) REFERENCES product(productId)
);