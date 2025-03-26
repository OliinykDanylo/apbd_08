namespace DevicesManager;

class ConnectionException : Exception
{
    public ConnectionException() : base("Wrong netowrk name.") { }
}