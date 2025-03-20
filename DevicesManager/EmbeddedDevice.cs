namespace DevicesManager;

public class EmbeddedDevice : Device
{
    private string _ipAddress;
    private string _networkName;

    public string IpAddress
    {
        get => _ipAddress;
        set
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(value,
                    @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$"))
            {
                throw new ArgumentException("Invalid IP address format.");
            }
            _ipAddress = value;
        }
    }

    public string NetworkName
    {
        get => _networkName;
        set => _networkName = value;
    }
    
    public EmbeddedDevice(int id, string name, bool isDeviceTurnedOn, string ipAddress, string networkName) : base(id, name, isDeviceTurnedOn)
    {
        IpAddress = ipAddress;
        NetworkName = networkName;
    }

    public void Connect()
    {
        if (!NetworkName.Contains("MD Ltd."))
        {
            throw new ConnectionException("Network Name does not contain MD Ltd.");
        }
    }

    public override void TurnOn()
    {
        Connect();
        base.TurnOn();
    } 
    
    public override string ToString()
    {
        return $"{Name} - {(IsDeviceTurnedOn ? "on" : "off")} - IP: {IpAddress} - Network: {NetworkName}";
    }
}