namespace DevicesManager;

public class SmartWatch : Device
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool isEnabled { get; set; }
    public int batteryLevel { get; set; }
}