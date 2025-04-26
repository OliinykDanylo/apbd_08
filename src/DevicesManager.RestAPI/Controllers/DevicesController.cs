using System.Text.Json;
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
    public IResult PostDevice([FromBody] JsonElement body)
    {
        try
        {
            string jsonString = body.GetRawText();

            using var document = JsonDocument.Parse(jsonString);
            var root = document.RootElement;

            string id = root.GetProperty("Id").GetString() ?? throw new ArgumentException("Device Id is required");

            Device device;
            if (id.StartsWith("SW-"))
            {
                device = JsonSerializer.Deserialize<SmartWatch>(jsonString)!;
            }
            else if (id.StartsWith("P-"))
            {
                device = JsonSerializer.Deserialize<PersonalComputer>(jsonString)!;
            }
            else if (id.StartsWith("E-"))
            {
                device = JsonSerializer.Deserialize<EmbeddedDevice>(jsonString)!;
            }
            else
            {
                throw new ArgumentException("Unknown device type.");
            }

            _deviceManager.Post(device);

            return Results.Created($"/api/devices/{device.Id}", device);
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

    [HttpPut("{id}")]
    public IResult UpdateDevice(string id, [FromBody] JsonElement body)
    {
        try
        {
            string jsonString = body.GetRawText();

            using var document = JsonDocument.Parse(jsonString);
            var root = document.RootElement;

            string deviceId = root.GetProperty("Id").GetString() ?? throw new ArgumentException("Device Id is required");

            if (deviceId != id)
            {
                return Results.BadRequest("Device Id in URL does not match Id in body.");
            }

            Device device;
            if (deviceId.StartsWith("SW-"))
            {
                device = JsonSerializer.Deserialize<SmartWatch>(jsonString)!;
            }
            else if (deviceId.StartsWith("P-"))
            {
                device = JsonSerializer.Deserialize<PersonalComputer>(jsonString)!;
            }
            else if (deviceId.StartsWith("E-"))
            {
                device = JsonSerializer.Deserialize<EmbeddedDevice>(jsonString)!;
            }
            else
            {
                throw new ArgumentException("Unknown device type.");
            }

            _deviceManager.EditDevice(device);

            return Results.Ok(device);
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

    [HttpDelete("{id}")]
    public IResult DeleteDevice(string id, string deviceType)
    {
        bool success = _deviceManager.DeleteDevice(id, deviceType);
        return success
            ? Results.NoContent()
            : Results.NotFound($"Device with ID '{id}' not found.");
    }
}