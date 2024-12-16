using TravelAgenc.Y.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using TravelAgency.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetTravelEndpointName = "GetTravel";     // Fixed const declaration for endpoint name

// Sample list of travels
List<TravelDto> travels = new List<TravelDto>
{
    new TravelDto(1, "Turkey", "Istanbul", 1000.0m, DateOnly.FromDateTime(DateTime.Now)),
    new TravelDto(2, "USA", "New York", 1500.0m, DateOnly.FromDateTime(DateTime.Now)),
    new TravelDto(3, "Germany", "Berlin", 1200.0m, DateOnly.FromDateTime(DateTime.Now)),
    new TravelDto(4, "France", "Paris", 1300.0m, DateOnly.FromDateTime(DateTime.Now)),
    new TravelDto(5, "England", "London", 1100.0m, DateOnly.FromDateTime(DateTime.Now))
};

// Endpoint to get a travel by id
app.MapGet("/travels/{id}", (int id) =>
{
    var travel = travels.Find(t => t.id == id);
    return travel is not null ? Results.Ok(travel) : Results.NotFound();
}).WithName(GetTravelEndpointName);

// Endpoint to create a new travel
app.MapPost("/travels", (CreateTravelsDto newTravel) =>
{
    // Create a new TravelDto and add to list
    TravelDto travelDto = new TravelDto(
        travels.Count + 1,  // Simple example to assign ID based on count
        newTravel.Name,
        newTravel.Genre,
        newTravel.price,
        newTravel.ReleaseDate
    );

    travels.Add(travelDto);

    // Returning a 201 Created response with the travel URL
    return Results.CreatedAtRoute(GetTravelEndpointName, new { id = travelDto.id }, travelDto);
});

// Endpoint to update an existing travel (PUT method)
app.MapPut("/travels/{id}", (int id, CreateTravelsDto updatedTravel) =>
{
    // Find the index of the travel item to update
    var index = travels.FindIndex(t => t.id == id);

    // If the travel doesn't exist, return NotFound
    if (index == -1)
    {
        return Results.NotFound();
    }

    // Update the travel
    travels[index] = new TravelDto(
        id,  // Retain the existing id
        updatedTravel.Name,
        updatedTravel.Genre,
        updatedTravel.price,
        updatedTravel.ReleaseDate
    );

    // Return a 204 No Content status as no content is returned after update
    return Results.NoContent();
});

app.MapDelete("/travels/{id}", (int id) =>
{
    // Kontrollo nëse ekziston ndonjë objekt me këtë id
    var travel = travels.Find(t => t.id == id);

    if (travel == null)
    {
        // Ktheje 404 Not Found nëse nuk gjendet
        return Results.NotFound($"Travel with ID {id} not found.");
    }

    // Fshi objektin me këtë id
    travels.RemoveAll(t => t.id == id);

    // Ktheje 204 No Content për një fshirje të suksesshme
    return Results.NoContent();
});


app.Run();
