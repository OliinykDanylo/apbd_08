using System.Text.RegularExpressions;
using DevicesManager;

namespace DevicesManager;

/// <summary>
/// Represents an embedded device with a network connection and an IP address.
/// Inherits from <see cref="Device"/>.
/// </summary>
public class EmbeddedDevice : Device
{
    private string _ipAddress;
    private bool _isConnected = false;

    /// <summary>
    /// Gets or sets the network name of the embedded device.
    /// </summary>
    public string NetworkName { get; set; }

    /// <summary>
    /// Gets or sets the IP address of the embedded device.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the IP address format is incorrect.</exception>
    public string IpAddress
    {
        get => _ipAddress;
        set
        {
            Regex ipRegex = new Regex("^((25[0-5]|(2[0-4]|1\\d|[1-9]|)\\d)\\.?\\b){4}$");
            if (ipRegex.IsMatch(value))
            {
                _ipAddress = value;
            }
            else
            {
                throw new ArgumentException("Wrong IP address format.");
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmbeddedDevice"/> class.
    /// </summary>
    /// <param name="id">The unique identifier for the device.</param>
    /// <param name="name">The name of the device.</param>
    /// <param name="isEnabled">The status of the device (enabled or disabled).</param>
    /// <param name="ipAddress">The IP address of the embedded device.</param>
    /// <param name="networkName">The name of the network the embedded device is connected to.</param>
    /// <exception cref="ArgumentException">Thrown if the ID does not match the required format (E-1).</exception>
    public EmbeddedDevice(string id, string name, bool isEnabled, string ipAddress, string networkName) 
        : base(id, name, isEnabled)
    {
        if (CheckId(id))
        {
            throw new ArgumentException("Invalid ID value. Required format: E-1", id);
        }

        IpAddress = ipAddress;
        NetworkName = networkName;
    }

    /// <summary>
    /// Turns on the embedded device and establishes a connection.
    /// </summary>
    public override void TurnOn()
    {
        Connect();
        base.TurnOn();
    }

    /// <summary>
    /// Turns off the embedded device and disconnects from the network.
    /// </summary>
    public override void TurnOff()
    {
        _isConnected = false;
        base.TurnOff();
    }

    /// <summary>
    /// Returns a string representation of the embedded device's status.
    /// </summary>
    /// <returns>A string describing the device's status, including its ID, name, and IP address.</returns>
    public override string ToString()
    {
        string enabledStatus = IsEnabled ? "enabled" : "disabled";
        return $"Embedded device {Name} ({Id}) is {enabledStatus} and has IP address {IpAddress}";
    }

    /// <summary>
    /// Attempts to connect the embedded device to the network.
    /// </summary>
    /// <exception cref="ConnectionException">Thrown if the device cannot connect to the specified network.</exception>
    private void Connect()
    {
        if (NetworkName.Contains("MD Ltd."))
        {
            _isConnected = true;
        }
        else
        {
            throw new ConnectionException();
        }
    }
    
    /// <summary>
    /// Validates the ID format for the embedded device.
    /// </summary>
    /// <param name="id">The device ID to check.</param>
    /// <returns>True if the ID format is valid; otherwise, false.</returns>
    private bool CheckId(string id) => id.Contains("E-");
}