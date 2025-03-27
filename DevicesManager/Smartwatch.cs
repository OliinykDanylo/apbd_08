using DevicesManager;

    /// <summary>
    /// Represents a smartwatch with a battery level and power state.
    /// Implements the <see cref="IPowerNotifier"/> interface to notify when the battery is low.
    /// </summary>
    public class Smartwatch : Device, IPowerNotifier
    {
        private int _batteryLevel;

        /// <summary>
        /// Gets the current battery level of the smartwatch.
        /// </summary>
        /// <returns>The current battery level as an integer.</returns>
        public int getBatteryLevel()
        {
            return _batteryLevel;
        }

        /// <summary>
        /// Gets or sets the battery level of the smartwatch.
        /// If the battery level is below 20%, a low battery notification will be triggered.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the battery level is not between 0 and 100.</exception>
        public int BatteryLevel
        {
            get => _batteryLevel;
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new ArgumentException("Invalid battery level value. Must be between 0 and 100.", nameof(value));
                }
                
                _batteryLevel = value;
                if (_batteryLevel < 20)
                {
                    Notify();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Smartwatch"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the smartwatch.</param>
        /// <param name="name">The name of the smartwatch.</param>
        /// <param name="isEnabled">Indicates whether the smartwatch is enabled or not.</param>
        /// <param name="batteryLevel">The initial battery level of the smartwatch.</param>
        /// <exception cref="ArgumentException">Thrown if the ID format is invalid.</exception>
        public Smartwatch(string id, string name, bool isEnabled, int batteryLevel) : base(id, name, isEnabled)
        {
            if (CheckId(id))
            {
                throw new ArgumentException("Invalid ID value. Required format: SW-1", id);
            }
            BatteryLevel = batteryLevel;
        }

        /// <summary>
        /// Sends a notification when the battery level is low.
        /// </summary>
        public void Notify()
        {
            Console.WriteLine($"Battery level is low. Current level is: {BatteryLevel}");
        }

        /// <summary>
        /// Turns on the smartwatch. If the battery level is too low, an exception is thrown.
        /// </summary>
        /// <exception cref="EmptyBatteryException">Thrown if the battery level is below 11%.</exception>
        public override void TurnOn()
        {
            if (BatteryLevel < 11)
            {
                throw new EmptyBatteryException();
            }

            base.TurnOn();
            BatteryLevel -= 10;

            if (BatteryLevel < 20)
            {
                Notify();
            }
        }

        /// <summary>
        /// Returns a string representation of the smartwatch.
        /// </summary>
        /// <returns>A string describing the smartwatch, its ID, name, and battery level.</returns>
        public override string ToString()
        {
            string enabledStatus = IsEnabled ? "enabled" : "disabled";
            return $"Smartwatch {Name} ({Id}) is {enabledStatus} and has {BatteryLevel}%";
        }

        /// <summary>
        /// Checks if the ID is in a valid format for a smartwatch.
        /// </summary>
        /// <param name="id">The ID to check.</param>
        /// <returns>True if the ID contains the "SW-" prefix, otherwise false.</returns>
        private bool CheckId(string id) => id.Contains("SW-");
    }