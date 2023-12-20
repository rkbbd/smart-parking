namespace SPSApps.Models.Parking
{
    public record BuildingDTO
    (
        string Address,
        string Info,
        decimal Latitude,
        decimal Longitude,
        int TotalAvailableParking,
        decimal FairPerParking,
        decimal EmergencyFairPerParking
    );
}
