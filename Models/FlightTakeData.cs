using System;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;


using System.Linq;
using System.Runtime.CompilerServices;
namespace Assignment3;

public class FlightTakeData
{
    //public List<Flights> FlightsList { get; set; }= new ();//List<Here goes lo cosa en singular de lo que quieres que se cree una lista> Name_of_the_list { get; set; }= new ();
    public FlightTakeData()
    {
        //LoadFlights();
    }

    public List<Flights> LoadFlights(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<Flights>>(json);
    }

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
    
    public string FLIGHTNUMBER { get; set;}= string.Empty;
    public string ARLINENAME {get; set;}= string.Empty;
     public string DEPARTUREAIRPORT {get; set;}= string.Empty;
     public string AIRLINECODE{get; set;}= string.Empty;
     public string ARRIVALAIRPORT {get; set;}= string.Empty;
     public string SCHEDULEDDEPARTURE {get; set;}= string.Empty;
     public string SCHEDULEDARRIVAL {get; set;}= string.Empty;
     public string AIRCRAFTTYPE {get; set;}= string.Empty;
     public string STATUS {get; set;}= string.Empty;
     
     public Flights (string flightNumber, string airlineName, string departureAirport, string airlineCode, string arrivalAirport, string scheduledDeparture, string scheduledArrival, string aircraftType, string status)
     {
         FLIGHTNUMBER = flightNumber;
         ARLINENAME = airlineName;
         DEPARTUREAIRPORT = departureAirport;
         AIRLINECODE = airlineCode;
         ARRIVALAIRPORT = arrivalAirport;
         SCHEDULEDDEPARTURE = scheduledDeparture;
         SCHEDULEDARRIVAL = scheduledArrival;
         AIRCRAFTTYPE = aircraftType;
         STATUS = status;
     }

}