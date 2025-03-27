namespace DevicesManager;

/// <summary>
/// Manages a collection of devices, allowing operations such as adding, editing, removing, and saving devices.
/// </summary>
public class DeviceManager : IDeviceManager
{
    private readonly IDeviceParser _deviceParser;
    private readonly IDeviceFileHandler _fileHandler;
    private const int MaxCapacity = 15;
    private List<Device> _devices = new(capacity: MaxCapacity);

    /// <summary>
    /// Initializes a new instance of the <see cref="DeviceManager"/> class.
    /// </summary>
    /// <param name="deviceParser">The device parser for handling device data.</param>
    /// <param name="fileHandler">The file handler for reading and writing device data.</param>
    public DeviceManager(IDeviceParser deviceParser, IDeviceFileHandler fileHandler)
    {
        _deviceParser = deviceParser;
        _fileHandler = fileHandler;
    }

    /// <summary>
    /// Adds a new device to the collection.
    /// </summary>
    /// <param name="newDevice">The device to add.</param>
    /// <exception cref="Exception">Thrown when the device storage reaches maximum capacity.</exception>
    public void AddDevice(Device newDevice)
    {
        if (_devices.Count >= MaxCapacity)
        {
            throw new Exception("Device storage is full.");
        }
        _devices.Add(newDevice);
    }

    /// <summary>
    /// Edits an existing device by replacing it with an updated version.
    /// </summary>
    /// <param name="editDevice">The updated device.</param>
    public void EditDevice(Device editDevice)
    {
        var device = GetDeviceById(editDevice.Id);
        if (device != null)
        {
            _devices.Remove(device);
            _devices.Add(editDevice);
        }
    }

    /// <summary>
    /// Removes a device from the collection by its ID.
    /// </summary>
    /// <param name="deviceId">The ID of the device to remove.</param>
    public void RemoveDeviceById(string deviceId)
    {
        var device = GetDeviceById(deviceId);
        if (device != null)
        {
            _devices.Remove(device);
        }
    }

    /// <summary>
    /// Turns on a device by its ID.
    /// </summary>
    /// <param name="deviceId">The ID of the device to turn on.</param>
    public void TurnOnDevice(string deviceId)
    {
        var device = GetDeviceById(deviceId);
        if (device != null)
        {
            device.IsEnabled = true;
        }
    }

    /// <summary>
    /// Turns off a device by its ID.
    /// </summary>
    /// <param name="deviceId">The ID of the device to turn off.</param>
    public void TurnOffDevice(string deviceId)
    {
        var device = GetDeviceById(deviceId);
        if (device != null)
        {
            device.IsEnabled = false;
        }
    }

    /// <summary>
    /// Retrieves a device by its ID.
    /// </summary>
    /// <param name="deviceId">The ID of the device.</param>
    /// <returns>The device if found; otherwise, null.</returns>
    public Device? GetDeviceById(string deviceId)
    {
        return _devices.FirstOrDefault(d => d.Id == deviceId);
    }

    /// <summary>
    /// Retrieves all stored devices.
    /// </summary>
    /// <returns>A list of all devices.</returns>
    public List<Device> GetDevices()
    {
        return _devices;
    }

    /// <summary>
    /// Saves all devices to a specified file.
    /// </summary>
    /// <param name="outputPath">The path where the devices should be saved.</param>
    public void SaveDevices(string outputPath)
    {
        var lines = _devices.Select(d =>
        {
            switch (d)
            {
                case Smartwatch smartwatch:
                    return $"{smartwatch.Id},{smartwatch.Name},{smartwatch.IsEnabled},{smartwatch.getBatteryLevel()}";
                case PersonalComputer pc:
                    return $"{pc.Id},{pc.Name},{pc.IsEnabled},{pc.OperatingSystem}";
                case EmbeddedDevice embeddedDevice:
                    return $"{d.Id},{d.Name},{d.IsEnabled},{embeddedDevice.IpAddress},{embeddedDevice.NetworkName}";
                default:
                    return $"{d.Id},{d.Name},{d.IsEnabled}";
            }
        }).ToArray();
        _fileHandler.WriteAllLines(outputPath, lines);
    }

    /// <summary>
    /// Clears all devices from the collection.
    /// </summary>
    public void ClearAllDevices()
    {
        _devices.Clear();
    }

    /// <summary>
    /// Gets the maximum capacity of devices that can be stored.
    /// </summary>
    /// <returns>The maximum number of devices.</returns>
    public int GetMaxCapacity()
    {
        return MaxCapacity;
    }
}