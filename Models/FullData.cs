using System;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
namespace Assignment3;

public class FullData
{  
    public List<Flights> Flights { get; set; } = new();
    public List<Airport> Airports { get; set; }= new ();//List<Here goes lo cosa en singular de lo que quieres que se cree una lista> same_name_del_feature_del_JSON { get; set; }= new ();
      
}



