using System;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
namespace Assignment3;

public class AirportTakeData
{  
        public List<Airport> Airports { get; set; }= new ();//List<Here goes lo cosa en singular de lo que quieres que se cree una lista> same_name_del_feature_del_JSON { get; set; }= new ();
        //public List<Flights> Flights { get; set; } = new();
}

public class Airport
{
    public string IataCode { get; set;}= string.Empty;
    public string Name {get; set;}= string.Empty;
     public string City {get; set;}= string.Empty;
     public string Country{get; set;}= string.Empty;
     public double Latitude {get; set;}= 0.0;
     public double Longitude {get; set;}= 0.0;
    public Airport(string iataCode, string name, string city, string country, double latitude, double longitude)//this is a constructor
    {
        IataCode = iataCode;
        Name = name;
        City = city;
        Country = country;
        Latitude = latitude;
        Longitude = longitude;
    }
}

