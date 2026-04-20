using System;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using System.Linq;

namespace Assignment3.ViewModels;

public partial class ThirdViewModel
{
    //this is the property that the graph is gonna read
    public ISeries[] Series { get; set; }

    public ThirdViewModel(List<Flights> allFlights)
    {
       
        var topAirlines = allFlights
            .GroupBy(f => f.AirlineName)// we make groups by name of the airline
            .Select(grupo => new { //we select the name 
                Nombre = grupo.Key, 
                Total = grupo.Count() //we count how much fligths are there
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
                Name = "Total flights"
            }
        };
    }
}
