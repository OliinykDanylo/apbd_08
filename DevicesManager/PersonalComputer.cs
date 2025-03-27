using DevicesManager;

/// <summary>
    /// Represents a personal computer with an operating system and power state.
    /// </summary>
    public class PersonalComputer : Device
    {
        /// <summary>
        /// Gets or sets the operating system of the personal computer.
        /// </summary>
        public string? OperatingSystem { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonalComputer"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the personal computer.</param>
        /// <param name="name">The name of the personal computer.</param>
        /// <param name="isEnabled">Indicates whether the personal computer is enabled or not.</param>
        /// <param name="operatingSystem">The operating system of the personal computer, or null if none.</param>
        /// <exception cref="ArgumentException">Thrown if the ID format is invalid.</exception>
        public PersonalComputer(string id, string name, bool isEnabled, string? operatingSystem) 
            : base(id, name, isEnabled)
        {
            if (!CheckId(id))
            {
                throw new ArgumentException("Invalid ID value. Required format: P-1", id);
            }
            
            OperatingSystem = operatingSystem;
        }

        /// <summary>
        /// Turns on the personal computer.
        /// </summary>
        /// <exception cref="EmptySystemException">Thrown if the operating system is not set.</exception>
        public override void TurnOn()
        {
            if (OperatingSystem is null)
            {
                throw new EmptySystemException();
            }

            base.TurnOn();
        }

        /// <summary>
        /// Returns a string representation of the personal computer.
        /// </summary>
        /// <returns>A string describing the personal computer, its ID, name, and power status.</returns>
        public override string ToString()
        {
            string enabledStatus = IsEnabled ? "enabled" : "disabled";
            string osStatus = OperatingSystem is null ? "has no OS" : $"has {OperatingSystem}";
            return $"PC {Name} ({Id}) is {enabledStatus} and {osStatus}";
        }

        /// <summary>
        /// Checks if the ID is in a valid format for a personal computer.
        /// </summary>
        /// <param name="id">The ID to check.</param>
        /// <returns>True if the ID contains the "P-" prefix, otherwise false.</returns>
        private bool CheckId(string id) => id.Contains("P-");
    }