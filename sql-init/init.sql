CREATE TABLE manufacturers(
  manufacturerId SERIAL PRIMARY KEY,
  name VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE categories(
  categoryId SERIAL PRIMARY KEY,
  name VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE "users"(
  userId uuid NOT NULL,
  email VARCHAR(255) NOT NULL UNIQUE,
  passwordHash VARCHAR(255) NOT NULL,
  firstname VARCHAR(255) NOT NULL,
  lastname VARCHAR(255) NOT NULL,
  PRIMARY KEY (userId)
);

CREATE TABLE roleTypes(
  roleTypeId SERIAL PRIMARY KEY,
  name VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE userRoles(
  userId uuid NOT NULL,
  roleTypeId INT NOT NULL,
  PRIMARY KEY (userId, roleTypeId),
  FOREIGN KEY (userId) REFERENCES users(userId),  
  FOREIGN KEY (roleTypeId) REFERENCES roleTypes(roleTypeId) 
);

CREATE TABLE addresses(
  userId uuid NOT NULL UNIQUE,
  isShipping BOOLEAN NOT NULL, -- 0 - billing, 1 - shipping
  recipientName VARCHAR(100) NOT NULL,
  streetAddress VARCHAR(255) NOT NULL,
  apartmentUnit VARCHAR(255),
  country VARCHAR(255) NOT NULL,
  city VARCHAR(255) NOT NULL,
  state VARCHAR(255) NOT NULL,
  postalCode VARCHAR(15) NOT NULL,
  mobileNumber VARCHAR(50) NOT NULL,
  PRIMARY KEY (userId, isShipping),
  FOREIGN KEY (userId) REFERENCES "users"(userId) ON DELETE CASCADE
);

CREATE TABLE paymentDetails(
  paymentDetailsId SERIAL PRIMARY KEY,
  userId uuid NOT NULL,
  paymentToken VARCHAR(4) NOT NULL,
  cardholderName VARCHAR(255) NOT NULL,
  cardType VARCHAR(50) NOT NULL,
  lastFourDigits VARCHAR(4) NOT NULL,
  expDate Date NOT NULL,
  FOREIGN KEY (userId) REFERENCES "users"(userId)
);

CREATE TABLE products(
  productId SERIAL PRIMARY KEY,
  name VARCHAR(255) NOT NULL,
  description VARCHAR(1000) NOT NULL,
  price DECIMAL(6, 2) NOT NULL,
  manufacturerId INT NOT NULL,
  categoryId INT NOT NULL,
  FOREIGN KEY (manufacturerId) REFERENCES manufacturers(manufacturerId),
  FOREIGN KEY (categoryId) REFERENCES categories(categoryId)
);

CREATE TABLE storeLocations(
  storeLocationId INT PRIMARY KEY,
  displayName VARCHAR(255) UNIQUE,
  address VARCHAR(255) UNIQUE
);

CREATE TABLE productStoreLocations(
  productId INT NOT NULL,
  storeLocationId INT NOT NULL,
  stock INT NOT NULL,
  PRIMARY KEY (productId, storeLocationId)
);

CREATE TABLE cartProducts(
  userId uuid NOT NULL,
  productId INT NOT NULL UNIQUE,
  storeLocationId INT NOT NULL UNIQUE,
  quantity INT NOT NULL CHECK (quantity > 0),
  PRIMARY KEY (userId, productId),
  FOREIGN KEY (userId) REFERENCES "users"(userId) ON DELETE CASCADE,
  FOREIGN KEY (productId, storeLocationId) REFERENCES productStoreLocations(productId, storeLocationId) ON DELETE CASCADE
);