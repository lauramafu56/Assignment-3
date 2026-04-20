
using System;
using System.Text.Json;
using Assignment3.Views;
using Avalonia.Controls;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace Assignment3.ViewModels;
public partial class MainWindowViewModel : ViewModelBase
{
    private FirstView firstView;
    private SecondView secondView ;
    private ThirdView thirdView ;
    private FirstViewModel _firstPage;
    private ThirdViewModel _thirdPage;
    private List<Airport> allAirports = new();//for store airport info
    private List<Flights> allFlights = new();//for store flight info
    [ObservableProperty]
    private UserControl _currentView;

    public MainWindowViewModel()
    {
        
        var data = LoadData(); 
        
        allAirports = data.Airports;
        allFlights = data.Flights;

        firstView = new FirstView { DataContext = new FirstViewModel(allAirports, allFlights) };
        secondView = new SecondView { DataContext = new SecondViewModel() }; // Si la vista 2 no necesita datos globales, déjala vacía
        thirdView = new ThirdView { DataContext = new ThirdViewModel(allFlights) };
        
        _firstPage = new FirstViewModel(allAirports, allFlights);
        _thirdPage = new ThirdViewModel(allFlights);

        CurrentView = firstView;
    }

    public void NextView()
    {
        if (CurrentView == firstView)
        {
            CurrentView = secondView;
        }
        else if (CurrentView == secondView)
        {
            CurrentView = thirdView;
        }
        else
        {
            CurrentView = firstView;
        }
 
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
