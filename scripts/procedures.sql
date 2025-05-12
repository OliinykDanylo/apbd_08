-- =====================================
-- DEVICE TABLE PROCEDURES
-- =====================================

CREATE PROCEDURE AddDevice
    @Id VARCHAR(255),
    @Name NVARCHAR(255),
    @IsEnabled BIT
AS
BEGIN
    INSERT INTO Device (Id, Name, IsEnabled)
    VALUES (@Id, @Name, @IsEnabled);
END;
GO

ALTER TABLE Device
ADD RowVersion ROWVERSION;

CREATE OR ALTER PROCEDURE UpdateDevice
    @Id VARCHAR(255),
    @Name NVARCHAR(255),
    @IsEnabled BIT,
    @OldRowVersion ROWVERSION
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Device
    SET Name = @Name,
        IsEnabled = @IsEnabled
    WHERE Id = @Id AND RowVersion = @OldRowVersion;

    IF @@ROWCOUNT = 0
        THROW 50000, 'Concurrency conflict: device was modified by another user.', 1;
END;

CREATE PROCEDURE DeleteDevice
    @Id VARCHAR(255)
AS
BEGIN
    DELETE FROM Device
    WHERE Id = @Id;
END;
GO

-- =====================================
-- SMARTWATCH PROCEDURES
-- =====================================

CREATE PROCEDURE AddSmartWatch
    @Id VARCHAR(255),
    @BatteryPercentage INT
AS
BEGIN
    INSERT INTO Smartwatch (BatteryPercentage, DeviceId)
    VALUES (@BatteryPercentage, @Id);
END;
GO

CREATE PROCEDURE UpdateSmartWatch
    @Id VARCHAR(255),
    @BatteryPercentage INT
AS
BEGIN
    UPDATE Smartwatch
    SET BatteryPercentage = @BatteryPercentage
    WHERE DeviceId = @Id;
END;
GO

CREATE PROCEDURE DeleteSmartWatch
    @Id VARCHAR(255)
AS
BEGIN
    DELETE FROM Smartwatch
    WHERE DeviceId = @Id;
END;
GO

-- =====================================
-- PERSONAL COMPUTER PROCEDURES
-- =====================================

CREATE PROCEDURE AddPersonalComputer
    @Id VARCHAR(255),
    @OperationSystem VARCHAR(255)
AS
BEGIN
    INSERT INTO PersonalComputer (OperationSystem, DeviceId)
    VALUES (@OperationSystem, @Id);
END;
GO

CREATE PROCEDURE UpdatePersonalComputer
    @Id VARCHAR(255),
    @OperationSystem VARCHAR(255)
AS
BEGIN
    UPDATE PersonalComputer
    SET OperationSystem = @OperationSystem
    WHERE DeviceId = @Id;
END;
GO

CREATE PROCEDURE DeletePersonalComputer
    @Id VARCHAR(255)
AS
BEGIN
    DELETE FROM PersonalComputer
    WHERE DeviceId = @Id;
END;
GO

-- =====================================
-- EMBEDDED DEVICE PROCEDURES
-- =====================================

CREATE PROCEDURE AddEmbeddedDevice
    @Id VARCHAR(255),
    @IpAddress VARCHAR(255),
    @NetworkName VARCHAR(255)
AS
BEGIN
    INSERT INTO Embedded (IpAddress, NetworkName, DeviceId)
    VALUES (@IpAddress, @NetworkName, @Id);
END;
GO

CREATE PROCEDURE UpdateEmbeddedDevice
    @Id VARCHAR(255),
    @IpAddress VARCHAR(255),
    @NetworkName VARCHAR(255)
AS
BEGIN
    UPDATE Embedded
    SET IpAddress = @IpAddress,
        NetworkName = @NetworkName
    WHERE DeviceId = @Id;
END;
GO

CREATE PROCEDURE DeleteEmbeddedDevice
    @Id VARCHAR(255)
AS
BEGIN
    DELETE FROM Embedded
    WHERE DeviceId = @Id;
END;
GO