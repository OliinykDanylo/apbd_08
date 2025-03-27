namespace DevicesManager;

/// <summary>
/// Defines a method for parsing device data from a line of text.
/// </summary>
public interface IDeviceParser
{
    /// <summary>
    /// Parses a line of text and converts it into a <see cref="Device"/>.
    /// </summary>
    /// <param name="line">The line of text containing device data.</param>
    /// <param name="lineNumber">The line number of the data in the input file or source.</param>
    /// <returns>A <see cref="Device"/> object parsed from the provided line of text.</returns>
    /// <exception cref="ArgumentException">Thrown if the line is not in a valid format.</exception>
    Device Parse(string line, int lineNumber);
}