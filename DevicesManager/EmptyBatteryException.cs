namespace DevicesManager;

/// <summary>
/// Exception thrown when the battery level is too low to power on the device.
/// </summary>
public class EmptyBatteryException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmptyBatteryException"/> class
    /// with a default error message.
    /// </summary>
    public EmptyBatteryException() 
        : base("Battery level is too low to turn it on.") 
    { }
}