namespace DevicesManager;

public interface IDeviceFileHandler
{
    public string[] ReadAllLines(string path);
    public void WriteAllLines(string path, string[] lines);
}