namespace DevicesManager.RestAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

[Route("api/smdevice")]
[ApiController]
public class SmartWatchController : ControllerBase
{
    private static List<SmartWatch> _devices = new List<SmartWatch>
    {
        new SmartWatch { Id = 1, Name = "Apple Watch", isEnabled = true, batteryLevel = 90 },
        new SmartWatch { Id = 2, Name = "Galaxy Watch", isEnabled = false, batteryLevel = 78 }
    };

    private readonly DeviceManager<SmartWatch> _deviceManager = new DeviceManager<SmartWatch>(_devices);

    [HttpPost]
    public IActionResult Post([FromBody] SmartWatch device)
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
    public IActionResult EditDevice(int id, [FromBody] SmartWatch updatedDevice)
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