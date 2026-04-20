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
using Mapsui.Projections; // for SphericalMercator
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
namespace Assignment3.ViewModels;

public partial class FirstViewModel: ObservableObject
{
    
    
    public string Greeting { get; } = "Choose the airport that you are interested in";
    
    private List<Airport> allAirports = new();//for store airport info
    private List<Flights> allFlights = new();//for store flight info
    public ObservableCollection<Flights> VisibleFlights { get; set; } = new();
    
      [ObservableProperty]
    private Map _mymap;


    [RelayCommand]
    public void ShowRoutes(string? originIata)
    {
        // Si el usuario no escribió nada, no hacemos nada
        if (string.IsNullOrWhiteSpace(originIata)) return;

        // Convertimos a mayúsculas por si el usuario escribió "cph" en minúsculas
        string code = originIata.ToUpper().Trim();
        //We clear the map before drawing the new lines
        ClearMap();
        // Llamamos a la lógica que ya escribimos
        DrawFlightRoutes(code);
    }
    public void DrawFlightRoutes(string originIata)
    {
        var originAirp = allAirports.FirstOrDefault(a => a.IataCode == originIata);
        if (originAirp == null) 
        return;

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
  
    private void LoadAll()
    {
        var data = LoadData();
        
        // Llenamos las listas que usa el mapa
        allAirports = data.Airports;
        allFlights = data.Flights;

        // Llenamos la lista que usa la Vista 2
        foreach (var f in data.Flights)
        {
            VisibleFlights.Add(f);
        }
        
    }
  

    public FirstViewModel()
    {
              // _airports = dataService.LoadAirports("Data/airports.json");
        // 1. Inicializamos la propiedad pública para que el Toolkit lance la notificación
        Mymap = new Map(); 
        Mymap.Layers.Add(OpenStreetMap.CreateTileLayer());

        // 2. ¡MUY IMPORTANTE! Llamamos a la carga de datos aquí
        LoadAll();
        // Create the map
        
  
        //Create layers
        var airportLayer = new MemoryLayer { Name = "Airports" };
        var routeLayer = new MemoryLayer { Name = "Routes" };

        
        _mymap.Layers.Add(airportLayer);
        _mymap.Layers.Add(routeLayer);
      
     
    

    }

        [RelayCommand]
        public void ClearMap()
        {
            // we search with LINQ for all the layers wich name is "Routes"
            var layersToDelete = Mymap.Layers.Where(l => l.Name == "Routes").ToList();

            foreach (var layer in layersToDelete)
            {
                Mymap.Layers.Remove(layer);
            }

            // we refresh the map
            Mymap.Refresh();
            
            
        }
  

            public FullData LoadData()
        {
            // 1. Leemos el archivo como un texto gigante (String)
            string jsonString = File.ReadAllText("Flights.json");

            // 2. Configuramos para que no le importen las mayúsculas/minúsculas
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // 3. we convert the text en objects of C#
            FullData data = JsonSerializer.Deserialize<FullData>(jsonString, options)!;
            
            return data;
        }
}
