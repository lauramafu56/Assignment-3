namespace Assignment3.ViewModels;
using System.Text.Json;
using System.Linq;
using System.Collections.ObjectModel;
using System.IO;
using Assignment3.Views;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class MainWindowViewModel : ViewModelBase
{
    private FirstView firstView {get;} = new FirstView() { DataContext= new FirstViewModel() };
    private SecondView secondView {get;} = new SecondView() { DataContext= new SecondViewModel() };
    private ThirdView thirdView {get;} = new ThirdView() { DataContext= new ThirdViewModel() };
    private FullData? data;

    [ObservableProperty]
    private UserControl _currentView;

    public MainWindowViewModel()
    {
        CurrentView = firstView;
        LoadData();
    }

    private void LoadData()
    {
        string jsonString = File.ReadAllText("Flights.json");
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        data = JsonSerializer.Deserialize<FullData>(jsonString, options);
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
    public void ExportFlightsToJson()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(data, options);
        File.WriteAllText("ExportedFlights.json", json);
    }
}
