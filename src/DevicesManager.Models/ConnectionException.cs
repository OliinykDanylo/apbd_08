namespace DevicesManager;

class ConnectionException : Exception
{
    public ConnectionException() : base("Wrong network name.") { }
}