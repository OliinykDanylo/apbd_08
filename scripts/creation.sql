-- Drop existing tables to avoid conflicts
DROP TABLE IF EXISTS Embedded;
DROP TABLE IF EXISTS PersonalComputer;
DROP TABLE IF EXISTS Smartwatch;
DROP TABLE IF EXISTS Device;

-- Create a Device table
CREATE TABLE Device (
                        Id VARCHAR(255) PRIMARY KEY,
                        Name NVARCHAR(255),
                        IsEnabled BIT
);

-- Create an Embedded table
CREATE TABLE Embedded (
                          Id INT IDENTITY(1,1) PRIMARY KEY,
                          IpAddress VARCHAR(255),
                          NetworkName VARCHAR(255),
                          DeviceId VARCHAR(255),
                          FOREIGN KEY (DeviceId) REFERENCES Device(Id)
);

-- Create a PersonalComputer table
CREATE TABLE PersonalComputer (
                                  Id INT IDENTITY(1,1) PRIMARY KEY,
                                  OperationSystem VARCHAR(255),
                                  DeviceId VARCHAR(255),
                                  FOREIGN KEY (DeviceId) REFERENCES Device(Id)
);

-- Create a Smartwatch table
CREATE TABLE Smartwatch (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            BatteryPercentage INT,
                            DeviceId VARCHAR(255),
                            FOREIGN KEY (DeviceId) REFERENCES Device(Id)
);