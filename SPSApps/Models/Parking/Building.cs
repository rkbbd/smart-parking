namespace SPSApps.Models.Parking
{
    public class Building : BaseModel<int>
    {
        public string Address { get; set; } = "Unknown";
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int TotalAvailableParking { get; set; }
        public decimal FairPerParking { get; set; }
    }
}