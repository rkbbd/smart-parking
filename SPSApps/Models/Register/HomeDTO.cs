using SPSApps.Models.Parking;

namespace SPSApps.Models.Register
{
    public record HomeDTO(string Email, List<Building> Buildings, List<RequestParking> ParkingRequests);
    public record ConfirmDTO(string Email, Building Buildings, int isEmergency);
    public record ParkingRequest(int Id, int IsEmergency, int Hour);

    public record RequestParkingDTO
    (
        List<RequestParking> RequestParkings,
        List<RequestParking> history
    );
}
