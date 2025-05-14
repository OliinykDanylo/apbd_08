using System.Text.Json.Serialization;

namespace DevicesManager;

using System.Text.RegularExpressions;
using DevicesManager;

public class EmbeddedDevice : Device
{
    private string _ipAddress;
    private bool _isConnected = false;

    public string NetworkName { get; set; }
    
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

    public EmbeddedDevice()
    {
    }

    public EmbeddedDevice(string id, string name, bool isEnabled, string ipAddress, string networkName) 
        : base(id, name, isEnabled)
    {
        if (!CheckId(id))
        {
            throw new ArgumentException("Invalid ID value. Required format: E-1", nameof(id));
        }

        IpAddress = ipAddress;
        NetworkName = networkName;
    }
    
    public override void TurnOn()
    {
        Connect();
        base.TurnOn();
    }

    public override void TurnOff()
    {
        _isConnected = false;
        base.TurnOff();
    }

    public override string ToString()
    {
        return $"Embedded Device: {Id}, Name: {Name}, IsEnabled: {IsEnabled}, IpAddress: {IpAddress}, NetworkName: {NetworkName}";
    }
    
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
    
    private bool CheckId(string id) => id.StartsWith("E-");
}