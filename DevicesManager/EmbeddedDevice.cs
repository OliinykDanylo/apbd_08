namespace DevicesManager;

public class EmbeddedDevice : Device
{ 
    public int Id { get; set; }
    public string Name { get; set; }
    public string ipAdress { get; set; }
    public bool isConnected { get; set; }
    public string NetworkName { get; set; }
}