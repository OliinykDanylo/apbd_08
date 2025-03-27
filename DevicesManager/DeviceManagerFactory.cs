namespace DevicesManager;

public class DeviceManagerFactory
{
    public static DeviceManager CreateDeviceManager()
    {
        IDeviceParser deviceParser = new DeviceParser();
        IDeviceFileHandler fileHandler = new DeviceFileHandler();
        return new DeviceManager(deviceParser, fileHandler);
    }
}