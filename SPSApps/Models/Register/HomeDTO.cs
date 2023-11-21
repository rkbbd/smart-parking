using SPSApps.Models.Parking;

namespace SPSApps.Models.Register
{
    public record HomeDTO(string Name, string Email, List<Building> Buildings);
    public record ConfirmDTO(string Name, string Email, Building Buildings, bool isEmergency);
}
