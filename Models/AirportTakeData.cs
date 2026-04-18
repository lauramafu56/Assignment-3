using System;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;


using System.Linq;
using System.Runtime.CompilerServices;
namespace Assignment3;

public class AirportTakeData
{
   
    public List<Airport> AirportList { get; set; }= new ();//List<Here goes lo cosa en singular de lo que quieres que se cree una lista> Name_of_the_list { get; set; }= new ();
    public AirportTakeData()
    {
        LoadAirports();
    }

    private void LoadAirports()
    {
        string json = File.ReadAllText("Flights.json");

        var airports = JsonSerializer.Deserialize<List<Airport>>(json);

       //El if comprueba que la deserialización no falló, y si todo está bien, asigna la lista de aeropuertos deserializada a tu propiedad AirportList.
        if (airports != null)
        {
            AirportList = airports; 
        }   
    }
}
public class Airport
{
    public string IATACODE { get; set;}= string.Empty;
    public string NAME {get; set;}= string.Empty;
     public string CITY {get; set;}= string.Empty;
     public string COUNTRY{get; set;}= string.Empty;
     public double LATITUDE {get; set;}= 0.0;
     public double LONGITUDE {get; set;}= 0.0;
    public Airport(string iataCode, string name, string city, string country, double latitude, double longitude)
    {
        IATACODE = iataCode;
        NAME = name;
        CITY = city;
        COUNTRY = country;
        LATITUDE = latitude;
        LONGITUDE = longitude;
    }
}

