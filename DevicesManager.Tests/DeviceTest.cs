using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevicesManager.Tests;

[TestClass]
[TestSubject(typeof(Device))]
public class DeviceTest
{
    
    private const string TestFilePath = "/Users/danylooliinyk/programming/uni/apbd/DevicesManager/DevicesManager.Tests/test.txt";

        [TestMethod]
        public void AddDevice_ShouldAddDevice_WhenStorageIsNotFull()
        {
            var deviceManager = new DeviceManager(TestFilePath);
            var device = new Smartwatch(1, "Test Watch", false, 50);
            
            deviceManager.AddDevice(device);
            
            Assert.Contains(device, deviceManager.GetDevices());
        }

        [TestMethod]
        public void AddDevice_ShouldNotAddDevice_WhenStorageIsFull()
        {
            var deviceManager = new DeviceManager(TestFilePath);
            for (int i = 0; i < 15; i++)
            {
                deviceManager.AddDevice(new Smartwatch(i, $"Test Watch {i}", false, 50));
            }
            var newDevice = new Smartwatch(16, "New Watch", false, 50);
            
            deviceManager.AddDevice(newDevice);
            
            Assert.DoesNotContain(newDevice, deviceManager.GetDevices());
        }

        [TestMethod]
        public void RemoveDevice_ShouldRemoveDevice_WhenDeviceExists()
        {
            var deviceManager = new DeviceManager(TestFilePath);
            var device = new Smartwatch(1, "Test Watch", false, 50);
            deviceManager.AddDevice(device);
            
            deviceManager.RemoveDevice(1);
            
            Assert.DoesNotContain(device, deviceManager.GetDevices());
        }

        [TestMethod]
        public void EditDevice_ShouldEditDevice_WhenDeviceExists()
        {
            var deviceManager = new DeviceManager(TestFilePath);
            var device = new Smartwatch(1, "Test Watch", false, 50);
            deviceManager.AddDevice(device);
            
            deviceManager.EditDevice("Test Watch", 75);
            
            Assert.AreEqual(75, ((Smartwatch)deviceManager.GetDevices().First(d => d.Id == 1)).BatteryPercentage);
        }

        [TestMethod]
        public void TurnOnDevice_ShouldTurnOnDevice_WhenDeviceExists()
        {
            var deviceManager = new DeviceManager(TestFilePath);
            var device = new Smartwatch(1, "Test Watch", false, 50);
            deviceManager.AddDevice(device);
            
            deviceManager.TurnOnDevice("Test Watch");
            
            Assert.IsTrue(deviceManager.GetDevices().First(d => d.Id == 1).IsDeviceTurnedOn);
        }

        [TestMethod]
        public void TurnOffDevice_ShouldTurnOffDevice_WhenDeviceExists()
        {
            var deviceManager = new DeviceManager(TestFilePath);
            var device = new Smartwatch(1, "Test Watch", true, 50);
            deviceManager.AddDevice(device);
            
            deviceManager.TurnOffDevice("Test Watch");
            
            Assert.IsFalse(deviceManager.GetDevices().First(d => d.Id == 1).IsDeviceTurnedOn);
        }

        [TestMethod]
        public void ShowAllDevices_ShouldShowAllDevices()
        {
            var deviceManager = new DeviceManager(TestFilePath);
            var device1 = new Smartwatch(1, "Test Watch 1", false, 50);
            var device2 = new Smartwatch(2, "Test Watch 2", false, 60);
            deviceManager.AddDevice(device1);
            deviceManager.AddDevice(device2);
            
            var devices = deviceManager.GetDevices();
            
            Assert.Contains(device1, devices);
            Assert.Contains(device2, devices);
        }

        [TestMethod]
        public void SaveDataToFile_ShouldSaveDataToFile()
        {
            var deviceManager = new DeviceManager(TestFilePath);
            var device = new Smartwatch(1, "Apple Watch SE2" ,true,27);
            deviceManager.AddDevice(device);
            var saveFilePath = "/Users/danylooliinyk/programming/uni/apbd/DevicesManager/DevicesManager.Tests/test.txt";
            
            deviceManager.SaveDataToFile(saveFilePath);
            
            var savedLines = File.ReadAllLines(saveFilePath);
            foreach (var line in savedLines)
            {
                Console.WriteLine(line);
            }
            Assert.IsTrue(savedLines.Contains("SW-1,Apple Watch SE2,True,27%"));
        }
}