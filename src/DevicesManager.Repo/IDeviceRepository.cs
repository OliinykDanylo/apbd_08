namespace DevicesManager.Logic;

public interface IDeviceRepository<T> where T : Device
{
    IEnumerable<T> GetAll();
    Device? GetById(string id);
    void Add(T device);
    void Update(T device);
    bool Delete(string id);
    string GenerateDeviceId(string type);
}