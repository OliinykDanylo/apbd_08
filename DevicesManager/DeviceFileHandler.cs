namespace DevicesManager;

/// <summary>
/// Handles file operations related to devices, such as reading and writing lines to a file.
/// </summary>
public class DeviceFileHandler : IDeviceFileHandler
{
    /// <summary>
    /// Reads all lines from the specified file.
    /// </summary>
    /// <param name="filePath">The path to the file to be read.</param>
    /// <returns>An array of strings, where each element represents a line from the file.</returns>
    /// <exception cref="IOException">Thrown if an I/O error occurs while reading the file.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if access to the file is denied.</exception>
    /// <exception cref="FileNotFoundException">Thrown if the specified file does not exist.</exception>
    public string[] ReadAllLines(string filePath)
    {
        return File.ReadAllLines(filePath);
    }

    /// <summary>
    /// Writes all lines to the specified file, overwriting the existing content.
    /// </summary>
    /// <param name="filePath">The path to the file to be written.</param>
    /// <param name="lines">An array of strings to write to the file.</param>
    /// <exception cref="IOException">Thrown if an I/O error occurs while writing to the file.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if access to the file is denied.</exception>
    /// <exception cref="FileNotFoundException">Thrown if the specified file does not exist.</exception>
    public void WriteAllLines(string filePath, string[] lines)
    {
        File.WriteAllLines(filePath, lines);
    }
}