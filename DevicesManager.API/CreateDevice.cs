public class CreateDevice
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsEnabled { get; set; }
    public string Type { get; set; }
    
    public int? BatteryLevel { get; set; }         // Smartwatch
    public string? OperatingSystem { get; set; }   // PC
    public string? IpAddress { get; set; }         // Embedded
    public string? NetworkName { get; set; }
}