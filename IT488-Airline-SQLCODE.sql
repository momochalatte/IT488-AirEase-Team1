--Flight Table
CREATE TABLE Flight (
FLightID INT Primary Key,
FlightNumber VARCHAR(20) UNIQUE,
DepartureDateTime DATETIME,
ArrivalDateTime DATETIME,
OriginAirPortCode VARCHAR(3),
DestinationAirportCode VARCHAR(3),
AvailableSeat INT,
FOREIGN KEY (OriginAirportCode) REFERENCES Airport(AirportCode),
FOREIGN KEY (DestinationAirportCode) REFERENCES Airport(AirportCode)
);

--Passenger Table
CREATE TABLE Passenger(
PassengerID INT PRIMARY KEY,
FirstName VARCHAR(50),
LastName VARCHAR(50),
EmailAddress VARCHAR(30),
PassportNumber VARCHAR (20),
Gender VARCHAR(20)
);

--Airline Table
CREATE TABLE Airline(
AirlineID INT PRIMARY KEY,
AirlineName VARCHAR(100),
ContactNumber VARCHAR(20),
OperatingRegion VARCHAR(100)
);

--Booking Table
CREATE TABLE Booking (
BookingID INT PRIMARY KEY,
FlightID INT,
PassengerID INT,
PaymentStatus VARCHAR(20),
FOREIGN KEY (FlightID) REFERENCES Flight(FlightID),
FOREIGN KEY (PassengerID) REFERENCES Passenger(PassengerID)
);

--Payment Table
CREATE TABLE Payment (
PaymentID INT PRIMARY KEY,
BookingID INT UNIQUE,
PaymentMethod VARCHAR(50),
cheque_no VARCHAR(15),
card_no VARCHAR(20),
Amount DECIMAL(10, 2),
TransactionDateTime DATETIME,
FOREIGN KEY (BookingID) REFERENCES Booking(BookingID)
);

--Table Cancellation
CREATE TABLE Cancellation(
PNR_no VARCHAR(10),
Cancellation_no VARCHAR(10),
Cancellation_dtate DATE, Fli_code VARCHAR(15)
);

-- Table Login
CREATE TABLE Login(
Username VARCHAR(20),
Password VARCHAR(20)
);



