using System;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;


using System.Linq;
using System.Runtime.CompilerServices;
namespace Assignment3;

public class FlightTakeData
{
    public List<Flights> Flights { get; set; } = new();
    //public List<Flights> FlightsList { get; set; }= new ();//List<Here goes lo cosa en singular de lo que quieres que se cree una lista> Name_of_the_list { get; set; }= new ();
   /* public FlightTakeData()
    {
        //LoadFlights();
    }
    public List<Flights> Flights { get; set; } = new();*/
/*
    public List<Flights> LoadFlights(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<Flights>>(json);
    }
*/
 /*public void LoadFlights()
    {

         string json = File.ReadAllText("Flights.json");
         var flights = JsonSerializer.Deserialize<List<Flights>>(json);

         if (flights !=null)
         {
            FlightsList = flights;
         }

    }*/

}
public class Flights
{
    public string FlightNumber { get; set; }
        public string AirlineName { get; set; }
        public string AirlineCode { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public DateTime ScheduledDeparture { get; set; } // C# converts automatically  text to date (fecha)
        public DateTime ScheduledArrival { get; set; }
        public string AircraftType { get; set; }
        public string Status { get; set; }
     
     public Flights (string flightNumber, string airlineName, string departureAirport, string airlineCode, string arrivalAirport, DateTime scheduledDeparture, DateTime scheduledArrival, string aircraftType, string status)
     {
         FlightNumber = flightNumber;
        AirlineName = airlineName;
         DepartureAirport = departureAirport;
         AirlineCode = airlineCode;
         ArrivalAirport = arrivalAirport;
         ScheduledDeparture = scheduledDeparture;
         ScheduledArrival  = scheduledArrival;
         AircraftType = aircraftType;
         Status = status;
     }

}