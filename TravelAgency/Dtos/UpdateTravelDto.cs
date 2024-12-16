namespace TravelAgency.Dtos;

public record class UpdateTravelDto(int id, string Name, string Genre, decimal price, DateOnly ReleaseDate );