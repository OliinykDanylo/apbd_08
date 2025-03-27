/// <summary>
/// Represents an abstract device with basic properties and functionalities.
/// </summary>
public abstract class Device
{
    /// <summary>
    /// Gets or sets the unique identifier of the device.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the device.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the device is enabled.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Device"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the device.</param>
    /// <param name="name">The name of the device.</param>
    /// <param name="isEnabled">A value indicating whether the device is enabled.</param>
    public Device(string id, string name, bool isEnabled)
    {
        Id = id;
        Name = name;
        IsEnabled = isEnabled;
    }

    /// <summary>
    /// Turns on the device by setting <see cref="IsEnabled"/> to <c>true</c>.
    /// </summary>
    public virtual void TurnOn()
    {
        IsEnabled = true;
    }

    /// <summary>
    /// Turns off the device by setting <see cref="IsEnabled"/> to <c>false</c>.
    /// </summary>
    public virtual void TurnOff()
    {
        IsEnabled = false;
    }
}