using System.IO;
using System.Linq;
using DevicesManager;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace DevicesManager.Tests;

[TestClass]
[TestSubject(typeof(Device))]
public class DeviceTest
{
    
    private const string TestFilePath = "/Users/danylooliinyk/programming/uni/apbd/DevicesManager/DevicesManager/input.txt";

        [TestMethod]
        public void AddDevice_ShouldAddDevice_WhenStorageIsNotFull()
        {
            // Arrange
            var deviceManager = new DeviceManager(TestFilePath);
            var device = new Smartwatch(1, "Test Watch", false, 50);

            // Act
            deviceManager.AddDevice(device);

            // Assert
            Assert.Contains(device, deviceManager.GetDevices());
        }

        [TestMethod]
        public void AddDevice_ShouldNotAddDevice_WhenStorageIsFull()
        {
            // Arrange
            var deviceManager = new DeviceManager(TestFilePath);
            for (int i = 0; i < 15; i++)
            {
                deviceManager.AddDevice(new Smartwatch(i, $"Test Watch {i}", false, 50));
            }
            var newDevice = new Smartwatch(16, "New Watch", false, 50);

            // Act
            deviceManager.AddDevice(newDevice);

            // Assert
            Assert.DoesNotContain(newDevice, deviceManager.GetDevices());
        }

        [TestMethod]
        public void RemoveDevice_ShouldRemoveDevice_WhenDeviceExists()
        {
            // Arrange
            var deviceManager = new DeviceManager(TestFilePath);
            var device = new Smartwatch(1, "Test Watch", false, 50);
            deviceManager.AddDevice(device);

            // Act
            deviceManager.RemoveDevice(1);

            // Assert
            Assert.DoesNotContain(device, deviceManager.GetDevices());
        }

        [TestMethod]
        public void EditDevice_ShouldEditDevice_WhenDeviceExists()
        {
            // Arrange
            var deviceManager = new DeviceManager(TestFilePath);
            var device = new Smartwatch(1, "Test Watch", false, 50);
            deviceManager.AddDevice(device);

            // Act
            deviceManager.EditDevice(1, 75);

            // Assert
            Assert.Equals(75, ((Smartwatch)deviceManager.GetDevices().First(d => d.Id == 1)).BatteryPercentage);
        }

        [TestMethod]
        public void TurnOnDevice_ShouldTurnOnDevice_WhenDeviceExists()
        {
            // Arrange
            var deviceManager = new DeviceManager(TestFilePath);
            var device = new Smartwatch(1, "Test Watch", false, 50);
            deviceManager.AddDevice(device);

            // Act
            deviceManager.TurnOnDevice(1);

            // Assert
            Assert.IsTrue(deviceManager.GetDevices().First(d => d.Id == 1).IsDeviceTurnedOn);
        }

        [TestMethod]
        public void TurnOffDevice_ShouldTurnOffDevice_WhenDeviceExists()
        {
            // Arrange
            var deviceManager = new DeviceManager(TestFilePath);
            var device = new Smartwatch(1, "Test Watch", true, 50);
            deviceManager.AddDevice(device);

            // Act
            deviceManager.TurnOffDevice(1);

            // Assert
            Assert.IsFalse(deviceManager.GetDevices().First(d => d.Id == 1).IsDeviceTurnedOn);
        }

        [TestMethod]
        public void ShowAllDevices_ShouldShowAllDevices()
        {
            // Arrange
            var deviceManager = new DeviceManager(TestFilePath);
            var device1 = new Smartwatch(1, "Test Watch 1", false, 50);
            var device2 = new Smartwatch(2, "Test Watch 2", false, 60);
            deviceManager.AddDevice(device1);
            deviceManager.AddDevice(device2);

            // Act
            var devices = deviceManager.GetDevices();

            // Assert
            Assert.Contains(device1, devices);
            Assert.Contains(device2, devices);
        }

        [TestMethod]
        public void SaveDataToFile_ShouldSaveDataToFile()
        {
            // Arrange
            var deviceManager = new DeviceManager(TestFilePath);
            var device = new Smartwatch(1, "Test Watch", false, 50);
            deviceManager.AddDevice(device);
            var saveFilePath = "saved_devices.txt";

            // Act
            deviceManager.SaveDataToFile(saveFilePath);

            // Assert
            var savedLines = File.ReadAllLines(saveFilePath);
            Assert.Contains("SW-1,Test Watch,false,50%", savedLines);
        }
}