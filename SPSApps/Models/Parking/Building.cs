namespace SPSApps.Models.Parking
{
    public class Building : BaseModel<int>
    {
        public string Address { get; set; } = "Unknown";
        public string Info { get; set; } = "";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int TotalAvailableParking { get; set; }
        public decimal FairPerParking { get; set; }
        public decimal EmergencyFairPerParking { get; set; }
        public bool IsEmergencyAvailable { get; set; }
        public string email { get; set; }
    }
}