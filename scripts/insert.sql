-- Insert base devices into Device table
INSERT INTO Device (Id, Name, IsEnabled) VALUES
                                             ('SW-1', 'FitPro', 0),
                                             ('SW-2', 'Galaxy Watch', 1),
                                             ('P-1', 'Office PC', 1),
                                             ('P-2', 'Gaming Rig', 0),
                                             ('E-1', 'Sensor A', 1),
                                             ('E-2', 'Sensor B', 0);

-- Insert smartwatches
INSERT INTO Smartwatch (BatteryPercentage, DeviceId) VALUES
                                                         (85, 'SW-1'),
                                                         (30, 'SW-2');

-- Insert personal computers
INSERT INTO PersonalComputer (OperationSystem, DeviceId) VALUES
                                                             ('Windows 11', 'P-1'),
                                                             ('Ubuntu', 'P-2');

-- Insert embedded devices
INSERT INTO Embedded (IpAddress, NetworkName, DeviceId) VALUES
                                                            ('192.168.0.101', 'MD Ltd. Sensors', 'E-1'),
                                                            ('192.168.0.102', 'MD Ltd. Systems', 'E-2');