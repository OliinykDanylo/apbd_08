namespace DevicesManager;

public class DeviceManager<T> where T : Device
{
    private readonly List<T> _devices;

    public DeviceManager(List<T> devices)
    {
        _devices = devices;
    }

    public void Post(T device)
    {
        _devices.Add(device);
    }

    public List<T> GetAllDevices()
    {
        return _devices;
    }

    public T GetDeviceById(int id)
    {
        return _devices.FirstOrDefault(d => d.Id == id);
    }

    public bool EditDevice(int id, T updatedDevice)
    {
        var device = _devices.FirstOrDefault(d => d.Id == id);
        if (device == null)
        {
            return false;
        }

        // to update properties dynamically
        device.Name = updatedDevice.Name;
        foreach (var property in typeof(T).GetProperties())
        {
            if (property.CanWrite && property.Name != nameof(Device.Id) && property.Name != nameof(Device.Name))
            {
                property.SetValue(device, property.GetValue(updatedDevice));
            }
        }

        return true;
    }

    public bool DeleteDevice(int id)
    {
        var device = _devices.FirstOrDefault(d => d.Id == id);
        if (device == null)
        {
            return false;
        }

        _devices.Remove(device);
        return true;
    }
}