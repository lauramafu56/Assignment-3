using System;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace Assignment3.ViewModels;

public partial class ThirdViewModel
{
<<<<<<< Updated upstream
    private List<Flights> _flights;

    // El constructor ahora acepta la lista
   
    //this is the property that the graph is gonna read
    public ISeries[] Series { get; set; }

    public ThirdViewModel(List<Flights> allFlights)
    {
        _flights = allFlights;
       
        var topAirlines = allFlights
            .GroupBy(f => f.AirlineName)// we make groups by name of the airline
            .Select(grupo => new { //we select the name 
=======
    private List<Airport> Airports = new();
    private List<Flights> Flights = new();

    public ObservableCollection<string> TopAirlines { get; set; } = new();

    public FullData LoadData3()
    {
        var jsonString3 = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Flights.json"));
        var options3 = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        FullData data3 = JsonSerializer.Deserialize<FullData>(jsonString3, options3)!;
        return data3;   
    }

    public ThirdViewModel()
    {
        var data3 = LoadData3();
        Flights = data3.Flights;

        var topAirlines = Flights
            .GroupBy(f => f.AirlineName)
            .Select(grupo => new { 
>>>>>>> Stashed changes
                Nombre = grupo.Key, 
                Total = grupo.Count() 
            })
            .OrderByDescending(x => x.Total)
            .Take(5)
            .ToList();

        TopAirlines.Clear();
        foreach (var airline in topAirlines)
        {
<<<<<<< Updated upstream
            new ColumnSeries<int>
            {
                Values = topAirlines.Select(a => a.Total).ToArray(),
                Name = "Total flights"
            }
        };
=======
            TopAirlines.Add($"{airline.Nombre}: {airline.Total} flights");
        }
>>>>>>> Stashed changes
    }
}

