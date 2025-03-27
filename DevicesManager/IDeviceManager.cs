namespace DevicesManager;

/// <summary>
    /// Defines methods for managing devices, including adding, editing, removing, and controlling device states.
    /// </summary>
    public interface IDeviceManager
    {
        /// <summary>
        /// Adds a new device to the device collection.
        /// </summary>
        /// <param name="newDevice">The device to be added.</param>
        void AddDevice(Device newDevice);

        /// <summary>
        /// Edits an existing device in the device collection.
        /// </summary>
        /// <param name="editDevice">The device with updated information.</param>
        void EditDevice(Device editDevice);

        /// <summary>
        /// Removes a device from the device collection by its ID.
        /// </summary>
        /// <param name="deviceId">The ID of the device to be removed.</param>
        void RemoveDeviceById(string deviceId);

        /// <summary>
        /// Turns on the device with the specified ID.
        /// </summary>
        /// <param name="deviceId">The ID of the device to turn on.</param>
        void TurnOnDevice(string deviceId);

        /// <summary>
        /// Turns off the device with the specified ID.
        /// </summary>
        /// <param name="deviceId">The ID of the device to turn off.</param>
        void TurnOffDevice(string deviceId);

        /// <summary>
        /// Gets a device by its ID.
        /// </summary>
        /// <param name="deviceId">The ID of the device to retrieve.</param>
        /// <returns>The device with the specified ID, or null if not found.</returns>
        Device? GetDeviceById(string deviceId);

        /// <summary>
        /// Gets the list of all devices in the device collection.
        /// </summary>
        /// <returns>A list of all devices.</returns>
        List<Device> GetDevices();

        /// <summary>
        /// Saves the current list of devices to the specified file path.
        /// </summary>
        /// <param name="outputPath">The file path where the devices will be saved.</param>
        void SaveDevices(string outputPath);

        /// <summary>
        /// Clears all devices from the device collection.
        /// </summary>
        void ClearAllDevices();
    }