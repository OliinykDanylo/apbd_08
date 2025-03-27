namespace DevicesManager;

/// <summary>
/// Exception thrown when the OS is not installed.
/// </summary>
class EmptySystemException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmptySystemException"/> class
    /// with a default error message.
    /// </summary>
    public EmptySystemException() : base("Operation system is not installed.") { }
}