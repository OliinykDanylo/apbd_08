namespace DevicesManager;

public class DeviceManager
{
    private const int MaxDevices = 15;
    private readonly List<Device> _devices = new List<Device>();
    private readonly string _filePath;

    public List<Device> GetDevices()
    {
        return _devices;
    }

    public DeviceManager(string filePath)
    {
        _filePath = filePath;
        LoadDevicesFromFile();
    }

    private void LoadDevicesFromFile()
    {
        var lines = File.ReadAllLines(_filePath);
        foreach (var line in lines)
        {
            try
            {
                var parts = line.Split(',');
                var deviceType = parts[0].Substring(0, parts[0].StartsWith("P") ? 1 : 2); // to use 1 character for PersonalComputer, 2 for others
                switch (deviceType)
                {
                    case "SW":
                        _devices.Add(new Smartwatch(
                            int.Parse(parts[0].Substring(3)),
                            parts[1],
                            bool.Parse(parts[2]),
                            int.Parse(parts[3].TrimEnd('%'))
                        ));
                        break;
                    case "P":
                        _devices.Add(new PersonalComputer(
                            int.Parse(parts[0].Substring(2)),
                            parts[1],
                            bool.Parse(parts[2]),
                            parts[3]
                        ));
                        break;
                    case "ED":
                        _devices.Add(new EmbeddedDevice(
                            int.Parse(parts[0].Substring(3)),
                            parts[1],
                            false, // devices are off by default
                            parts[2],
                            parts[3]
                        ));
                        break;
                    default:
                        throw new FormatException($"Unknown device type: {deviceType}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing line: {line}. Exception: {ex.Message}");
            }
        }
    }
    
    public void AddDevice(Device device)
    {
        if (_devices.Count >= MaxDevices)
        {
            Console.WriteLine("Device storage is full.");
            return;
        }
        _devices.Add(device);
    }

    public void RemoveDevice(int id)
    {
        var device = _devices.FirstOrDefault(d => d.Id == id);
        if (device != null)
        {
            _devices.Remove(device);
        }
    }

    public void EditDevice(string name, object newData)
    {
        var device = _devices.FirstOrDefault(d => d.Name == name);
        if (device != null)
        {
            switch (device)
            {
                case Smartwatch smartwatch:
                    if (newData is int newBatteryPercentage)
                    {
                        smartwatch.BatteryPercentage = newBatteryPercentage;
                    }
                    break;
                case PersonalComputer pc:
                    if (newData is string newOperatingSystem)
                    {
                        pc.OperatingSystem = newOperatingSystem;
                    }
                    break;
                case EmbeddedDevice ed:
                    if (newData is (string newIpAddress, string newNetworkName))
                    {
                        ed.IpAddress = newIpAddress;
                        ed.NetworkName = newNetworkName;
                    }
                    break;
                default:
                    Console.WriteLine("Unsupported device type.");
                    break;
            }
        }
    }

    // public void TurnOnDevice(int id)
    // {
    //     var device = _devices.FirstOrDefault(d => d.Id == id);
    //     device?.TurnOn();
    // }

    public void TurnOnDevice(string name)
    {
        var device = _devices.FirstOrDefault(d => d.Name == name);
        device?.TurnOn();
    }

    // public void TurnOffDevice(int id)
    // {
    //     var device = _devices.FirstOrDefault(d => d.Id == id);
    //     device?.TurnOff();
    // }
    
    public void TurnOffDevice(string name)
    {
        var device = _devices.FirstOrDefault(d => d.Name == name);
        device?.TurnOff();
    }

    public void ShowAllDevices()
    {
        Console.WriteLine("All devices:");
        foreach (var device in _devices)
        {
            Console.WriteLine(device);
        }
    }
    
    private string FormatDeviceData(Device device)
    {
        switch (device)
        {
            case Smartwatch smartwatch:
                return $"SW-{smartwatch.Id},{smartwatch.Name},{smartwatch.IsDeviceTurnedOn},{smartwatch.BatteryPercentage}%";
            case PersonalComputer pc:
                return $"P-{pc.Id},{pc.Name},{pc.IsDeviceTurnedOn},{pc.OperatingSystem}";
            case EmbeddedDevice ed:
                return $"ED-{ed.Id},{ed.Name},{ed.IpAddress},{ed.NetworkName}";
            default:
                throw new InvalidOperationException("Unsupported device type.");
        }
    }
    
    public void SaveDataToFile(string filePath)
    {
        try
        {
            var lines = _devices.Select(FormatDeviceData).ToArray();
            File.WriteAllLines(filePath, lines);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving data to file: {ex.Message}");
        }
    }

    // public void SaveDataToFile(string filePath)
    // {
    //     try
    //     {
    //         var lines = _devices.Select(d => d.ToString()).ToArray();
    //         File.WriteAllLines(filePath, lines);
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"Error saving data to file: {ex.Message}");
    //     }
    // }

    // For Debugging
    public void getDevicesIds()
    {
        Console.WriteLine("Devices ids:");
        foreach (var device in _devices)
        {
            Console.WriteLine(device.Name + " - with ID: " + device.Id);
        }
    }
}