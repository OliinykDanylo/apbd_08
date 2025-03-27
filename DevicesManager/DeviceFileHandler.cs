namespace DevicesManager;

public class DeviceFileHandler : IDeviceFileHandler
{
    public string[] ReadAllLines(string filePath)
    {
        return File.ReadAllLines(filePath);
    }

    public void WriteAllLines(string filePath, string[] lines)
    {
        File.WriteAllLines(filePath, lines);
    }
}