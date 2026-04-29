using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

using LiveChartsCore.SkiaSharpView.Drawing;



using LiveChartsCore.SkiaSharpView.SKCharts;
using BruTile.Wmts.Generated;


namespace Assignment3.ViewModels;

public partial class ThirdViewModel: ObservableObject
{
    //this is the property that the graph is gonna read
   
    private List<Airport> Airports = new();
    private List<Flights> Flights= new();
    //graph 1
    public ISeries[] Series { get; set; }
    public Axis[] XAxes { get; set; }
    public Axis[] YAxes { get; set; }
    //graph 2
    public ISeries[] Series2 { get; set; }
    public Axis[] XAxes2 { get; set; }
    public Axis[] YAxes2 { get; set; }
    //Pie graph
    public ObservableCollection<ISeries> PieSeries { get; set; } = new();
    //duration graph
    public ObservableCollection<ISeries> DurationSeries { get; set; } = new();
    public Axis[] DurationXAxes { get; set; }
    // graph 3
     public ISeries[] Series3 { get; set; }
    public Axis[] XAxes3 { get; set; }
    public Axis[] YAxes3 { get; set; }
    //graph 4
    public ISeries[] Series4 { get; set; }
    public Axis[] XAxes4 { get; set; }
    public Axis[] YAxes4 { get; set; }

    public ObservableCollection<Flights> AvailableFlights {get; set;}= new();

    // --- PROPIEDADES DE VISIBILIDAD (Renombradas) ---

[ObservableProperty]
private bool _showSeries = true; // 

[ObservableProperty]
private bool _showSeries2 = true; // 

[ObservableProperty]
private bool _showPieSeries = true; // 
[ObservableProperty]
private bool _showDurationSeries = false; //
[ObservableProperty]
private bool _showSeries3 = false; //
[ObservableProperty]
private bool _showSeries4 = false; //

[ObservableProperty]
private bool _showEmptyState;
     public ObservableCollection<string> AvailableCharts 
    { 
        get
        {
            var chartsList = new ObservableCollection<string>();
            
            // If there are no hidden charts (all charts are visible), return empty collection
            if (HiddenCharts.Count == 0)
                return chartsList;
            
            // Add the "Show All Charts" option at the top if there are multiple hidden charts
            if (HiddenCharts.Count > 1)
                chartsList.Add("Show All Charts");
            
            // Add individual hidden charts
            if (HiddenCharts.Contains("Top Airlines"))
                chartsList.Add("Top Airlines");
                
            if (HiddenCharts.Contains("Flights per City"))
                chartsList.Add("Flights per City");
                
            if (HiddenCharts.Contains("Flight Status"))
                chartsList.Add("Flight Status");
                
            if (HiddenCharts.Contains("Average Duration"))
                chartsList.Add("Average Duration");

            if (HiddenCharts.Contains("Airports graph"))
                chartsList.Add("Airports graph");

            if (HiddenCharts.Contains("the last chart"))
                chartsList.Add("the last chart");
                
            return chartsList;
        }
    }
     private HashSet<string> _hiddenCharts = new HashSet<string>();
    private HashSet<string> HiddenCharts 
    { 
        get => _hiddenCharts;
        set
        {
            _hiddenCharts = value;
            OnPropertyChanged(nameof(AvailableCharts));
        }
    }

     public FullData LoadData3()
    {
        var jsonString3 = File.ReadAllText("Flights.json");

        var options3 = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        FullData data3 = JsonSerializer.Deserialize<FullData>(jsonString3, options3)!;
            
     return data3;   
    }

    
 
    [RelayCommand]
public void RemoveChart(string chartId)
{
    string? chartToRemove = null;
    
    switch (chartId)
    {
        case "Series":
            ShowSeries = false;
            chartToRemove = "Top Airlines";
            break;
        case "Series2":
            ShowSeries2 = false;
            chartToRemove = "Flights per City";
            break;
        case "PieSeries":
            ShowPieSeries = false;
            chartToRemove = "Flight Status";
            break;
        case "Duration":
            ShowDurationSeries = false;
            chartToRemove = "Average Duration";
            break;
          case "AirportSeries":
            ShowSeries3 = false;
            chartToRemove = "Airports graph";
            break;
          case "Lastone":
            ShowSeries4 = false;
            chartToRemove = "the last chart";
            break;
       

    }

    if (chartToRemove != null)
    {
        HiddenCharts.Add(chartToRemove);
        OnPropertyChanged(nameof(AvailableCharts));
        
        // El estado vacío se activa si todas están ocultas
        ShowEmptyState = !ShowSeries && !ShowSeries2 && !ShowPieSeries && !ShowDurationSeries;
    }
}
[RelayCommand]
public void AddChart(string chartName)
{

    if (chartName == "Average Duration")
    {
        // Calculamos los datos justo antes de mostrarla (por si acaso)
        var data = LoadData3();
        
       CalcularDuraciones(data.Flights);
        
        ShowDurationSeries = true; // ¡Aquí ocurre la magia!
        OnPropertyChanged(nameof(ShowDurationSeries));
        return;
    }
    if (chartName == "Top Airlines")
        {
            // Show the specified charts
            ShowSeries = true;

            // Update available charts
            OnPropertyChanged(nameof(AvailableCharts));
              
            HiddenCharts.Remove("Top Airlines"); 
           
            return;
        }
       if (chartName == "Flights per City")
        {
            // Show the specified charts
            ShowSeries2 = true;

            // Update available charts
            OnPropertyChanged(nameof(AvailableCharts));
              
            HiddenCharts.Remove("Flights per City"); 
           
            return;
        }
     if (chartName == "Flight Status")
        {
            // Show the specified charts
            ShowPieSeries = true;

            // Update available charts
            OnPropertyChanged(nameof(AvailableCharts));
              
            HiddenCharts.Remove("Flight Status"); 
           
            return;
        }
    
    
   if (chartName == "Show All Charts")
        {
            // Show all charts
            ShowSeries3 = true;
            ShowDurationSeries = true;
            ShowSeries4= true;
            
            // Hide empty state since charts are now visible
            ShowEmptyState = false;
            
            // Update available charts
            OnPropertyChanged(nameof(AvailableCharts));
            
             HiddenCharts.Remove("Average Duration"); 
             HiddenCharts.Remove("Airports graph"); 
             HiddenCharts.Remove("the last chart"); 
            return;
        }
    
    // Quitamos la opción de la lista de "pendientes"
    
   
      // Notificamos manualmente por si acaso
   
    OnPropertyChanged(nameof(AvailableCharts));
    
  
  
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
                Fill= new SolidColorPaint(SKColors.LightGreen),
                
            }
            
        };

        XAxes = new Axis[]
        {
        new Axis
        {
            Labels = topAirlines.Select(a => a.Name).ToArray(),
            LabelsRotation = 25, // To read better if the names are long //Para que se lean mejor si los nombres son largos

        }
        };
         YAxes = new Axis[]
        {
            new Axis
            {
                Name="Number of flights",
               
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
                Name = "Number of flights per city",
                Fill= new SolidColorPaint(SKColors.Orange),
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
        
        var statusGroups = Flights
        .GroupBy(f => f.Status)
        .Select(g => new { Status = g.Key, Count = g.Count() })
        .ToList();

        PieSeries.Clear();
        foreach (var group in statusGroups)
        {
            PieSeries.Add(new PieSeries<int>
            {
                Values = new[] { group.Count },
                Name = group.Status,
                // Usamos una forma más directa de configurar las etiquetas
                DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                // CORRECCIÓN: Usamos Coordinate.PrimaryValue
                DataLabelsFormatter = point => $"{point.Context.Series.Name}: {point.Coordinate.PrimaryValue}"
            });
        }
           var data = LoadData3();
    
    // Agrupamos por aeropuerto de llegada para ver cuánto se tarda en llegar a cada sitio
    var promedioPorDestino = data.Flights
        .Select(f => {
            // Intentamos convertir las strings a DateTime
           DateTime Departure = f.ScheduledDeparture;
            DateTime Arrival = f.ScheduledArrival;
            
            // Si la llegada es "menor" que la salida, es que aterrizó al día siguiente
            if (Arrival < Departure) Arrival = Arrival.AddDays(1);
            
            return new { f.ArrivalAirport, Duracion = (Arrival - Departure).TotalMinutes };
        })
        .GroupBy(x => x.ArrivalAirport)
        .Select(g => new { 
            Destino = g.Key, 
            Promedio = g.Average(x => x.Duracion) 
        })
        .OrderByDescending(x => x.Promedio)
        .Take(5)
        .ToList();

    DurationSeries.Clear();
    DurationSeries.Add(new RowSeries<double> // RowSeries hace las barras horizontales
    {
        Values = promedioPorDestino.Select(d => d.Promedio).ToArray(),
        Name = "Minutes Average"
    });

    DurationXAxes = new Axis[] {
        new Axis { Labels = promedioPorDestino.Select(d => d.Destino).ToArray(),
        }
    };

    HiddenCharts.Add("Average Duration"); 
    HiddenCharts.Add("Airports graph"); 
     HiddenCharts.Add("the last chart"); 
    OnPropertyChanged(nameof(AvailableCharts));


    Flights = data3.Flights;
         var top = Flights
            .GroupBy(f => f.DepartureAirport)// we make groups by name of the city
            .Select(group3 => new { //we select the name 
                NameOfCountry = group3.Key, 
                TotalCountrys = group3.Count() //we count how much fligths are there
            })

            .OrderByDescending(x => x.TotalCountrys)// we order from biggest to smallest
            .Take(6)//we take the 6 first for not make the graph ugly
            .ToList();

        Series3 = new ISeries[]
        {
            new ColumnSeries<int>
            {
                Values = top.Select(d => d.TotalCountrys).ToArray(),
                Name = "AirportSeries",
                Fill= new SolidColorPaint(SKColors.Purple),
                
            }
            
        };

        XAxes3 = new Axis[]
        {
        new Axis
        {
            Labels = top.Select(c => c.NameOfCountry).ToArray(),
            LabelsRotation = 25, // To read better if the names are long //Para que se lean mejor si los nombres son largos

        }
        };
         YAxes3 = new Axis[]
        {
            new Axis
            {
                Name="Number of flights",
               
            }
        };

          Flights = data3.Flights;


        var topDayOfTheWeek = Flights
            .GroupBy(f => f.ScheduledDeparture.DayOfWeek)// we make groups by the departure time of the airline
            .Select(group4 => new { //we select the name 
                NameOfTheDay = group4.Key.ToString(),
                TotalD = group4.Count() //we count how much fligths are there
            })
            .OrderByDescending(x => x.TotalD)// we order from biggest to smallest
            //.Take(7)//we take the 7 first for not make the graph ugly
            .ToList();

            
        Series4 = new ISeries[]
        {
            new ColumnSeries<int>
            {
                Values = topDayOfTheWeek.Select(b => b.TotalD).ToArray(),
                Name = "AirportSeries",
                Fill= new SolidColorPaint(SKColors.Salmon),
                
            }
            
        };
        

        XAxes4 = new Axis[]
        {
        new Axis
        {
            
            Labels = topDayOfTheWeek.Select(b => b.NameOfTheDay).ToArray(),
            LabelsRotation = 25, // To read better if the names are long //Para que se lean mejor si los nombres son largos

        }
        };
         YAxes4 = new Axis[]
        {
            new Axis
            {
                Name="Number of flights",
               
            }
        };

    }

    public void CalcularDuraciones(List<Flights> listaVuelos)
{
    var promedioData = listaVuelos
        .Select(f => {
            DateTime salida = f.ScheduledDeparture;
            DateTime llegada = f.ScheduledArrival;
            if (llegada < salida) llegada = llegada.AddDays(1);
            return new { f.ArrivalAirport, Duracion = (llegada - salida).TotalMinutes };
        })
        .GroupBy(x => x.ArrivalAirport)
        .Select(g => new { 
            Destino = g.Key, 
            Promedio = Math.Round(g.Average(x => x.Duracion), 2) 
        })
        .OrderByDescending(x => x.Promedio)
        .Take(5)
        .ToList();

    DurationSeries.Clear();
    DurationSeries.Add(new RowSeries<double> 
    {
        Values = promedioData.Select(d => d.Promedio).ToArray(),
        Name = "Minutes Average",
        Fill= new SolidColorPaint(SKColors.Pink),
        DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.End,
        DataLabelsFormatter = point => $"{point.Coordinate.PrimaryValue} min"
    });

    DurationXAxes = new Axis[] {
        new Axis { Labels = promedioData.Select(d => d.Destino).ToArray() }
    };
}
   

    [RelayCommand]
    public void ExportChartToImage()
    {
        var chart = new SKCartesianChart
        {
            Series = Series,
            XAxes = XAxes,
            Width = 900,
            Height = 600
        };

        using var image = chart.GetImage();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = File.OpenWrite("ExportedChart.png");
        data.SaveTo(stream);
    }

}
