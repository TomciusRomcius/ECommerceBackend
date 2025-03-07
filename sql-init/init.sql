CREATE TABLE manufacturers(
  manufacturerId SERIAL PRIMARY KEY,
  name VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE categories(
  categoryId SERIAL PRIMARY KEY,
  name VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE "users"(
  userId UUID NOT NULL,
  email VARCHAR(255) NOT NULL UNIQUE,
  passwordHash VARCHAR(255) NOT NULL,
  firstname VARCHAR(255) NOT NULL,
  lastname VARCHAR(255) NOT NULL,
  PRIMARY KEY (userId)
);

CREATE TABLE paymentSessions(
  userId UUID NOT NULL UNIQUE,
  paymentSessionId varchar(255) NOT NULL UNIQUE,
  paymentSessionProvider varchar(50) NOT NULL,
  PRIMARY KEY (paymentSessionId),
  FOREIGN KEY (userId) REFERENCES "users"(userId)
);

CREATE TABLE roleTypes(
  roleTypeId SERIAL PRIMARY KEY,
  name VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE userRoles(
  userId UUID NOT NULL,
  roleTypeId INT NOT NULL,
  PRIMARY KEY (userId, roleTypeId),
  FOREIGN KEY (userId) REFERENCES users(userId),  
  FOREIGN KEY (roleTypeId) REFERENCES roleTypes(roleTypeId) 
);

CREATE TABLE shippingAddresses(
  shippingAddressId BIGSERIAL PRIMARY KEY,
  userId uuid NOT NULL,
  recipientName VARCHAR(100) NOT NULL,
  streetAddress VARCHAR(255) NOT NULL,
  apartmentUnit VARCHAR(255),
  country VARCHAR(255) NOT NULL,
  city VARCHAR(255) NOT NULL,
  state VARCHAR(255) NOT NULL,
  postalCode VARCHAR(15) NOT NULL,
  mobileNumber VARCHAR(50) NOT NULL,
  isDefault BOOLEAN DEFAULT false,
  FOREIGN KEY (userId) REFERENCES "users"(userId) ON DELETE CASCADE
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
  storeLocationId SERIAL PRIMARY KEY,
  displayName VARCHAR(255) UNIQUE,
  address VARCHAR(255) UNIQUE
);

CREATE TABLE productStoreLocations(
  storeLocationId INT NOT NULL,
  productId INT NOT NULL,
  stock INT NOT NULL,
  PRIMARY KEY (productId, storeLocationId),
  FOREIGN KEY (storeLocationId) REFERENCES storeLocations(storeLocationId) ON DELETE CASCADE,
  FOREIGN KEY (productId) REFERENCES products(productId) ON DELETE CASCADE
);

CREATE TABLE cartProducts(
  userId UUID NOT NULL,
  productId INT NOT NULL UNIQUE,
  storeLocationId INT NOT NULL UNIQUE,
  quantity INT NOT NULL CHECK (quantity > 0),
  PRIMARY KEY (userId, productId, storeLocationId),
  FOREIGN KEY (userId) REFERENCES "users"(userId) ON DELETE CASCADE,
  FOREIGN KEY (productId, storeLocationId) REFERENCES productStoreLocations(productId, storeLocationId) ON DELETE CASCADE
);

CREATE TABLE orders (
  orderId SERIAL NOT NULL,
  userId UUID NOT NULL,
  shippingAddressId INT NOT NULL,
  placedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  fulfilled BOOLEAN NOT NULL,
  PRIMARY KEY (userId, orderId),
  FOREIGN KEY (userId) REFERENCES "users"(userId) ON DELETE CASCADE,
  FOREIGN KEY (shippingAddressId) REFERENCES shippingAddresses(shippingAddressId)
);

CREATE TABLE orderProducts(
  userId UUID NOT NULL UNIQUE,
  orderId SERIAL NOT NULL UNIQUE,
  -- Store product information again because the product can be updated
  -- as time goes on 
  name VARCHAR(255) NOT NULL UNIQUE,
  price DECIMAL(6, 2) NOT NULL,
  quantity INT NOT NULL CHECK (quantity > 0),
  productId INT NOT NULL UNIQUE,
  PRIMARY KEY (userId, orderId),
  FOREIGN KEY (userId) REFERENCES "users"(userId) ON DELETE CASCADE,
  FOREIGN KEY (userId, orderId) REFERENCES orders(userId, orderId) ON DELETE CASCADE,
  FOREIGN KEY (productId) REFERENCES products(productId) ON DELETE CASCADE
);