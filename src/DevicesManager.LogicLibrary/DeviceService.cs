using System.Data;
using System.Text.Json;

namespace DevicesManager.Logic;

public class DeviceService<T> : IDeviceService<T> where T : Device, new()
{
    private readonly IDeviceRepository<T> _repository;

    public DeviceService(IDeviceRepository<T> repository)
    {
        _repository = repository;
    }

    public List<T> GetAllDevices() => _repository.GetAll().ToList();

    public Device GetDeviceById(string id) => _repository.GetById(id);

    public void Post(Device device)
    {
        _repository.Add((T)device);
        // Optional: handle type-specific logic here if needed
    }

    public bool EditDevice(Device device)
    {
        _repository.Update((T)device);
        return true;
    }

    public bool DeleteDevice(string id)
    {
        return _repository.Delete(id);
    }


    public string GenerateDeviceId(string type)
    {
        return _repository.GenerateDeviceId(type);
    }
}
    
