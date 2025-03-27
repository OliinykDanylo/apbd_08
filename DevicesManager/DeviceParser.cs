using System;

namespace DevicesManager;

public class DeviceParser : IDeviceParser
{
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

    private Device ParsePC(string line, int lineNumber)
    {
        var parts = line.Split(',');
        if (parts.Length != 4)
        {
            throw new ArgumentException($"Line {lineNumber} is corrupted.");
        }
        return new PersonalComputer(parts[0], parts[1], bool.Parse(parts[2]), parts[3]);
    }

    private Device ParseSmartwatch(string line, int lineNumber)
    {
        var parts = line.Split(',');
        if (parts.Length != 4)
        {
            throw new ArgumentException($"Line {lineNumber} is corrupted.");
        }
        return new Smartwatch(parts[0], parts[1], bool.Parse(parts[2]), int.Parse(parts[3].TrimEnd('%')));
    }

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