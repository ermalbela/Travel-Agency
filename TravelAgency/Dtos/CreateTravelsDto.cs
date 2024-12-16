namespace TravelAgency.Dtos;

public record class CreateTravelsDto(string Name, string Genre, decimal price, DateOnly ReleaseDate);
