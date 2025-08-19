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

INSERT INTO Booking (BookingID, flightID, PassengerID, PaymentStatus, SeatNumber)
VALUES
(101, 1, 8, 'Paid', '12A'),
(102, 2, 9, 'Pending', '7C'),
(103, 3, 10, 'Cancelled', '14B'),
(104, 4, 17, 'Paid', '3D'),
(105, 5, 15, 'Paid', '1A'),
(106, 6, 16, 'Pending', '9F'),
(107, 7, 18, 'Paid', '6E'),
(108, 3, 11, 'Cancelled', '14B'),
(109, 4, 12, 'Paid', '3D'),
(110, 5, 13, 'Paid', '1A'),
(111, 6, 14, 'Pending', '9F');

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

--ADD EMAIL

  ALTER TABLE Passenger
  ADD Email VARCHAR(50);

UPDATE Passenger SET Email = 'olivia.ramirez@airease.com' WHERE PassengerID = 8;
UPDATE Passenger SET Email = 'marcus.chen@airease.com' WHERE PassengerID = 9;
UPDATE Passenger SET Email = 'hannah.oconnel@airease.com' WHERE PassengerID = 10;
UPDATE Passenger SET Email = 'leo.takahashi@airease.com' WHERE PassengerID = 11;
UPDATE Passenger SET Email = 'priya.deshmukh@airease.com' WHERE PassengerID = 12;
UPDATE Passenger SET Email = 'ethan.blackwell@airease.com' WHERE PassengerID = 13;
UPDATE Passenger SET Email = 'naomi.stjames@airease.com' WHERE PassengerID = 14;
UPDATE Passenger SET Email = 'cat.cristy@airease.com' WHERE PassengerID = 15;
UPDATE Passenger SET Email = 'marth.allen@airease.com' WHERE PassengerID = 16;
UPDATE Passenger SET Email = 'ruhshona@airease.com' WHERE PassengerID = 17;
UPDATE Passenger SET Email = 'logan.b@airease.com' WHERE PassengerID = 18;

INSERT INTO [AirEase].[dbo].[Payment] (
    BookingID, PaymentMethod, cheque_no, card_no, Amount, TransactionDateTime
)
SELECT 
    b.BookingID,
    -- Assign a default method for unpaid (could be updated later)
    'Pending' AS PaymentMethod,
    NULL AS cheque_no,
    NULL AS card_no,
    ROUND(100 + (b.BookingID * 2.5), 2) AS Amount,
    GETDATE() AS TransactionDateTime
FROM [AirEase].[dbo].[Booking] b
WHERE b.PaymentStatus IN ('Pending', 'Cancelled');

INSERT INTO [AirEase].[dbo].[Payment] (
    BookingID, PaymentMethod, cheque_no, card_no, Amount, TransactionDateTime
)
SELECT 
    b.BookingID,
    CASE 
        WHEN b.BookingID % 3 = 0 THEN 'CreditCard'
        WHEN b.BookingID % 3 = 1 THEN 'Cheque'
        ELSE 'Cash'
    END AS PaymentMethod,
    CASE 
        WHEN b.BookingID % 3 = 1 THEN CONCAT('CHK', b.BookingID, '001')
        ELSE NULL
    END AS cheque_no,
    CASE 
        WHEN b.BookingID % 3 = 0 THEN 
            CASE 
                WHEN b.BookingID % 2 = 0 THEN '4111111111111111'
                ELSE '5500000000000004'
            END
        ELSE NULL
    END AS card_no,
    ROUND(100 + (b.BookingID * 3.75), 2) AS Amount,
    GETDATE() AS TransactionDateTime
FROM [AirEase].[dbo].[Booking] b
WHERE b.PaymentStatus = 'Paid';

INSERT INTO flight_user (
    Username, password, Email, SecurityQuestion, SecurityAnswer, PhoneNumber, PreferredLanguage
)
VALUES
-- Admin user
('admin01', 'hashed_admin_pw', 'admin@airease.com', 
 'What is your favorite aircraft?', 'boeing747', '555-0101', 'English'),

-- Passenger users
('olivia.ramirez', 'hashed_pw1', 'olivia.ramirez@airease.com', 
 'What is your pet’s name?', 'luna', '555-0303', 'Spanish'),

('marcus.chen', 'hashed_pw2', 'marcus.chen@airease.com', 
 'What is your favorite color?', 'blue', '555-0404', 'Mandarin'),

('hannah.oconnel', 'hashed_pw3', 'hannah.oconnel@airease.com', 
 'What was your first car?', 'civic', '555-0505', 'English'),

('leo.takahashi', 'hashed_pw4', 'leo.takahashi@airease.com', 
 'What is your mother’s maiden name?', 'sato', '555-0606', 'Japanese');

 INSERT INTO flight_user (
    Username, password, Email, SecurityQuestion, SecurityAnswer, PhoneNumber, PreferredLanguage
)
VALUES
('priya.deshmukh', 'hashed_pw5', 'priya.deshmukh@airease.com', 
 'What is your favorite book?', '1984', '555-0707', 'Hindi'),

('ethan.blackwell', 'hashed_pw6', 'ethan.blackwell@airease.com', 
 'What was your childhood nickname?', 'e-dawg', '555-0808', 'English'),

('naomi.stjames', 'hashed_pw7', 'naomi.stjames@airease.com', 
 'What is your dream destination?', 'bali', '555-0909', 'French'),

('cat.cristy', 'hashed_pw8', 'cat.cristy@airease.com', 
 'What is your favorite movie?', 'inception', '555-1010', 'English'),

('marth.allen', 'hashed_pw9', 'marth.allen@airease.com', 
 'What is your favorite season?', 'spring', '555-1111', 'English'),

('ruhshona', 'hashed_pw10', 'ruhshona@airease.com', 
 'What is your favorite flower?', 'tulip', '555-1212', 'Uzbek'),

('logan.b', 'hashed_pw11', 'logan.b@airease.com', 
 'What is your favorite sport?', 'soccer', '555-1313', 'English');

 INSERT INTO [AirEase].[dbo].[Payment] (
    BookingID, PaymentMethod, cheque_no, card_no, Amount, TransactionDateTime
)
SELECT 
    b.BookingID,
    -- Assign a default method for unpaid (could be updated later)
    'Pending' AS PaymentMethod,
    NULL AS cheque_no,
    NULL AS card_no,
    ROUND(100 + (b.BookingID * 2.5), 2) AS Amount,
    GETDATE() AS TransactionDateTime
FROM [AirEase].[dbo].[Booking] b
WHERE b.PaymentStatus IN ('Pending', 'Cancelled');

INSERT INTO [AirEase].[dbo].[Payment] (
    BookingID, PaymentMethod, cheque_no, card_no, Amount, TransactionDateTime
)
SELECT 
    b.BookingID,
    CASE 
        WHEN b.BookingID % 3 = 0 THEN 'CreditCard'
        WHEN b.BookingID % 3 = 1 THEN 'Cheque'
        ELSE 'Cash'
    END AS PaymentMethod,
    CASE 
        WHEN b.BookingID % 3 = 1 THEN CONCAT('CHK', b.BookingID, '001')
        ELSE NULL
    END AS cheque_no,
    CASE 
        WHEN b.BookingID % 3 = 0 THEN 
            CASE 
                WHEN b.BookingID % 2 = 0 THEN '4111111111111111'
                ELSE '5500000000000004'
            END
        ELSE NULL
    END AS card_no,
    ROUND(100 + (b.BookingID * 3.75), 2) AS Amount,
    GETDATE() AS TransactionDateTime
FROM [AirEase].[dbo].[Booking] b
WHERE b.PaymentStatus = 'Paid';
