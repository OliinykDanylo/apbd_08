using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace DevicesManager.Logic;

public class DeviceService<T> : IDeviceService<T> where T : Device, new()
{
    private readonly string _connectionString;

    public DeviceService(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public void Post(Device device)
    {
        if (device == null)
        {
            throw new ArgumentNullException(nameof(device), "Device cannot be null.");
        }

        if (string.IsNullOrEmpty(device.Id))
        {
            throw new ArgumentException("Device Id cannot be null or empty.");
        }

        if (string.IsNullOrEmpty(device.Name))
        {
            throw new ArgumentException("Device Name cannot be null or empty.");
        }

        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        // Insert into Device table
        string query = "INSERT INTO Device (Id, Name, IsEnabled) VALUES (@Id, @Name, @IsEnabled)";
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id", device.Id);
            command.Parameters.AddWithValue("@Name", device.Name);
            command.Parameters.AddWithValue("@IsEnabled", device.IsEnabled);
            command.ExecuteNonQuery();
        }

        // Insert into specific table
        if (device is SmartWatch smartwatch)
        {
            query = "INSERT INTO Smartwatch (BatteryPercentage, DeviceId) VALUES (@BatteryPercentage, @DeviceId)";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@BatteryPercentage", smartwatch.BatteryLevel);
            command.Parameters.AddWithValue("@DeviceId", smartwatch.Id);
            command.ExecuteNonQuery();
        }
        else if (device is PersonalComputer pc)
        {
            query = "INSERT INTO PersonalComputer (OperationSystem, DeviceId) VALUES (@OperationSystem, @DeviceId)";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DeviceId", pc.Id);
            command.Parameters.AddWithValue("@OperationSystem", pc.OperatingSystem);
            command.ExecuteNonQuery();
        }
        else if (device is EmbeddedDevice embedded)
        {
            query = "INSERT INTO Embedded (IpAddress, NetworkName, DeviceId) VALUES (@IpAddress, @NetworkName, @DeviceId)";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DeviceId", embedded.Id);
            command.Parameters.AddWithValue("@IpAddress", embedded.IpAddress);
            command.Parameters.AddWithValue("@NetworkName", embedded.NetworkName);
            command.ExecuteNonQuery();
        }
    }

    public List<T> GetAllDevices()
    {
        var devices = new List<T>();

        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT Id, Name, IsEnabled FROM Device";
        using var command = new SqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            devices.Add(new T
            {
                Id = reader["Id"].ToString(),
                Name = reader["Name"].ToString(),
                IsEnabled = (bool)reader["IsEnabled"]
            });
        }

        return devices;
    }
    
    public Device GetDeviceById(string id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        
        var query = "SELECT Id, Name, IsEnabled FROM Device WHERE Id = @Id";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }
        
        var deviceId = reader["Id"].ToString();
        var deviceName = reader["Name"].ToString();
        var isEnabled = (bool)reader["IsEnabled"];
        
        if (deviceId.StartsWith("SW-"))
        {
            reader.Close();
            query = "SELECT BatteryPercentage FROM Smartwatch WHERE DeviceId = @DeviceId";
            using var smartwatchCommand = new SqlCommand(query, connection);
            smartwatchCommand.Parameters.AddWithValue("@DeviceId", id);

            using var smartwatchReader = smartwatchCommand.ExecuteReader();
            if (smartwatchReader.Read())
            {
                return new SmartWatch
                {
                    Id = deviceId,
                    Name = deviceName,
                    IsEnabled = isEnabled,
                    BatteryLevel = (int)smartwatchReader["BatteryPercentage"]
                };
            }
        }
        else if (deviceId.StartsWith("P-"))
        {
            reader.Close();
            query = "SELECT OperationSystem FROM PersonalComputer WHERE DeviceId = @DeviceId";
            using var pcCommand = new SqlCommand(query, connection);
            pcCommand.Parameters.AddWithValue("@DeviceId", id);

            using var pcReader = pcCommand.ExecuteReader();
            if (pcReader.Read())
            {
                return new PersonalComputer
                {
                    Id = deviceId,
                    Name = deviceName,
                    IsEnabled = isEnabled,
                    OperatingSystem = pcReader["OperationSystem"].ToString()
                };
            }
        }
        else if (deviceId.StartsWith("E-"))
        {
            reader.Close();
            query = "SELECT IpAddress, NetworkName FROM Embedded WHERE DeviceId = @DeviceId";
            using var embeddedCommand = new SqlCommand(query, connection);
            embeddedCommand.Parameters.AddWithValue("@DeviceId", id);

            using var embeddedReader = embeddedCommand.ExecuteReader();
            if (embeddedReader.Read())
            {
                return new EmbeddedDevice
                {
                    Id = deviceId,
                    Name = deviceName,
                    IsEnabled = isEnabled,
                    IpAddress = embeddedReader["IpAddress"].ToString(),
                    NetworkName = embeddedReader["NetworkName"].ToString()
                };
            }
        }

        return null;
    }

    public bool EditDevice(Device device)
    {
        if (device == null)
        {
            throw new ArgumentNullException(nameof(device), "Device cannot be null.");
        }

        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        // Update Device table
        var query = "UPDATE Device SET Name = @Name, IsEnabled = @IsEnabled WHERE Id = @Id";
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id", device.Id);
            command.Parameters.AddWithValue("@Name", device.Name);
            command.Parameters.AddWithValue("@IsEnabled", device.IsEnabled);
            command.ExecuteNonQuery();
        }

        // Update specific table
        if (device is SmartWatch smartwatch)
        {
            query = "UPDATE Smartwatch SET BatteryPercentage = @BatteryPercentage WHERE DeviceId = @DeviceId";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@BatteryPercentage", smartwatch.BatteryLevel);
            command.Parameters.AddWithValue("@DeviceId", smartwatch.Id);
            command.ExecuteNonQuery();
        }
        else if (device is PersonalComputer pc)
        {
            query = "UPDATE PersonalComputer SET OperationSystem = @OperationSystem WHERE DeviceId = @DeviceId";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@OperationSystem", pc.OperatingSystem);
            command.Parameters.AddWithValue("@DeviceId", pc.Id);
            command.ExecuteNonQuery();
        }
        else if (device is EmbeddedDevice embedded)
        {
            query = "UPDATE Embedded SET IpAddress = @IpAddress, NetworkName = @NetworkName WHERE DeviceId = @DeviceId";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@IpAddress", embedded.IpAddress);
            command.Parameters.AddWithValue("@NetworkName", embedded.NetworkName);
            command.Parameters.AddWithValue("@DeviceId", embedded.Id);
            command.ExecuteNonQuery();
        }

        return true;
    }
    
    public bool DeleteDevice(string id, string deviceType)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        
        switch (deviceType)
        {
            case "SW":
                var deleteSmartwatchesQuery = "DELETE FROM Smartwatch WHERE DeviceId = @DeviceId";
                using (var deleteSmartwatchesCommand = new SqlCommand(deleteSmartwatchesQuery, connection))
                {
                    deleteSmartwatchesCommand.Parameters.AddWithValue("@DeviceId", id);
                    deleteSmartwatchesCommand.ExecuteNonQuery();
                }
                break;

            case "P":
                var deletePCsQuery = "DELETE FROM PersonalComputer WHERE DeviceId = @DeviceId";
                using (var deletePCsCommand = new SqlCommand(deletePCsQuery, connection))
                {
                    deletePCsCommand.Parameters.AddWithValue("@DeviceId", id);
                    deletePCsCommand.ExecuteNonQuery();
                }
                break;

            case "E":
                var deleteEmbeddedQuery = "DELETE FROM Embedded WHERE DeviceId = @DeviceId";
                using (var deleteEmbeddedCommand = new SqlCommand(deleteEmbeddedQuery, connection))
                {
                    deleteEmbeddedCommand.Parameters.AddWithValue("@DeviceId", id);
                    deleteEmbeddedCommand.ExecuteNonQuery();
                }
                break;

            default:
                throw new ArgumentException("Invalid device type.");
        }
        
        var deleteDeviceQuery = "DELETE FROM Device WHERE Id = @Id";
        using (var deleteDeviceCommand = new SqlCommand(deleteDeviceQuery, connection))
        {
            deleteDeviceCommand.Parameters.AddWithValue("@Id", id);
            return deleteDeviceCommand.ExecuteNonQuery() > 0;
        }
    }
}