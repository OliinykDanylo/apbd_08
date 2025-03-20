namespace DevicesManager;

public class ConnectionException : Exception
{
    public ConnectionException(string message) : base(message) { }
}