namespace DevicesManager.Logic;

public interface IDeviceService<T> where T : Device
{
    void Post(Device device);
    List<T> GetAllDevices();
    T GetDeviceById(string id);
    bool EditDevice(Device device);
    bool DeleteDevice(string id, string deviceType);
}