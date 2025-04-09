namespace DevicesManager.RestAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

[Route("api/embdevice")]
[ApiController]
public class EmbeddedController : ControllerBase
{
    private static List<EmbeddedDevice> _devices = new List<EmbeddedDevice>
    {
        new EmbeddedDevice
        {
            Id = 1,
            Name = "EmbeddedDevice1",
            ipAdress = "192.198.1.6",
            isConnected = true,
            NetworkName = "Network1"
        },
        new EmbeddedDevice
        {
            Id = 2,
            Name = "Device2",
            ipAdress = "192.198.2.1",
            isConnected = false,
            NetworkName = "Network2"
        }
    };

    private readonly DeviceManager<EmbeddedDevice> _deviceManager = new DeviceManager<EmbeddedDevice>(_devices);

    [HttpPost]
    public IActionResult Post([FromBody] EmbeddedDevice device)
    {
        _deviceManager.Post(device);
        return CreatedAtAction("Post", device);
    }

    [HttpGet]
    public IActionResult GetAllDevices()
    {
        return Ok(_deviceManager.GetAllDevices());
    }

    [HttpGet("{id}")]
    public IActionResult GetDeviceById(int id)
    {
        var device = _deviceManager.GetDeviceById(id);
        if (device == null)
        {
            return NotFound();
        }
        return Ok(device);
    }

    [HttpPut("{id}")]
    public IActionResult EditDevice(int id, [FromBody] EmbeddedDevice updatedDevice)
    {
        if (!_deviceManager.EditDevice(id, updatedDevice))
        {
            return NotFound($"Device {id} not found");
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteDevice(int id)
    {
        if (!_deviceManager.DeleteDevice(id))
        {
            return NotFound($"Device {id} not found");
        }
        return NoContent();
    }
}