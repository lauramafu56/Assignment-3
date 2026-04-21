using System;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Collections.ObjectModel;

namespace Assignment3.ViewModels;

public partial class ThirdViewModel: ObservableObject
{
    //this is the property that the graph is gonna read
   
    private List<Airport> Airports = new();
    private List<Flights> Flights= new();
    public ISeries[] Series { get; set; }
    public Axis[] XAxes { get; set; }
    public ISeries[] Series2 { get; set; }
    public Axis[] XAxes2 { get; set; }
    public Axis[] YAxes2 { get; set; }
    public ISeries[] PieSeries { get; set; } 




    public ObservableCollection<Flights> AvailableFlights {get; set;}= new();

     public FullData LoadData3()
    {
        var jsonString3 = File.ReadAllText("Flights.json");

        var options3 = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        FullData data3 = JsonSerializer.Deserialize<FullData>(jsonString3, options3)!;
            
     return data3;   
    }
    public ThirdViewModel()
    {
       var data3 = LoadData3();
        
        // Llenamos la lista que vamos a usar 
        
        Flights = data3.Flights;


        var topAirlines = Flights
            .GroupBy(f => f.AirlineName)// we make groups by name of the airline
            .Select(group => new { //we select the name 
                Name = group.Key, 
                Total = group.Count() //we count how much fligths are there
            })
            .OrderByDescending(x => x.Total)// we order from biggest to smallest
            .Take(5)//we take the 5 first for not make the graph ugly
            .ToList();

        // graph configuration
        Series = new ISeries[]
        {
            new ColumnSeries<int>
            {
                Values = topAirlines.Select(a => a.Total).ToArray(),
                Name = "Total flights",
                
            }
            
        };

        XAxes = new Axis[]
        {
        new Axis
        {
            Labels = topAirlines.Select(a => a.Name).ToArray(),
            LabelsRotation = 25, // Para que se lean mejor si los nombres son largos
          
        }
        };

        Flights = data3.Flights;
         var topCountrys = Flights
            .GroupBy(f => f.ArrivalAirport)// we make groups by name of the city
            .Select(group2 => new { //we select the name 
                NameOfCity = group2.Key, 
                TotalFlights = group2.Count() //we count how much fligths are there
            })
            .OrderByDescending(x => x.TotalFlights)// we order from biggest to smallest
            .Take(5)//we take the 5 first for not make the graph ugly
            .ToList();


        Series2 = new ISeries[]
        {
            new ColumnSeries<int>
            {
                Values = topCountrys.Select(a => a.TotalFlights).ToArray(),
                Name = "Number of flights per city"
            }
        };

        XAxes2 = new Axis[]
        {
            new Axis
            {
                Labels = topCountrys.Select(a => a.NameOfCity).ToArray(),
                LabelsRotation = 15 // Para que se lean mejor si los nombres son largos

            }
        };
        YAxes2 = new Axis[]
        {
            new Axis
            {
                Name="Number of flights",
               
            }
        };

          PieSeries = new ISeries[]
        {
            new PieSeries<int>
            {
                Values = new []{ 2, 5, 4, 2, 6 },
                Name = "Income", 
                Stroke = null
            },
            
            
        };
    }
   
}
