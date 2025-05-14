namespace DevicesManager.Logic;

using Microsoft.Data.SqlClient;
using System.Data;

public class DeviceRepository<T> : IDeviceRepository<T> where T : Device, new()
{
    private readonly string _connectionString;

    public DeviceRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<T> GetAll()
    {
        var devices = new List<T>();

        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        string query = "SELECT Id, Name, IsEnabled FROM Device";
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
    
    public Device? GetById(string id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        
        var deviceQuery = "SELECT Id, Name, IsEnabled FROM Device WHERE Id = @Id";
        using var deviceCommand = new SqlCommand(deviceQuery, connection);
        deviceCommand.Parameters.AddWithValue("@Id", id);

        using var deviceReader = deviceCommand.ExecuteReader();
        if (!deviceReader.Read())
        {
            return null;
        }

        string deviceId = deviceReader["Id"].ToString()!;
        string name = deviceReader["Name"].ToString()!;
        bool isEnabled = (bool)deviceReader["IsEnabled"];
        deviceReader.Close();
        
        if (deviceId.StartsWith("SW-"))
        {
            var swQuery = "SELECT BatteryPercentage FROM Smartwatch WHERE DeviceId = @DeviceId";
            using var swCommand = new SqlCommand(swQuery, connection);
            swCommand.Parameters.AddWithValue("@DeviceId", id);

            using var swReader = swCommand.ExecuteReader();
            if (swReader.Read())
            {
                return new SmartWatch
                {
                    Id = deviceId,
                    Name = name,
                    IsEnabled = isEnabled,
                    BatteryLevel = (int)swReader["BatteryPercentage"]
                };
            }
        }
        else if (deviceId.StartsWith("P-"))
        {
            var pcQuery = "SELECT OperationSystem FROM PersonalComputer WHERE DeviceId = @DeviceId";
            using var pcCommand = new SqlCommand(pcQuery, connection);
            pcCommand.Parameters.AddWithValue("@DeviceId", id);

            using var pcReader = pcCommand.ExecuteReader();
            if (pcReader.Read())
            {
                return new PersonalComputer
                {
                    Id = deviceId,
                    Name = name,
                    IsEnabled = isEnabled,
                    OperatingSystem = pcReader["OperationSystem"].ToString()
                };
            }
        }
        else if (deviceId.StartsWith("E-"))
        {
            var embeddedQuery = "SELECT IpAddress, NetworkName FROM Embedded WHERE DeviceId = @DeviceId";
            using var embeddedCommand = new SqlCommand(embeddedQuery, connection);
            embeddedCommand.Parameters.AddWithValue("@DeviceId", id);

            using var embeddedReader = embeddedCommand.ExecuteReader();
            if (embeddedReader.Read())
            {
                return new EmbeddedDevice
                {
                    Id = deviceId,
                    Name = name,
                    IsEnabled = isEnabled,
                    IpAddress = embeddedReader["IpAddress"].ToString(),
                    NetworkName = embeddedReader["NetworkName"].ToString()
                };
            }
        }

        return null;
    }

    // public void Add(T device)
    // {
    //     using var connection = new SqlConnection(_connectionString);
    //     connection.Open();
    //     using var transaction = connection.BeginTransaction();
    //
    //     try
    //     {
    //         using (var command = new SqlCommand("AddDevice", connection, transaction))
    //         {
    //             command.CommandType = CommandType.StoredProcedure;
    //             command.Parameters.AddWithValue("@Id", device.Id);
    //             command.Parameters.AddWithValue("@Name", device.Name);
    //             command.Parameters.AddWithValue("@IsEnabled", device.IsEnabled);
    //             command.ExecuteNonQuery();
    //         }
    //
    //         if (device is SmartWatch sw)
    //         {
    //             using var command = new SqlCommand("AddSmartWatch", connection, transaction);
    //             command.CommandType = CommandType.StoredProcedure;
    //             command.Parameters.AddWithValue("@Id", sw.Id);
    //             command.Parameters.AddWithValue("@BatteryPercentage", sw.BatteryLevel);
    //             command.ExecuteNonQuery();
    //         }
    //         else if (device is PersonalComputer pc)
    //         {
    //             using var command = new SqlCommand("AddPersonalComputer", connection, transaction);
    //             command.CommandType = CommandType.StoredProcedure;
    //             command.Parameters.AddWithValue("@Id", pc.Id);
    //             command.Parameters.AddWithValue("@OperationSystem", (object?)pc.OperatingSystem ?? DBNull.Value);
    //             command.ExecuteNonQuery();
    //         }
    //         else if (device is EmbeddedDevice ed)
    //         {
    //             using var command = new SqlCommand("AddEmbeddedDevice", connection, transaction);
    //             command.CommandType = CommandType.StoredProcedure;
    //             command.Parameters.AddWithValue("@Id", ed.Id);
    //             command.Parameters.AddWithValue("@IpAddress", ed.IpAddress);
    //             command.Parameters.AddWithValue("@NetworkName", ed.NetworkName);
    //             command.ExecuteNonQuery();
    //         }
    //
    //         transaction.Commit();
    //     }
    //     catch
    //     {
    //         transaction.Rollback();
    //         throw;
    //     }
    // }
    public void Add(T device)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            if (device is SmartWatch sw)
            {
                using var command = new SqlCommand("AddSmartwatch", connection, transaction)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@DeviceId", sw.Id);
                command.Parameters.AddWithValue("@Name", sw.Name);
                command.Parameters.AddWithValue("@IsEnabled", sw.IsEnabled);
                command.Parameters.AddWithValue("@BatteryPercentage", sw.BatteryLevel);
                command.ExecuteNonQuery();
            }
            else if (device is PersonalComputer pc)
            {
                using var command = new SqlCommand("AddPersonalComputer", connection, transaction)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@DeviceId", pc.Id);
                command.Parameters.AddWithValue("@Name", pc.Name);
                command.Parameters.AddWithValue("@IsEnabled", pc.IsEnabled);
                command.Parameters.AddWithValue("@OperationSystem", (object?)pc.OperatingSystem ?? DBNull.Value);
                command.ExecuteNonQuery();
            }
            else if (device is EmbeddedDevice ed)
            {
                using var command = new SqlCommand("AddEmbedded", connection, transaction)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@DeviceId", ed.Id);
                command.Parameters.AddWithValue("@Name", ed.Name);
                command.Parameters.AddWithValue("@IsEnabled", ed.IsEnabled);
                command.Parameters.AddWithValue("@IpAddress", ed.IpAddress);
                command.Parameters.AddWithValue("@NetworkName", ed.NetworkName);
                command.ExecuteNonQuery();
            }
            else
            {
                throw new InvalidOperationException("Unsupported device type.");
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public void Update(T device)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            using (var command = new SqlCommand("UpdateDevice", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", device.Id);
                command.Parameters.AddWithValue("@Name", device.Name);
                command.Parameters.AddWithValue("@IsEnabled", device.IsEnabled);
                
                var rowVersionParam = command.Parameters.Add("@OldRowVersion", SqlDbType.Timestamp);
                rowVersionParam.Value = device.RowVersion;
                
                command.ExecuteNonQuery();
            }

            if (device is SmartWatch sw)
            {
                using var command = new SqlCommand("UpdateSmartWatch", connection, transaction);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", sw.Id);
                command.Parameters.AddWithValue("@BatteryPercentage", sw.BatteryLevel);
                command.ExecuteNonQuery();
            }
            else if (device is PersonalComputer pc)
            {
                using var command = new SqlCommand("UpdatePersonalComputer", connection, transaction);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", pc.Id);
                command.Parameters.AddWithValue("@OperationSystem", (object?)pc.OperatingSystem ?? DBNull.Value);
                command.ExecuteNonQuery();
            }
            else if (device is EmbeddedDevice ed)
            {
                using var command = new SqlCommand("UpdateEmbeddedDevice", connection, transaction);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", ed.Id);
                command.Parameters.AddWithValue("@IpAddress", ed.IpAddress);
                command.Parameters.AddWithValue("@NetworkName", ed.NetworkName);
                command.ExecuteNonQuery();
            }

            transaction.Commit();
        }
        catch (SqlException ex) when (ex.Number == 50000)
        {
            transaction.Rollback();
            throw new DBConcurrencyException("The device was updated by someone else.", ex);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public bool Delete(string id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            string prefix = id.Split('-')[0].ToUpper();

            switch (prefix)
            {
                case "SW":
                    using (var cmd = new SqlCommand("DeleteSmartWatch", connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                    break;

                case "P":
                    using (var cmd = new SqlCommand("DeletePersonalComputer", connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                    break;

                case "E":
                    using (var cmd = new SqlCommand("DeleteEmbeddedDevice", connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                    break;

                default:
                    throw new ArgumentException("Unknown device prefix");
            }

            // Then delete from Device table
            using (var cmd = new SqlCommand("DeleteDevice", connection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }

            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
    
    public string GenerateDeviceId(string type)
    {
        string prefix = type.ToUpper() switch
        {
            "SW" => "SW-",
            "P" => "P-",
            "E" => "E-",
            _ => throw new ArgumentException("Invalid device type")
        };

        string query =
            $"SELECT MAX(CAST(SUBSTRING(Id, LEN('{prefix}') + 1, LEN(Id)) AS INT)) FROM Device WHERE Id LIKE '{prefix}%'";

        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        using var command = new SqlCommand(query, connection);
        var result = command.ExecuteScalar();

        int maxId = result != DBNull.Value ? Convert.ToInt32(result) : 0;

        return $"{prefix}{maxId + 1}";
    }
}