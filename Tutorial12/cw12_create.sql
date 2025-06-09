-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2021-04-20 04:46:38.52

-- tables
-- Table: Client
CREATE TABLE Client (
    IdClient int  NOT NULL IDENTITY,
    FirstName nvarchar(120)  NOT NULL,
    LastName nvarchar(120)  NOT NULL,
    Email nvarchar(120)  NOT NULL,
    Telephone nvarchar(120)  NOT NULL,
    Pesel nvarchar(120)  NOT NULL,
    CONSTRAINT Client_pk PRIMARY KEY  (IdClient)
);

-- Table: Client_Trip
CREATE TABLE Client_Trip (
    IdClient int  NOT NULL,
    IdTrip int  NOT NULL,
    RegisteredAt datetime  NOT NULL,
    PaymentDate datetime  NULL,
    CONSTRAINT Client_Trip_pk PRIMARY KEY  (IdClient,IdTrip)
);

-- Table: Country
CREATE TABLE Country (
    IdCountry int  NOT NULL IDENTITY,
    Name nvarchar(120)  NOT NULL,
    CONSTRAINT Country_pk PRIMARY KEY  (IdCountry)
);

-- Table: Country_Trip
CREATE TABLE Country_Trip (
    IdCountry int  NOT NULL,
    IdTrip int  NOT NULL,
    CONSTRAINT Country_Trip_pk PRIMARY KEY  (IdCountry,IdTrip)
);

-- Table: Trip
CREATE TABLE Trip (
    IdTrip int  NOT NULL IDENTITY,
    Name nvarchar(120)  NOT NULL,
    Description nvarchar(220)  NOT NULL,
    DateFrom datetime  NOT NULL,
    DateTo datetime  NOT NULL,
    MaxPeople int  NOT NULL,
    CONSTRAINT Trip_pk PRIMARY KEY  (IdTrip)
);

-- foreign keys
-- Reference: Country_Trip_Country (table: Country_Trip)
ALTER TABLE Country_Trip ADD CONSTRAINT Country_Trip_Country
    FOREIGN KEY (IdCountry)
    REFERENCES Country (IdCountry);

-- Reference: Country_Trip_Trip (table: Country_Trip)
ALTER TABLE Country_Trip ADD CONSTRAINT Country_Trip_Trip
    FOREIGN KEY (IdTrip)
    REFERENCES Trip (IdTrip);

-- Reference: Table_5_Client (table: Client_Trip)
ALTER TABLE Client_Trip ADD CONSTRAINT Table_5_Client
    FOREIGN KEY (IdClient)
    REFERENCES Client (IdClient);

-- Reference: Table_5_Trip (table: Client_Trip)
ALTER TABLE Client_Trip ADD CONSTRAINT Table_5_Trip
    FOREIGN KEY (IdTrip)
    REFERENCES Trip (IdTrip);

-- End of file.



-- Insert Countries
INSERT INTO Country (Name) VALUES ('Poland'), ('Germany'), ('Italy');

-- Insert Trips
INSERT INTO Trip (Name, Description, DateFrom, DateTo, MaxPeople)
VALUES 
('Rome Adventure', 'Trip to Rome', '2025-05-01', '2025-05-10', 20),
('Berlin Tour', 'Trip to Berlin', '2025-06-01', '2025-06-07', 15);

-- Link Countries to Trips
INSERT INTO Country_Trip (IdCountry, IdTrip)
SELECT c.IdCountry, t.IdTrip
FROM Country c, Trip t
WHERE (c.Name = 'Italy' AND t.Name = 'Rome Adventure')
   OR (c.Name = 'Germany' AND t.Name = 'Berlin Tour')
   OR (c.Name = 'Poland' AND t.Name = 'Rome Adventure');

-- Insert a Client (for delete test)
INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel)
VALUES ('Test', 'User', 'test@example.com', '123-456-789', '90010112345');
