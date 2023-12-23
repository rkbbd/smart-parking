using SPSApps.Models.Parking;

namespace SPSApps.Models.Register
{
    public record HomeDTO(string Email, List<Building> Buildings, List<RequestParking> ParkingRequests);
    public record ConfirmDTO(string Email, Building Buildings, int isEmergency,bool isAvailable, double WaitingTime);
    public record ParkingRequest(int Id, int IsEmergency, int Hour, double WaitingTime, bool isAvailable);

    public record RequestParkingDTO
    (
        List<RequestParking> RequestParkings,
        List<RequestParking> history
    );
}
