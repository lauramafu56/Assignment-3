namespace Assignment3.Tests;
using Assignment3.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class SecondViewModelTests
{
    private void LogSuccess(string testName)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✓ {testName} - PASSED");
        Console.ResetColor();
    }

    private void LogStart(string testName)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"→ Running: {testName}");
        Console.ResetColor();
    }

    [Xunit.Fact]
    public void FilterFlights_ByAirport_ShouldReturnCorrectResults()
    {
        LogStart("FilterFlights_ByAirport_ShouldReturnCorrectResults");
        // Arrange
        var viewModel = new SecondViewModel();

        // Act
        var selectedAirport = viewModel.VisibleAirports.FirstOrDefault(a => a.IataCode == "JFK");
        if (selectedAirport != null)
        {
            viewModel.SelectedAirport = selectedAirport;
            viewModel.GetDesiredFlightsCommand.Execute(null);

            // Assert
            Xunit.Assert.All(viewModel.SelectedFlights, flight => Xunit.Assert.Equal("JFK", flight.DepartureAirport));
        }
        LogSuccess("FilterFlights_ByAirport_ShouldReturnCorrectResults");
    }

    [Xunit.Fact]
    public void Initialize_ShouldLoadAirportsAndFlights()
    {
        LogStart("Initialize_ShouldLoadAirportsAndFlights");
        // Arrange & Act
        var viewModel = new SecondViewModel();

        // Assert
        Xunit.Assert.NotEmpty(viewModel.VisibleAirports);
        Xunit.Assert.NotEmpty(viewModel.AllFlights);
        LogSuccess("Initialize_ShouldLoadAirportsAndFlights");
    }

    [Xunit.Fact]
    public void Initialize_ShouldPopulateAvailableStatuses()
    {
        LogStart("Initialize_ShouldPopulateAvailableStatuses");
        // Arrange & Act
        var viewModel = new SecondViewModel();

        // Assert
        Xunit.Assert.NotEmpty(viewModel.AvailableStatuses);
        LogSuccess("Initialize_ShouldPopulateAvailableStatuses");
    }

    [Xunit.Fact]
    public void GetDesiredFlights_WithNullAirport_ShouldReturnEmptyList()
    {
        LogStart("GetDesiredFlights_WithNullAirport_ShouldReturnEmptyList");
        // Arrange
        var viewModel = new SecondViewModel();
        viewModel.SelectedAirport = null;

        // Act
        viewModel.GetDesiredFlightsCommand.Execute(null);

        // Assert
        Xunit.Assert.Empty(viewModel.SelectedFlights);
        LogSuccess("GetDesiredFlights_WithNullAirport_ShouldReturnEmptyList");
    }

    [Xunit.Fact]
    public void GetDesiredFlights_WithStatusFilter_ShouldReturnFilteredResults()
    {
        LogStart("GetDesiredFlights_WithStatusFilter_ShouldReturnFilteredResults");
        // Arrange
        var viewModel = new SecondViewModel();
        var selectedAirport = viewModel.VisibleAirports.FirstOrDefault();
        var selectedStatus = viewModel.AvailableStatuses.FirstOrDefault();

        // Act
        if (selectedAirport != null && selectedStatus != null)
        {
            viewModel.SelectedAirport = selectedAirport;
            viewModel.SelectedStatus = selectedStatus;
            viewModel.GetDesiredFlightsCommand.Execute(null);

            // Assert
            Xunit.Assert.All(viewModel.SelectedFlights, flight => Xunit.Assert.Equal(selectedStatus, flight.Status));
        }
        LogSuccess("GetDesiredFlights_WithStatusFilter_ShouldReturnFilteredResults");
    }

    [Xunit.Fact]
    public void ExportFlightsToJson_ShouldCreateJsonFile()
    {
        LogStart("ExportFlightsToJson_ShouldCreateJsonFile");
        // Arrange
        var viewModel = new SecondViewModel();
        const string filename = "ExportedFlightsTest.json";
        if (File.Exists(filename))
            File.Delete(filename);

        // Act
        viewModel.ExportFlightsToJson();

        // Assert
        Xunit.Assert.True(File.Exists("ExportedFlights.json"));
        LogSuccess("ExportFlightsToJson_ShouldCreateJsonFile");
    }

    [Xunit.Fact]
    public void LoadFlights_ShouldReturnValidData()
    {
        LogStart("LoadFlights_ShouldReturnValidData");
        // Arrange & Act
        var viewModel = new SecondViewModel();
        var data = viewModel.LoadFlights();

        // Assert
        Xunit.Assert.NotNull(data);
        Xunit.Assert.NotEmpty(data.Flights);
        LogSuccess("LoadFlights_ShouldReturnValidData");
    }

    [Xunit.Fact]
    public void LoadAirports_ShouldReturnValidData()
    {
        LogStart("LoadAirports_ShouldReturnValidData");
        // Arrange & Act
        var viewModel = new SecondViewModel();
        var data = viewModel.LoadAirports();

        // Assert
        Xunit.Assert.NotNull(data);
        Xunit.Assert.NotEmpty(data.Airports);
        LogSuccess("LoadAirports_ShouldReturnValidData");
    }

    [Xunit.Fact]
    public void Airport_Constructor_ShouldSetPropertiesCorrectly()
    {
        LogStart("Airport_Constructor_ShouldSetPropertiesCorrectly");
        // Arrange
        const string iataCode = "CPH";
        const string name = "Copenhagen Airport";
        const string city = "Copenhagen";
        const string country = "Denmark";
        const double latitude = 55.6179;
        const double longitude = 12.6560;

        // Act
        var airport = new Airport(iataCode, name, city, country, latitude, longitude);

        // Assert
        Xunit.Assert.Equal(iataCode, airport.IataCode);
        Xunit.Assert.Equal(name, airport.Name);
        Xunit.Assert.Equal(city, airport.City);
        Xunit.Assert.Equal(country, airport.Country);
        Xunit.Assert.Equal(latitude, airport.Latitude);
        Xunit.Assert.Equal(longitude, airport.Longitude);
        LogSuccess("Airport_Constructor_ShouldSetPropertiesCorrectly");
    }

    [Xunit.Fact]
    public void Flights_Constructor_ShouldSetPropertiesCorrectly()
    {
        LogStart("Flights_Constructor_ShouldSetPropertiesCorrectly");
        // Arrange
        const string flightNumber = "SK451";
        const string airlineName = "Scandinavian Airlines";
        const string departureAirport = "CPH";
        const string airlineCode = "SK";
        const string arrivalAirport = "AMS";
        var scheduledDeparture = new System.DateTime(2026, 1, 5, 7, 30, 0);
        var scheduledArrival = new System.DateTime(2026, 1, 5, 9, 35, 0);
        const string aircraftType = "Airbus A320";
        const string status = "Landed";

        // Act
        var flight = new Flights(flightNumber, airlineName, departureAirport, airlineCode, arrivalAirport, 
                                  scheduledDeparture, scheduledArrival, aircraftType, status);

        // Assert
        Xunit.Assert.Equal(flightNumber, flight.FlightNumber);
        Xunit.Assert.Equal(airlineName, flight.AirlineName);
        Xunit.Assert.Equal(departureAirport, flight.DepartureAirport);
        Xunit.Assert.Equal(airlineCode, flight.AirlineCode);
        Xunit.Assert.Equal(arrivalAirport, flight.ArrivalAirport);
        Xunit.Assert.Equal(scheduledDeparture, flight.ScheduledDeparture);
        Xunit.Assert.Equal(scheduledArrival, flight.ScheduledArrival);
        Xunit.Assert.Equal(aircraftType, flight.AircraftType);
        Xunit.Assert.Equal(status, flight.Status);
        LogSuccess("Flights_Constructor_ShouldSetPropertiesCorrectly");
    }
}
