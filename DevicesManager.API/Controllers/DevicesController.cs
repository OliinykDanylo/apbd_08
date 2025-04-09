using Microsoft.AspNetCore.Mvc;

namespace DevicesManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly DeviceManager _deviceManager;

    public DevicesController(DeviceManager deviceManager)
    {
        _deviceManager = deviceManager;
    }

    [HttpGet]
    public IResult GetAllDevices()
    {
        var devices = _deviceManager.GetDevices()
            .Select(d => new DeviceAPI
            {
                Id = d.Id,
                Name = d.Name,
                IsEnabled = d.IsEnabled,
                Type = d.GetType().Name
            })
            .ToList();

        return Results.Ok(devices);
    }

    [HttpGet("{id}")]
    public IResult GetDeviceById(string id)
    {
        var device = _deviceManager.GetDeviceById(id);
        if (device is null)
            return Results.NotFound();

        var dto = new DeviceAPI
        {
            Id = device.Id,
            Name = device.Name,
            IsEnabled = device.IsEnabled,
            Type = device.GetType().Name
        };

        return Results.Ok(dto);
    }

    [HttpPost]
    public IResult CreateDevice(CreateDevice dto)
    {
        // You’ll need a factory or condition logic here to construct the correct type
        var id = Guid.NewGuid().ToString();
        Device newDevice;

        switch (dto.Type.ToLower())
        {
            case "smartwatch":
                newDevice = new Smartwatch(dto.Id, dto.Name, false, dto.BatteryLevel ?? 0); // Pass the required constructor arguments
                break;

            case "pc":
            case "personalcomputer":
                newDevice = new PersonalComputer(dto.Id, dto.Name, false, dto.OperatingSystem ?? "Unknown OS"); // Pass the required constructor arguments
                break;

            case "embeddeddevice":
                newDevice = new EmbeddedDevice(dto.Id, dto.Name, false, dto.IpAddress ?? "192.168.1.1", dto.NetworkName ?? "DefaultNet"); // Pass the required constructor arguments
                break;

            default:
                return Results.BadRequest($"Unsupported device type: {dto.Type}");
        }

        _deviceManager.AddDevice(newDevice);

        var result = new DeviceAPI
        {
            Id = newDevice.Id,
            Name = newDevice.Name,
            IsEnabled = newDevice.IsEnabled,
            Type = newDevice.GetType().Name
        };

        return Results.Created($"/api/devices/{newDevice.Id}", result);
    }

    [HttpPut("{id}")]
    public IResult UpdateDevice(string id, UpdateDevice dto)
    {
        var existing = _deviceManager.GetDeviceById(id);
        if (existing is null)
            return Results.NotFound();

        existing.Name = dto.Name;
        existing.IsEnabled = dto.IsEnabled;

        _deviceManager.EditDevice(existing);

        return Results.NoContent();
    }

    [HttpDelete("{id}")]
    public IResult DeleteDevice(string id)
    {
        var device = _deviceManager.GetDeviceById(id);
        if (device is null)
            return Results.NotFound();

        _deviceManager.RemoveDeviceById(id);

        return Results.NoContent();
    }

    [HttpPost("{id}/turn-on")]
    public IResult TurnOnDevice(string id)
    {
        var device = _deviceManager.GetDeviceById(id);
        if (device is null)
            return Results.NotFound();

        _deviceManager.TurnOnDevice(id);
        return Results.Ok();
    }

    [HttpPost("{id}/turn-off")]
    public IResult TurnOffDevice(string id)
    {
        var device = _deviceManager.GetDeviceById(id);
        if (device is null)
            return Results.NotFound();

        _deviceManager.TurnOffDevice(id);
        return Results.Ok();
    }
}