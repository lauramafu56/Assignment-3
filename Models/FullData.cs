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
/*Preguntas:

- Could from the beginning of the project, I have created a class that contains both the list of flights and the list of airports?

- There should be an easier way to access to the deserialized info of the JSON right?
  For example on second view model we had deserialized the Json twice, one for the flights and one for the airports, 
  but i guess we could had done it once, right?

*/



