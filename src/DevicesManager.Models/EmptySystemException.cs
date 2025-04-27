namespace DevicesManager;

class EmptySystemException : Exception
{
    public EmptySystemException() : base("Operation system is not installed.") { }
}