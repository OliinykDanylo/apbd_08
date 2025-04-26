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
    
    // public void Post(Device device, string type)
    // {
    //     if (device == null)
    //     {
    //         throw new ArgumentNullException(nameof(device), "Device cannot be null.");
    //     }
    //
    //     if (string.IsNullOrEmpty(device.Id))
    //     {
    //         throw new ArgumentException("Device Id cannot be null or empty.");
    //     }
    //
    //     if (string.IsNullOrEmpty(device.Name))
    //     {
    //         throw new ArgumentException("Device Name cannot be null or empty.");
    //     }
    //
    //     using var connection = new SqlConnection(_connectionString);
    //     connection.Open();
    //
    //     // Insert into the Device table
    //     string query = "INSERT INTO Device (Id, Name, IsEnabled) VALUES (@Id, @Name, @IsEnabled)";
    //     using (var command = new SqlCommand(query, connection))
    //     {
    //         command.Parameters.AddWithValue("@Id", device.Id);
    //         command.Parameters.AddWithValue("@Name", device.Name);
    //         command.Parameters.AddWithValue("@IsEnabled", device.IsEnabled);
    //         command.ExecuteNonQuery();
    //     }
    //
    //     // Insert into the specific table based on the type
    //     if (type.Equals("SW"))
    //     {
    //         var smartwatchJson = JsonSerializer.Serialize(device);
    //         var smartwatch = JsonSerializer.Deserialize<SmartWatch>(smartwatchJson)!;
    //
    //         query = "INSERT INTO Smartwatch (BatteryPercentage, DeviceId) VALUES (@BatteryPercentage, @DeviceId)";
    //         using var command = new SqlCommand(query, connection);
    //         command.Parameters.AddWithValue("@BatteryPercentage", smartwatch.BatteryLevel);
    //         command.Parameters.AddWithValue("@DeviceId", smartwatch.Id);
    //         command.ExecuteNonQuery();
    //     }
    //     else if (type.Equals("P"))
    //     {
    //         var pcJson = JsonSerializer.Serialize(device);
    //         var pc = JsonSerializer.Deserialize<PersonalComputer>(pcJson)!;
    //
    //         query = "INSERT INTO PersonalComputer (OperationSystem, DeviceId) VALUES (@OperationSystem, @DeviceId)";
    //         using var command = new SqlCommand(query, connection);
    //         command.Parameters.AddWithValue("@DeviceId", pc.Id);
    //         command.Parameters.AddWithValue("@OperationSystem", pc.OperatingSystem);
    //         command.ExecuteNonQuery();
    //     }
    //     else if (type.Equals("E"))
    //     {
    //         var embeddedJson = JsonSerializer.Serialize(device);
    //         var embedded = JsonSerializer.Deserialize<EmbeddedDevice>(embeddedJson)!;
    //
    //         query = "INSERT INTO Embedded (IpAddress, NetworkName, DeviceId) VALUES (@IpAddress, @NetworkName, @DeviceId)";
    //         using var command = new SqlCommand(query, connection);
    //         command.Parameters.AddWithValue("@DeviceId", embedded.Id);
    //         command.Parameters.AddWithValue("@IpAddress", embedded.IpAddress);
    //         command.Parameters.AddWithValue("@NetworkName", embedded.NetworkName);
    //         command.ExecuteNonQuery();
    //     }
    //     else
    //     {
    //         throw new ArgumentException("Unknown device type.");
    //     }
    // }
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

    public T GetDeviceById(string id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT Id, Name, IsEnabled FROM Device WHERE Id = @Id";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new T
            {
                Id = reader["Id"].ToString(),
                Name = reader["Name"].ToString(),
                IsEnabled = (bool)reader["IsEnabled"]
            };
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
                var deletePCsQuery = "DELETE FROM PC WHERE DeviceId = @DeviceId";
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