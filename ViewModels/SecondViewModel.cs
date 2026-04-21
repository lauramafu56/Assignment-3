using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Linq;
using NetTopologySuite.Operation.Buffer;
using System.ComponentModel;

namespace Assignment3.ViewModels;
// Mi nueva mision: Conseguir rellenar la lista de SelectedFlights en base al selected airport
public partial class SecondViewModel: ObservableObject
{
    public string Greeting { get; } = "On this view the user can select an airport and see the flights departing from it.";

    public AirportTakeData AirportTakeData { get; } = new AirportTakeData();
    public FlightTakeData FlightTakeData { get; } = new FlightTakeData();

    // Airports from the airport list
    public ObservableCollection<Airport> VisibleAirports { get; } = new();
    public ObservableCollection<Flights> AllFlights { get; }= new();//To store flight info
    public ObservableCollection<Flights> SelectedFlights { get; }= new();//To store flight info
    public ObservableCollection<string> AvailableStatuses { get; } = new();//To store available flight statuses Copilot


    public void Initialize()// this method is used to initialize the data entendible to the UI.
    {
      var airportData = LoadAirports();
      var flightData = LoadFlights();
      foreach (var airport in airportData.Airports)
      {
          VisibleAirports.Add(airport);
      }
      
      foreach (var flight in flightData.Flights)
      {
          AllFlights.Add(flight);
      }

      // Populate available statuses from the flights data. Copilot
      var uniqueStatuses = AllFlights
          .Select(f => f.Status)
          .Distinct()
          .OrderBy(s => s)
          .ToList();
      
      foreach (var status in uniqueStatuses)
      {
          AvailableStatuses.Add(status);
      }
    }
    public AirportTakeData LoadAirports()
    {
        string JsonAirpotData = File.ReadAllText("Flights.json");

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        AirportTakeData airportData = JsonSerializer.Deserialize<AirportTakeData>(JsonAirpotData, options)!;
        return airportData;
    }
    
    [ObservableProperty]
    public Airport? _SelectedAirport;

    [ObservableProperty]//Copilot
    public string? _SelectedStatus;

    [RelayCommand]
    private void GetDesiredFlights()
    {
        SelectedFlights.Clear();

       if (SelectedAirport == null)
            return;

        // Filter flights where the selected airport is the departure airport
        var filtered = AllFlights.Where(f => f.DepartureAirport == SelectedAirport.IataCode);

        // Additionally filter by status if a status is selected. Copilot
        if (!string.IsNullOrEmpty(SelectedStatus))
        {
            filtered = filtered.Where(f => f.Status == SelectedStatus);
        }

        foreach (var f in filtered)
            SelectedFlights.Add(f);
    }

    public SecondViewModel()//Here you can add logic to update the UI based on the selected airport
    {
        Initialize();
        
    }

//trying to add andrea's reading from JSON to take the info of the flights too.
    public FullData LoadFlights()
    {
      // 1. Leemos el archivo como un texto gigante (String)
      string jsonFlightData = File.ReadAllText("Flights.json");

      // 2. Configuramos para que no le importen las mayúsculas/minúsculas
      var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

      // 3. we convert the text en objects of C#
      FullData data = JsonSerializer.Deserialize<FullData>(jsonFlightData, options)!;

      return data;
    }
}
