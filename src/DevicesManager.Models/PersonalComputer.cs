using DevicesManager;

    public class PersonalComputer : Device
    {
        public string? OperatingSystem { get; set; }
        
        public PersonalComputer(string id, string name, bool isEnabled, string? operatingSystem) 
            : base(id, name, isEnabled)
        {
            if (!CheckId(id))
            {
                throw new ArgumentException("Invalid ID value. Required format: P-1", nameof(id));
            }
            
            OperatingSystem = operatingSystem;
        }
        
        public override void TurnOn()
        {
            if (OperatingSystem is null)
            {
                throw new EmptySystemException();
            }

            base.TurnOn();
        }

        public override string ToString()
        {
            return $"Personal Computer: {Id}, Name: {Name}, IsEnabled: {IsEnabled}, OperatingSystem: {OperatingSystem}";
        }

        private bool CheckId(string id) => id.StartsWith("P-");
    }