namespace DevicesManager;

public class PersonalComputer : Device
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool isEnabled { get; set; }
    public string OperatingSystem { get; set; }
}