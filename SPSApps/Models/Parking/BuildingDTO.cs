namespace SPSApps.Models.Parking
{
    public record BuildingDTO
    (
        string Address,
        decimal Latitude,
        decimal Longitude,
        int TotalAvailableParking,
        decimal FairPerParking,
        string Name,
        string Email
    );
}
