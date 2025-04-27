namespace DevicesManager;

public class SmartWatch : Device
{
        private int _batteryLevel;
        
        public int getBatteryLevel()
        {
            return _batteryLevel;
        }
        
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
        
        public SmartWatch()
        {
        }
        
        public SmartWatch(string id, string name, bool isEnabled, int batteryLevel) : base(id, name, isEnabled)
        {
            if (!CheckId(id))
            {
                throw new ArgumentException("Invalid ID value. Required format: SW-1", id);
            }
            BatteryLevel = batteryLevel;
        }
        
        public void Notify()
        {
            Console.WriteLine($"Battery level is low. Current level is: {BatteryLevel}");
        }
        
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

        public override string ToString()
        {
            return $"SmartWatch: {base.ToString()}, Battery Level: {BatteryLevel}";
        }

        private bool CheckId(string id) => id.StartsWith("SW-");
}