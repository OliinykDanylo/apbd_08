namespace DevicesManager;

public interface IDeviceManager
{
    void AddDevice(Device newDevice);
    void EditDevice(Device editDevice);
    void RemoveDeviceById(string deviceId);
    void TurnOnDevice(string deviceId);
    void TurnOffDevice(string deviceId);
    Device? GetDeviceById(string deviceId);
    List<Device> GetDevices();
    void SaveDevices(string outputPath);
    void ClearAllDevices();
}