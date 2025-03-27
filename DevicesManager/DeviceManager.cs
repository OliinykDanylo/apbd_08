namespace DevicesManager;

public class DeviceManager : IDeviceManager
{
    private readonly IDeviceParser _deviceParser;
    private readonly IDeviceFileHandler _fileHandler;
    private const int MaxCapacity = 15;
    private List<Device> _devices = new(capacity: MaxCapacity);

    public DeviceManager(IDeviceParser deviceParser, IDeviceFileHandler fileHandler)
    {
        _deviceParser = deviceParser;
        _fileHandler = fileHandler;
    }

    public void AddDevice(Device newDevice)
    {
        if (_devices.Count >= MaxCapacity)
        {
            throw new Exception("Device storage is full.");
        }
        _devices.Add(newDevice);
    }

    public void EditDevice(Device editDevice)
    {
        var device = GetDeviceById(editDevice.Id);
        if (device != null)
        {
            _devices.Remove(device);
            _devices.Add(editDevice);
        }
    }

    public void RemoveDeviceById(string deviceId)
    {
        var device = GetDeviceById(deviceId);
        if (device != null)
        {
            _devices.Remove(device);
        }
    }

    public void TurnOnDevice(string deviceId)
    {
        var device = GetDeviceById(deviceId);
        if (device != null)
        {
            device.IsEnabled = true;
        }
    }

    public void TurnOffDevice(string deviceId)
    {
        var device = GetDeviceById(deviceId);
        if (device != null)
        {
            device.IsEnabled = false;
        }
    }

    public Device? GetDeviceById(string deviceId)
    {
        return _devices.FirstOrDefault(d => d.Id == deviceId);
    }

    public List<Device> GetDevices()
    {
        return _devices;
    }

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

    public void ClearAllDevices()
    {
        _devices.Clear();
    }

    public int GetMaxCapacity()
    {
        return MaxCapacity;
    }
}