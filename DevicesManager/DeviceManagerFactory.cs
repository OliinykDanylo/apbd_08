namespace DevicesManager;

/// <summary>
/// Factory class for creating instances of <see cref="DeviceManager"/>.
/// </summary>
public class DeviceManagerFactory
{
    /// <summary>
    /// Creates and returns a new instance of <see cref="DeviceManager"/>.
    /// </summary>
    /// <returns>A new instance of <see cref="DeviceManager"/> with default dependencies.</returns>
    public static DeviceManager CreateDeviceManager()
    {
        IDeviceParser deviceParser = new DeviceParser();
        IDeviceFileHandler fileHandler = new DeviceFileHandler();
        return new DeviceManager(deviceParser, fileHandler);
    }
}