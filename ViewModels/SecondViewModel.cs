using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Assignment3.ViewModels;

public partial class SecondViewModel
{
    public string Greeting { get; } = "Here you can find the information about the airport that you are choosing, so choose an airport first";

      public AirportTakeData AirportTakeData { get; } = new AirportTakeData();

    // Airports from the airport list
    public ObservableCollection<Airport> Airports { get; } = new();
}
