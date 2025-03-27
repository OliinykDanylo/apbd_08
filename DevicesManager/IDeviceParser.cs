namespace DevicesManager;

public interface IDeviceParser
{
    Device Parse(string line, int lineNumber);
}