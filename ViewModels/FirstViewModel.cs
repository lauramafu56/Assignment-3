using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Mapsui;
using Mapsui.Layers;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using Mapsui.Tiling;
using Mapsui.Nts; // For the Geometries
using NetTopologySuite.Geometries; // For the lines
using Mapsui.Projections; // Para SphericalMercator
using System.Collections.Generic;

namespace Assignment3.ViewModels;

public partial class FirstViewModel: ObservableObject
{
    
    
    public string Greeting { get; } = "Choose the airport that you are interested in";
    // Estas son las "cajas" donde guardaremos la información
    private List<Airport> allAirports = new();
    private List<Flights> allFlights = new();
    public ObservableCollection<Flights> VisibleFlights { get; set; } = new();
    
      [ObservableProperty]
    private Map _mymap;
    public void Initialize() 
    {
        var data = LoadData(); 
    
    // copy the flights from JSON to our observable list
        foreach (var f in data.Flights)
        {
        VisibleFlights.Add(f);
    }
}
    public void DrawFlightRoutes(string originIata)
{
    var originAirp = allAirports.FirstOrDefault(a => a.IataCode == originIata);
    if (originAirp == null) return;

    var flightsFromOrigin = allFlights.Where(f => f.DepartureAirport == originIata).ToList();

    // CORRECCIÓN: Usamos una capa de memoria (MemoryLayer)
    var layer = new MemoryLayer { Name = "Routes" };
    var features = new List<GeometryFeature>(); // Lista temporal de dibujos

    var startPoint = SphericalMercator.FromLonLat(originAirp.Longitude, originAirp.Latitude);

    foreach (var flight in flightsFromOrigin)
    {
        var destAirp = allAirports.FirstOrDefault(a => a.IataCode == flight.ArrivalAirport);
        if (destAirp == null) continue;

        var endPoint = SphericalMercator.FromLonLat(destAirp.Longitude, destAirp.Latitude);

        // Creamos la línea
        var line = new LineString(new[] { 
        new Coordinate(startPoint), 
        new Coordinate(endPoint) 
        });

        // Añadimos la línea a nuestra lista de dibujos
        features.Add(new GeometryFeature(line));
    }

    // Asignamos los dibujos a la capa
    layer.Features = features;

    // Añadimos la capa al mapa
    Mymap.Layers.Add(layer);
    Mymap.Refresh();
}

  

    public FirstViewModel()
    {
              // _airports = dataService.LoadAirports("Data/airports.json");
        // Create the map
        _mymap = new Map();
       
        _mymap.Layers.Add(OpenStreetMap.CreateTileLayer());
      
        //Create layers
        var airportLayer = new MemoryLayer { Name = "Airports" };
        var routeLayer = new MemoryLayer { Name = "Routes" };

        
        _mymap.Layers.Add(airportLayer);
        _mymap.Layers.Add(routeLayer);
      
     
    

    }

  

            public FlightTakeData LoadData()
        {
            // 1. Leemos el archivo como un texto gigante (String)
            string jsonString = File.ReadAllText("Flights.json");

            // 2. Configuramos para que no le importen las mayúsculas/minúsculas
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // 3. we convert the text en objects of C#
            FlightTakeData data = JsonSerializer.Deserialize<FlightTakeData>(jsonString, options)!;
            
            return data;
        }
}
