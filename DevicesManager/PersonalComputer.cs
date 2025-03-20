namespace DevicesManager;

public class PersonalComputer : Device
{
    public string OperatingSystem { get; set; }

    public PersonalComputer(int id, string name, bool isDeviceTurnedOn, string operatingSystem) 
        : base(id, name, isDeviceTurnedOn)
    {
        OperatingSystem = operatingSystem;
    }

    public override void TurnOn()
    {
        if(string.IsNullOrEmpty(OperatingSystem))
            throw new EmptySystemException("Operating system is not set");
        
        base.TurnOn();
    }
    
    public override string ToString()
    {
        return $"{Name} - {(IsDeviceTurnedOn ? "on" : "off")} - OS: {OperatingSystem}";
    }
}