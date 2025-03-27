using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevicesManager.Tests;

/// <summary>
/// Unit tests for the <see cref="Device"/> class.
/// Tests various methods in the <see cref="DeviceManager"/> class, including adding, removing,
/// editing devices, turning them on and off, showing all devices, and saving data to a file.
/// </summary>
[TestClass]
[TestSubject(typeof(Device))]
public class DeviceTest
{
    private readonly DeviceManager deviceManager;

    public DeviceTest()
    {
        deviceManager = DeviceManagerFactory.CreateDeviceManager();
    }
    
    private const string TestFilePath = "/Users/danylooliinyk/programming/uni/apbd/DevicesManager/DevicesManager.Tests/test.txt";

        /// <summary>
        /// Tests if a device can be added to the device manager when the storage is not full.
        /// </summary>
        [TestMethod]
        public void AddDevice_ShouldAddDevice_WhenStorageIsNotFull()
        {
            var device = new Smartwatch("1", "Test Watch", false, 50);
            
            deviceManager.AddDevice(device);
            
            Assert.Contains(device, deviceManager.GetDevices());
        }
        
        /// <summary>
        /// Tests that a device cannot be added when the storage is full.
        /// An exception should be thrown indicating the storage is full.
        /// </summary>
        [TestMethod]
        public void AddDevice_ShouldNotAddDevice_WhenStorageIsFull()
        {
            
            deviceManager.ClearAllDevices();

            try
            {
                for (int i = 0; i < deviceManager.GetMaxCapacity(); i++) 
                {
                    deviceManager.AddDevice(new Smartwatch(i.ToString(), $"Test Watch {i}", false, 50));
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Unexpected exception thrown during setup: {ex.Message}");
            }

            var newDevice = new Smartwatch("16", "New Watch", false, 50);

            var exception = Assert.ThrowsException<Exception>(() => deviceManager.AddDevice(newDevice));
            Assert.AreEqual("Device storage is full.", exception.Message);
        }

        /// <summary>
        /// Tests that a device can be removed from the device manager when it exists.
        /// </summary>
        [TestMethod]
        public void RemoveDevice_ShouldRemoveDevice_WhenDeviceExists()
        {
            var device = new Smartwatch("1", "Test Watch", false, 50);
            deviceManager.AddDevice(device);
            
            deviceManager.RemoveDeviceById("1");
            
            Assert.DoesNotContain(device, deviceManager.GetDevices());
        }

        /// <summary>
        /// Tests that a device can be edited in the device manager when it exists.
        /// </summary>
        [TestMethod]
        public void EditDevice_ShouldEditDevice_WhenDeviceExists()
        {
            var device = new Smartwatch("1", "Test Watch", false, 50);
            deviceManager.AddDevice(device);
            
            deviceManager.EditDevice(new Smartwatch("1", "Test Watch", false, 75));
            
            Assert.AreEqual(75, ((Smartwatch)deviceManager.GetDevices().First(d => d.Id == "1")).getBatteryLevel());
        }

        /// <summary>
        /// Tests that a device can be turned on when it exists in the device manager.
        /// </summary>
        [TestMethod]
        public void TurnOnDevice_ShouldTurnOnDevice_WhenDeviceExists()
        {
            var device = new Smartwatch("1", "Test Watch", false, 50);
            deviceManager.AddDevice(device);
            
            deviceManager.TurnOnDevice("1");
            
            Assert.IsTrue(deviceManager.GetDevices().First(d => d.Id == "1").IsEnabled);
        }

        /// <summary>
        /// Tests that a device can be turned off when it exists in the device manager.
        /// </summary>
        [TestMethod]
        public void TurnOffDevice_ShouldTurnOffDevice_WhenDeviceExists()
        {
            var device = new Smartwatch("1", "Test Watch", true, 50);
            deviceManager.AddDevice(device);
            
            deviceManager.TurnOffDevice("1");
            
            Assert.IsFalse(deviceManager.GetDevices().First(d => d.Id == "1").IsEnabled);
        }

        /// <summary>
        /// Tests that all devices are correctly shown in the device manager.
        /// </summary>
        [TestMethod]
        public void ShowAllDevices_ShouldShowAllDevices()
        {
            var device1 = new Smartwatch("1", "Test Watch 1", false, 50);
            var device2 = new Smartwatch("2", "Test Watch 2", false, 60);
            deviceManager.AddDevice(device1);
            deviceManager.AddDevice(device2);
            
            var devices = deviceManager.GetDevices();
            
            Assert.Contains(device1, devices);
            Assert.Contains(device2, devices);
        }

        /// <summary>
        /// Tests that the device manager correctly saves data to a file.
        /// </summary>
        [TestMethod]
        public void SaveDataToFile_ShouldSaveDataToFile()
        {
            var device = new Smartwatch("SW-1", "Apple Watch SE2" ,true,27);
            deviceManager.AddDevice(device);
            var saveFilePath = "/Users/danylooliinyk/programming/uni/apbd/DevicesManager/DevicesManager.Tests/test.txt";
            
            deviceManager.SaveDevices(saveFilePath);
            
            var savedLines = File.ReadAllLines(saveFilePath);
            foreach (var line in savedLines)
            {
                Console.WriteLine(line);
            }
            Assert.IsTrue(savedLines.Contains("SW-1,Apple Watch SE2,True,27"));
        }
}