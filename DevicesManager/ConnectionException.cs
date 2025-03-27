namespace DevicesManager;

/// <summary>
/// Exception thrown when the network name is wrong.
/// </summary>
class ConnectionException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionException"/> class
    /// with a default error message.
    /// </summary>
    public ConnectionException() : base("Wrong netowrk name.") { }
}