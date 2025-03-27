using System;

namespace DevicesManager;

/// <summary>
/// Parses device data from a string format into corresponding <see cref="Device"/> objects.
/// </summary>
public class DeviceParser : IDeviceParser
{
    /// <summary>
    /// Parses a line of device data and converts it into a specific <see cref="Device"/> instance.
    /// </summary>
    /// <param name="line">The line of data representing a device.</param>
    /// <param name="lineNumber">The line number in the input file (used for error reporting).</param>
    /// <returns>A <see cref="Device"/> object parsed from the input line.</returns>
    /// <exception cref="ArgumentException">Thrown if the line is corrupted or cannot be parsed.</exception>
    public Device Parse(string line, int lineNumber)
    {
        if (line.StartsWith("P-"))
        {
            return ParsePC(line, lineNumber);
        }
        else if (line.StartsWith("SW-"))
        {
            return ParseSmartwatch(line, lineNumber);
        }
        else if (line.StartsWith("ED-"))
        {
            return ParseEmbedded(line, lineNumber);
        }
        else
        {
            throw new ArgumentException($"Line {lineNumber} is corrupted.");
        }
    }

    /// <summary>
    /// Parses a line representing a <see cref="PersonalComputer"/>.
    /// </summary>
    /// <param name="line">The line of data representing a personal computer.</param>
    /// <param name="lineNumber">The line number in the input file.</param>
    /// <returns>A <see cref="PersonalComputer"/> object.</returns>
    /// <exception cref="ArgumentException">Thrown if the line is corrupted or does not match the expected format.</exception>
    private Device ParsePC(string line, int lineNumber)
    {
        var parts = line.Split(',');
        if (parts.Length != 4)
        {
            throw new ArgumentException($"Line {lineNumber} is corrupted.");
        }
        return new PersonalComputer(parts[0], parts[1], bool.Parse(parts[2]), parts[3]);
    }

    /// <summary>
    /// Parses a line representing a <see cref="Smartwatch"/>.
    /// </summary>
    /// <param name="line">The line of data representing a smartwatch.</param>
    /// <param name="lineNumber">The line number in the input file.</param>
    /// <returns>A <see cref="Smartwatch"/> object.</returns>
    /// <exception cref="ArgumentException">Thrown if the line is corrupted or does not match the expected format.</exception>
    private Device ParseSmartwatch(string line, int lineNumber)
    {
        var parts = line.Split(',');
        if (parts.Length != 4)
        {
            throw new ArgumentException($"Line {lineNumber} is corrupted.");
        }
        return new Smartwatch(parts[0], parts[1], bool.Parse(parts[2]), int.Parse(parts[3].TrimEnd('%')));
    }

    /// <summary>
    /// Parses a line representing an <see cref="EmbeddedDevice"/>.
    /// </summary>
    /// <param name="line">The line of data representing an embedded device.</param>
    /// <param name="lineNumber">The line number in the input file.</param>
    /// <returns>An <see cref="EmbeddedDevice"/> object.</returns>
    /// <exception cref="ArgumentException">Thrown if the line is corrupted or does not match the expected format.</exception>
    private Device ParseEmbedded(string line, int lineNumber)
    {
        var parts = line.Split(',');
        if (parts.Length != 5)
        {
            throw new ArgumentException($"Line {lineNumber} is corrupted.");
        }
        return new EmbeddedDevice(parts[0], parts[1], bool.Parse(parts[2]), parts[3], parts[4]);
    }
}