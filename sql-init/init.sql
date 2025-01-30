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

CREATE TABLE roleTypes(
  roleTypeId SERIAL PRIMARY KEY,
  name varchar(255) NOT NULL UNIQUE
);

CREATE TABLE userRoles(
  userId uuid NOT NULL,
  roleTypeId int NOT NULL,
  PRIMARY KEY (userId, roleTypeId),
  FOREIGN KEY (userId) REFERENCES users(userId),  
  FOREIGN KEY (roleTypeId) REFERENCES roleTypes(roleTypeId) 
);

CREATE TABLE addresses(
  userId uuid NOT NULL UNIQUE,
  isShipping BOOLEAN NOT NULL, -- 0 - billing, 1 - shipping
  recipientName varchar(100) NOT NULL,
  streetAddress varchar(255) NOT NULL,
  apartmentUnit varchar(255),
  country varchar(255) NOT NULL,
  city varchar(255) NOT NULL,
  state varchar(255) NOT NULL,
  postalCode varchar(15) NOT NULL,
  mobileNumber varchar(50) NOT NULL,
  PRIMARY KEY (userId, isShipping),
  FOREIGN KEY (userId) REFERENCES "users"(userId) ON DELETE CASCADE
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
  stock int NOT NULL,
  manufacturerId int NOT NULL,
  categoryId int NOT NULL,
  FOREIGN KEY (manufacturerId) REFERENCES manufacturers(manufacturerId),
  FOREIGN KEY (categoryId) REFERENCES categories(categoryId)
);

CREATE TABLE cartProducts(
  userId uuid NOT NULL,
  productId int NOT NULL UNIQUE,
  quantity int NOT NULL CHECK (quantity > 0),
  PRIMARY KEY (userId, productId),
  FOREIGN KEY (userId) REFERENCES "users"(userId) ON DELETE CASCADE,
  FOREIGN KEY (productId) REFERENCES products(productId) ON DELETE CASCADE
);