namespace DevicesManager;

public class Smartwatch : Device, IPowerNotifier
{
    private int _batteryPercentage;
    
    public int BatteryPercentage
    {
        get => _batteryPercentage;
        set
        {
            if (value < 0 || value > 100) 
                throw new ArgumentOutOfRangeException("Battery percentage must be between 0 and 100");
            
            _batteryPercentage = value;

            if (_batteryPercentage < 20)
            {
                NotifyLowBattery();
            }
        }
    }
    
    public Smartwatch(int id, string name, bool isDeviceTurnedOn, int batteryPercentage) 
        : base(id, name, isDeviceTurnedOn)
    {
        BatteryPercentage = batteryPercentage;
    }

    public void NotifyLowBattery()
    {
        Console.WriteLine("Battery is low. Please charge your smartwatch.");
    }

    public override void TurnOn()
    {
        if (BatteryPercentage < 11)
            throw new EmptyBatteryException("Battery is too low to turn on the smartwatch");
        
        base.TurnOn();
        BatteryPercentage -= 10;
    }

    public override string ToString()
    {
        return $"{Name} - {(IsDeviceTurnedOn ? "on" : "off")} - Battery: {BatteryPercentage}%";
    }

    public string getName()
    {
        return Name;
    }
}