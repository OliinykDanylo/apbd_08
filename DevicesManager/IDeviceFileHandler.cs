namespace DevicesManager;

/// <summary>
/// Defines methods for reading and writing device data from/to files.
/// </summary>
public interface IDeviceFileHandler
{
    /// <summary>
    /// Reads all lines from a file at the specified path.
    /// </summary>
    /// <param name="path">The file path to read from.</param>
    /// <returns>An array of strings representing the lines read from the file.</returns>
    string[] ReadAllLines(string path);

    /// <summary>
    /// Writes the specified lines to a file at the given path.
    /// </summary>
    /// <param name="path">The file path to write to.</param>
    /// <param name="lines">An array of strings to write to the file.</param>
    void WriteAllLines(string path, string[] lines);
}