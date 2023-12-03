using SPSApps.Models.Parking;

namespace SPSApps.Models.Register
{
    public record HomeDTO(string Name, string Email, List<Building> Buildings);
    public record ConfirmDTO(string Name, string Email, Building Buildings, int isEmergency);
    public record ParkingRequest(int Id, int IsEmergency);
    public record RequestParkingDTO
    (
        List<RequestParking> RequestParkings,
        string Name, string Email

    );
}
