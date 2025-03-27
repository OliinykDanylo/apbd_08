namespace DevicesManager;

/// <summary>
/// Defines a method for notifying power-related events.
/// </summary>
public interface IPowerNotifier
{
    /// <summary>
    /// Sends a notification about a power-related event (e.g., low battery, power on/off).
    /// </summary>
    void Notify();
}