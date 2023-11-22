namespace SPSApps.Models.Parking
{
    public class RequestParking : BaseModel<int>
    {
        public int BuildingId { get; set; }
        public int Fair { get; set; }
        public int IsActive { get; set; }
        public DateTime AccessTime { get; set; }
        public Building Building { get; set; }
    }
}
