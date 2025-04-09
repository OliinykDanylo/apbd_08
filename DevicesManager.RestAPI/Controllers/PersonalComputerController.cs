namespace DevicesManager.RestAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

[Route("api/pc")]
[ApiController]
public class PersonalComputerController : ControllerBase
{
    private static List<PersonalComputer> _devices = new List<PersonalComputer>
    {
        new PersonalComputer { Id = 1, Name = "Dell XPS", isEnabled = true, OperatingSystem = "Windows 11" },
        new PersonalComputer { Id = 2, Name = "MacBook Pro", isEnabled = true, OperatingSystem = "macOS Ventura" }
    };

    private readonly DeviceManager<PersonalComputer> _deviceManager = new DeviceManager<PersonalComputer>(_devices);

    [HttpPost]
    public IActionResult Post([FromBody] PersonalComputer device)
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
    public IActionResult EditDevice(int id, [FromBody] PersonalComputer updatedDevice)
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