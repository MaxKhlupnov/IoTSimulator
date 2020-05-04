namespace IoTSimulation
{

    public class SampleEventModel
    {
        public string device_id { get; set; }
        public string datetime { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string altitude { get; set; }
        public string speed { get; set; }
        public string battery_voltage { get; set; }
        public string cabin_temperature { get; set; }
        public string fuel_level { get; set; }
    }
}
