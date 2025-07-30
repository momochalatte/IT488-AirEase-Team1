CREATE DATABASE AirEase; 
USE AirEase;


--Flight Table
CREATE TABLE flight (
flightID INT Primary Key IDENTITY(1,1),
flightNumber VARCHAR(20) UNIQUE,
Destination VARCHAR(35),
carrier_name VARCHAR(35),
departure VARCHAR(35),
DepartureDateTime DATETIME,
ArrivalDateTime DATETIME,
seat_booked INT
);

--Passenger Table

CREATE TABLE Passenger(
PassengerID INT IDENTITY(1,1) PRIMARY KEY,
FirstName VARCHAR(50),
LastName VARCHAR(50),
passenger_dob VARCHAR(30),
ticket_number VARCHAR (20),
Gender VARCHAR(20)
);

--Airport Table
CREATE TABLE Airport(
airport_code VARCHAR(50) PRIMARY KEY,
airport_location VARCHAR(50) NOT NULL
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
CREATE TABLE flight_user(
Username VARCHAR(20)PRIMARY KEY,
password VARCHAR(20) NOT NULL,
);

SET IDENTITY_INSERT flight ON;

INSERT INTO flight (flightID, flightNumber, Destination, carrier_name, departure, DepartureDateTime, ArrivalDateTime, seat_booked ) VALUES
(1, 1000, 'London', 'Air Europa', 'New York', '08:00pm', '03:00am', 105),
(2, 1001, 'Hong Kong', 'Hong Kong Airlines', 'Tokyo', '12:00pm', '4:30pm',106 ),
(3, 1002, 'Philadelphia', 'United Airlines', 'Orlando', '08:00am', '10:05am', 107),
(4, 1003, 'New Orleans', 'Southwest', 'Denver', '01:30pm', '03:45pm',108 ),
(5, 1004, 'Honolulu', 'Hawaiian Airlines', 'Alaska', '03:00am', '08:45am',109 ),
(6, 1005, 'Las Vegas', 'United Airlines', 'Austin', '01:30pm', '03:30pm',110 ),
(7, 1006, 'Paris', 'Trasnsavia France', 'Madrid', '12:45pm', '02:50pm',111 );
SET IDENTITY_INSERT flight OFF; 

INSERT INTO Passenger (PassengerID, FirstName, LastName, passenger_dob, ticket_number, gender) VALUES
(8,'Olivia', 'Ramirez', '1989-04-15', 'A734521689', 'Female'),
(9,'Marcus', 'Chen', '1995-08-09', 'B248973502', 'Male'),
(10,'Hannah', 'OConnel', '1982-12-03', 'C592760184', 'Female'),
(11,'Leo', 'Takahashi', '2001-02-21', 'D803216749', 'Male'),
(12,'Priya','Deshmukh','1990-07-30','E479103685','Female'),
(13,'Ethan','Blackwell','1976-10-17','F120853976','Male'),
(14,'Naomi','St. James','1984-06-25','G913205471','Female'),
(15,'Cat', 'Cristy', '1990,05,25', 'S578956412','Female'),
(16,'Moriah','Allen','1990,06,06', 'D11236547', 'Female'),
(17,'Ruhshona', '', '1990,04,22', 'C125897456', 'Female'),
(18,'Logan', 'B', '1990,12,15', 'CF123698745', 'Male');

INSERT INTO Airport ( airport_code, airport_location) VALUES
('LHR', 'London'),
('JKF','New York'),
('HKG','Hong Kong'),
('TYO','Toyko'),
('PHL','Philadelphia'),
('MCO','Orlando'),
('MYS','New Orleans'),
('DIA','Denver'),
('HNL','Honolulu'),
('ANC','Anchorage'),
('LAS','Las Vegas'),
('AUS','Austin'),
('CDG','Roussy-en-France'),
('MAD','Madrid');