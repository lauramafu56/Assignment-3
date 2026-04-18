using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Mapsui;
using Mapsui.Layers;
using Mapsui.UI.Avalonia;
using Mapsui.Tiling.Layers;

using Mapsui.Tiling;


namespace Assignment3.ViewModels;

public partial class FirstViewModel: ObservableObject
{
    public string Greeting { get; } = "Choose the airport that you are interested in";
    
    [ObservableProperty]
    private Map map;

    public FirstViewModel()
    {
        // Create the map
        Map = new Map();
       
        Map.Layers.Add(OpenStreetMap.CreateTileLayer());

// hoy me levantao muy tempranito estaban cantando my way los gorriones
      
        //Create layers
        var airportLayer = new MemoryLayer { Name = "Airports" };
        var routeLayer = new MemoryLayer { Name = "Routes" };

        
        Map.Layers.Add(airportLayer);
        Map.Layers.Add(routeLayer);
/*
        var line = new LineString(new[]
        {
            new MPoint(originLongitude, originLatitude),
            new MPoint(destinationLongitude, destinationLatitude)
        });
        var routeFeature = new LineStringFeature(line);
        
        routeLayer.Features.Add(routeFeature);*/
    }
}
