using System.Data;
using System.Text.Json;
using System.Text.RegularExpressions;
using DevicesManager.Logic;
using Microsoft.AspNetCore.Mvc;

namespace DevicesManager.RestAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{

    private readonly IDeviceService<Device> _deviceManager;

    public DevicesController(IDeviceService<Device> deviceManager)
    {
        _deviceManager = deviceManager;
    }
    
    [HttpGet]
    public IResult GetAllDevices()
    {
        var devices = _deviceManager.GetAllDevices()
            .Select(d => new
            {
                d.Id,
                d.Name,
                d.IsEnabled,
                Type = d.GetType().Name
            });

        return Results.Ok(devices);
    }

    [HttpGet("{id}")]
    public IResult GetDeviceById(string id)
    {
        var device = _deviceManager.GetDeviceById(id);
        return device is null
            ? Results.NotFound($"Device with ID '{id}' not found.")
            : Results.Ok(device);
    }
    
    [HttpPost("{type}")]
    public IResult PostDevice(string type, [FromBody] JsonElement body)
    {
        try
        {
            if (body.ValueKind != JsonValueKind.String)
            {
                return Results.BadRequest("Request body must be a JSON string.");
            }

            string textBody = body.GetString() ?? throw new ArgumentException("Body is empty.");
            var lines = textBody.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            Device device;

            switch (type.ToLower())
            {
                case "sw":
                    if (lines.Length < 3) throw new ArgumentException("Invalid SmartWatch input format.");
                    device = new SmartWatch
                    {
                        Name = lines[0],
                        IsEnabled = bool.Parse(lines[1]),
                        BatteryLevel = int.Parse(lines[2])
                    };
                    break;

                case "p":
                    if (lines.Length < 3) throw new ArgumentException("Invalid PersonalComputer input format.");
                    device = new PersonalComputer
                    {
                        Name = lines[0],
                        IsEnabled = bool.Parse(lines[1]),
                        OperatingSystem = lines[2]
                    };
                    break;

                case "e":
                    if (lines.Length < 4) throw new ArgumentException("Invalid EmbeddedDevice input format.");
                    device = new EmbeddedDevice
                    {
                        Name = lines[0],
                        IsEnabled = bool.Parse(lines[1]),
                        IpAddress = lines[2],
                        NetworkName = lines[3]
                    };
                    break;

                default:
                    return Results.BadRequest($"Unsupported device type: {type}");
            }
            
            device.Id = _deviceManager.GenerateDeviceId(type);

            ValidateDevice(device);

            _deviceManager.Post(device);

            return Results.Created($"/api/devices/{device.Id}", device);
        }
        catch (FormatException ex)
        {
            return Results.BadRequest($"Invalid format: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private void ValidateDevice(Device device)
    {
        if (device == null)
        {
            throw new ArgumentException("Device cannot be null.");
        }

        switch (device)
        {
            case SmartWatch smartwatch:
                if (smartwatch.BatteryLevel < 0 || smartwatch.BatteryLevel > 100)
                {
                    throw new ArgumentException("Battery percentage must be between 0 and 100.");
                }
                break;

            case PersonalComputer pc:
                if (!pc.CanBeTurnedOn())
                {
                    throw new ArgumentException("PC cannot be turned on.");
                }
                break;

            case EmbeddedDevice embedded:
                if (string.IsNullOrWhiteSpace(embedded.IpAddress) || !IsValidIpAddress(embedded.IpAddress))
                {
                    throw new ArgumentException("Invalid IP address format.");
                }
                if (string.IsNullOrWhiteSpace(embedded.NetworkName))
                {
                    throw new ArgumentException("Network name cannot be empty.");
                }
                break;

            default:
                throw new ArgumentException("Unsupported device type.");
        }
    }

    private bool IsValidIpAddress(string ipAddress)
    {
        Regex ipRegex = new Regex("^((25[0-5]|(2[0-4]|1\\d|[1-9]|)\\d)\\.?\\b){4}$");
        return ipRegex.IsMatch(ipAddress);
    }

    [HttpPut("{id}")]
    public IResult UpdateDevice(string id, [FromBody] JsonElement body)
    {
        try
        {
            if (body.ValueKind != JsonValueKind.String)
            {
                return Results.BadRequest("Request body must be a newline-delimited string.");
            }

            string textBody = body.GetString() ?? throw new ArgumentException("Body is empty.");
            var lines = textBody.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (lines.Length < 5)
            {
                return Results.BadRequest("Insufficient fields in body (expected at least 5).");
            }

            string inputId = lines[0];
            if (inputId != id)
            {
                return Results.BadRequest("Device Id in URL does not match Id in body.");
            }

            byte[] rowVersion;
            try
            {
                string hexRowVersion = lines[^1];
                if (hexRowVersion.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    hexRowVersion = hexRowVersion.Substring(2);
                }
                
                rowVersion = ConvertHexToByteArray(hexRowVersion);
            }
            catch (FormatException)
            {
                return Results.BadRequest("Invalid RowVersion format. Expected hexadecimal (0x prefix) or Base64.");
            }

            Device device;

            if (inputId.StartsWith("SW-"))
            {
                if (lines.Length < 5)
                    return Results.BadRequest("Missing SmartWatch fields.");

                device = new SmartWatch
                {
                    Id = inputId,
                    Name = lines[1],
                    IsEnabled = bool.Parse(lines[2]),
                    BatteryLevel = int.Parse(lines[3]),
                    RowVersion = rowVersion
                };
            }
            else if (inputId.StartsWith("P-"))
            {
                if (lines.Length < 5)
                    return Results.BadRequest("Missing PersonalComputer fields.");

                device = new PersonalComputer
                {
                    Id = inputId,
                    Name = lines[1],
                    IsEnabled = bool.Parse(lines[2]),
                    OperatingSystem = lines[3],
                    RowVersion = rowVersion
                };
            }
            else if (inputId.StartsWith("E-"))
            {
                if (lines.Length < 6)
                    return Results.BadRequest("Missing EmbeddedDevice fields.");

                device = new EmbeddedDevice
                {
                    Id = inputId,
                    Name = lines[1],
                    IsEnabled = bool.Parse(lines[2]),
                    IpAddress = lines[3],
                    NetworkName = lines[4],
                    RowVersion = rowVersion
                };
            }
            else
            {
                return Results.BadRequest("Unknown device type.");
            }

            _deviceManager.EditDevice(device);

            return Results.Ok(device);
        }
        catch (FormatException ex)
        {
            return Results.BadRequest($"Invalid format: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (DBConcurrencyException)
        {
            return Results.Conflict("The device was updated by another user. Please reload and try again.");
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
    
    private byte[] ConvertHexToByteArray(string hex)
    {
        if (hex.Length % 2 != 0)
            throw new ArgumentException("Hex string must have an even length.");

        byte[] byteArray = new byte[hex.Length / 2];
        for (int i = 0; i < hex.Length; i += 2)
        {
            byteArray[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return byteArray;
    }

    [HttpDelete("{id}")]
    public IResult DeleteDevice(string id)
    {
        bool success = _deviceManager.DeleteDevice(id);
        return success
            ? Results.NoContent()
            : Results.NotFound($"Device with ID '{id}' not found.");
    }
}