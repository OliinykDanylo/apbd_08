namespace DevicesManager.Logic;

public interface IDeviceService<T> where T : Device
{
    void Post(Device device);
    List<T> GetAllDevices();
    Device GetDeviceById(string id);
    bool EditDevice(Device device);
    bool DeleteDevice(string id);
    string GenerateDeviceId(string type);
}