namespace Assignment3.Tests;
using Assignment3.ViewModels;
using System.Collections.Generic;
using System.Linq;

public class SecondViewModelTests
{
    [Fact]
    public void FilterFlights_ByAirport_ShouldReturnCorrectResults()
    {
        // Arrange
        var viewModel = new SecondViewModel();// We create an instance of the SecondViewModel
        viewModel.Initialize();//We add data to the view model by calling the Initialize method, which loads the airports and flights data from the JSON file. 

        // Act
        viewModel.SelectedAirport = viewModel.VisibleAirports.FirstOrDefault(a => a.Name == "JFK International Airport");
        viewModel.GetDesiredFlightsCommand.Execute(null);

        // Assert
        Assert.All(viewModel.SelectedFlights, flight => Assert.Equal("JFK International Airport", flight.DepartureAirport));
    }
}
