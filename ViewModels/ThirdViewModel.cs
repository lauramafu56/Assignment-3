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
using CommunityToolkit.Mvvm.Input;


using LiveChartsCore.SkiaSharpView.SKCharts;


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
    private ISeries[] _pieSeries = Array.Empty<ISeries>();
    public ObservableCollection<ISeries> PieSeries { get; set; } = new();
    public ObservableCollection<ISeries> DurationSeries { get; set; } = new();
    public Axis[] DurationXAxes { get; set; }

    public ObservableCollection<Flights> AvailableFlights {get; set;}= new();

    // --- PROPIEDADES DE VISIBILIDAD (Renombradas) ---

[ObservableProperty]
private bool _showSeries = true; // Antes: PlatformsChart (Gráfica de Aerolíneas)

[ObservableProperty]
private bool _showSeries2 = true; // Antes: GenrePieChart (Gráfica de Ciudades)

[ObservableProperty]
private bool _showPieSeries = true; // Antes: SubscriptionTypePieChart (Gráfica de Estados)

[ObservableProperty]
private bool _showDurationSeries = false; // Para la gráfica del "Reto" de promedios

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
            if (HiddenCharts.Contains("Top Streaming Platforms by Usage"))
                chartsList.Add("Top Streaming Platforms by Usage");
                
            if (HiddenCharts.Contains("Most Streamed Music Genres"))
                chartsList.Add("Most Streamed Music Genres");
                
            if (HiddenCharts.Contains("Most Popular Subscription Type"))
                chartsList.Add("Most Popular Subscription Type");
                
            if (HiddenCharts.Contains("Minutes Streamed per day by Age"))
                chartsList.Add("Minutes Streamed per day by Age");

            if (HiddenCharts.Contains("Top Artists by Listeners"))
                chartsList.Add("Top Artists by Listeners");
                
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
    public void Show3chartsCommand()
    {
     
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
    }
    else if (chartName == "Show All Charts")
    {
        ShowSeries = true; 
        ShowSeries2 = true; 
        ShowPieSeries = true; 
        ShowDurationSeries = true;
    }
    
    // Quitamos la opción de la lista de "pendientes"
    HiddenCharts.Remove(chartName);
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
           DateTime salida = f.ScheduledDeparture;
            DateTime llegada = f.ScheduledArrival;
            
            // Si la llegada es "menor" que la salida, es que aterrizó al día siguiente
            if (llegada < salida) llegada = llegada.AddDays(1);
            
            return new { f.ArrivalAirport, Duracion = (llegada - salida).TotalMinutes };
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
