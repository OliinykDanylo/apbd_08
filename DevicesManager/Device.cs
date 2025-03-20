namespace DevicesManager;

public abstract class Device
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDeviceTurnedOn { get; set; }

    protected Device() {}

    public Device(int id, string name, bool isDeviceTurnedOn)
    {
        Id = id;
        Name = name;
        IsDeviceTurnedOn = isDeviceTurnedOn;
    }
    
    public virtual void TurnOn()
    {
        if(IsDeviceTurnedOn) throw new Exception("Device is already turned on");
        IsDeviceTurnedOn = true;
    }

    public void TurnOff()
    {
        if (!IsDeviceTurnedOn) throw new Exception("Device is already turned off");
        IsDeviceTurnedOn = false;
    }

    public abstract override string ToString();

}