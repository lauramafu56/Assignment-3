using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace Assignment3.ViewModels;

public partial class SecondViewModel
{
    public string Greeting { get; } = "Here you can find the information about the airport that you are choosing, so choose an airport first";

      public AirportTakeData AirportTakeData { get; } = new AirportTakeData();

    // Airports from the airport list
    public ObservableCollection<Airport> VisibleAirports { get; } = new();


    public void Initialize()// this method is used to initialize the data entendible to the UI.
  {
    var airportData = LoadAirports();
    foreach (var airport in airportData.Airports)
    {
        VisibleAirports.Add(airport);
    }
  }
    public AirportTakeData LoadAirports()
    {
        string JsonAirpotData = File.ReadAllText("Flights.json");

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        AirportTakeData airportData = JsonSerializer.Deserialize<AirportTakeData>(JsonAirpotData, options)!;
        return airportData;
    }

    public SecondViewModel()
    {
        Initialize();
    }


}
