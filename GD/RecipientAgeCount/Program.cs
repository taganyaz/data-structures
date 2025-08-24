/**
Problem:

Let’s say we’re generating reports with aggregate statistics about recipients. These reports will include stats about recipient ages in different locations.
For this problem, please write a class `RecipientAgeCalculator` that allows us to calculate, for any geographic location, the recipient age breakdown.



RecipientAgeCalculator’s constructor has one argument: `geoHierarchy`, which is a map/dictionary that represents how geographic areas relate to each other. Each key is a geographic area identifier and its value is the geographic area identifier of the larger geographic area that contains it. For example, in Rwanda the country is split into provinces which are spit into districts. So the hierarchy might look like this:

new Dictionary<string, string> {
  {"District1", "Province1"},
  {"District2", "Province1"},
  {"District3", "Province2"},
  {"District4", "Province2"},
  {"Province1", "Rwanda"},
  {"Province2", "Rwanda"},
  {"Rwanda", null},
}
In this example, “District1” is in “Province1” which is in “Rwanda” which has no enclosing area because its value is null
The hierarchy can be arbitrarily deep, and may differ in depth and types of geographic areas from place to place
You can assume the geographic hierarchy won’t change over time


The class has the following methods:

`AddRecipient(Recipient recipient)` adds a new recipient to the recipients tracked by an instance of the class 

`Recipient` is an object with two properties: `string geoArea` and `int age`. The geo_area is a unique identifier of a geographic area (for example, a country, county, district, village, etc)
`GetAgeBreakdownForGeo(string geoArea, List<int> bucketDividers)` returns the age breakdown for all recipients tracked by this instance who live in the specified geo area or geo areas inside it.
`geoArea` - the geographic area identifier we want the age breakdown for
`bucketDividers` is a list of integers indicating the dividing ages between buckets. For example, if the bucket dividers are {17, 29, 44, 64} then you’ll get buckets "0-17", "18-29", "30-44", "45-64", and "65+". The divider is included in the younger bucket and not in the older bucket
The return value is a dictionary of age bucket to ratio of recipients in that bucket (specified as a double)


Ideally, neither of these methods should require re-processing all existing recipients (imagine we have a large number of recipients and this is performance sensitive).

**/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

// TODO: define RecipientAgeCalculator class
class RecipientAgeCalculator
{
  private Dictionary<string, List<int>> recipientsAgeByGeo;
  private Dictionary<string, string> geoHierachies;
  private Dictionary<string, HashSet<string>> geoDecedants;

  public RecipientAgeCalculator(Dictionary<string, string> geoAreas)
  {
    geoHierachies = geoAreas ?? new Dictionary<string, string>();
    recipientsAgeByGeo = new Dictionary<string, List<int>>();
    geoDecedants = new Dictionary<string, HashSet<string>>();

    BuildGeoHiearachy();
  }

  public void AddRecipient(Recipient recipient)
  {
    var geoArea = recipient.GeoArea;

    /*if(geoDecedants.ContainsKey(geoArea))
    {
      foreach(var decedant in geoDecedants[geoArea])
      {
        if (!recipientsAgeByGeo.ContainsKey(decedant))
        {
          recipientsAgeByGeo[decedant] = new List<int>();
        }
        recipientsAgeByGeo[decedant].Add(recipient.Age);
      }
    }
    else 
    {
      recipientsAgeByGeo[geoArea] = new List<int>() {recipient.Age};
    }*/
    if (!recipientsAgeByGeo.ContainsKey(geoArea))
        {
          recipientsAgeByGeo[geoArea] = new List<int>();
        }
        recipientsAgeByGeo[geoArea].Add(recipient.Age);
  }

  public Dictionary<string, double> GetAgeBreakdownForGeo(string geoArea, List<int> bucketDividers)
  {
      Dictionary<string, double> result = new Dictionary<string, double>();
      List<int> allAges = new List<int>();

      if (string.IsNullOrWhiteSpace(geoArea) || bucketDividers == null || bucketDividers.Count == 0)
      {
        return result;
      }

      if (geoDecedants.ContainsKey(geoArea))
      {
        foreach(var decedant in geoDecedants[geoArea])
        {
            if (recipientsAgeByGeo.ContainsKey(decedant))
            {
              allAges.AddRange(recipientsAgeByGeo[decedant]);
            }
        }
      }
      else
      {
        if (recipientsAgeByGeo.ContainsKey(geoArea))
        {
          allAges.AddRange(recipientsAgeByGeo[geoArea]);
        }
      }

      int agesCount = allAges.Count;
      if (agesCount == 0)
      {
        return result;
      }

      string bucketName = "";
      int ageBreakDownCount = 0;
      bucketDividers.Sort();

      for(int i = 0; i < bucketDividers.Count; i++)
      {
        if (i == 0)
        {
          bucketName  = $"0-{bucketDividers[i]}";
          ageBreakDownCount = allAges.Count(age => age <= bucketDividers[i]);
        }
        else
        {
          bucketName = $"{bucketDividers[i - 1] + 1}-{bucketDividers[i]}";
          ageBreakDownCount = allAges.Count(age => age > bucketDividers[i - 1] && age <= bucketDividers[i]);
        }

        
        result[bucketName] =  ageBreakDownCount / (double) agesCount;
      
      }
      bucketName = $"{bucketDividers[bucketDividers.Count - 1] + 1}+";
      ageBreakDownCount = allAges.Count(age => age > bucketDividers[bucketDividers.Count - 1]);
      result[bucketName] =  ageBreakDownCount / (double) agesCount;

      return result;
  }

  private void BuildGeoHiearachy()
  {
    foreach(var geoArea in geoHierachies.Keys)
    {
      var current = geoArea;
      while(current != null)
      {
        if (!geoDecedants.ContainsKey(current))
        {
          geoDecedants[current] = new HashSet<string>();
        }
        geoDecedants[current].Add(geoArea);

        geoHierachies.TryGetValue(current, out current);
      }
    }
  }

}

// TODO: define Recipient class
class Recipient
{
  public Recipient(string geoArea, int age)
  {
    GeoArea = geoArea;
    Age = age;
  }

  public string GeoArea{get; private set;}
  public int Age{get; private set;}
}

class MainClass { 
  static void Main(string[] args) {
    RunTests();
  }

  static void RunTests() {
    var geoHierarchy = new Dictionary<string, string> {
      {"Village1", "State1"},
      {"Village2", "State1"},
      {"Village3", "State1"},
      {"Village4", "State2"},
      {"Village5", "State2"},
      {"Village6", "State3"},
      {"Village7", "State3"},
      {"Village8", "State3"},
      {"State1", "CountryA"},
      {"State2", "CountryB"},
      {"State3", "CountryB"},
      {"CountryA", null},
      {"CountryB", null},
    };

    var ageCalculator = new RecipientAgeCalculator(geoHierarchy);

    var recipients = new List<Recipient> {
      new Recipient("Village1", 10),
      new Recipient("Village2", 50),
      new Recipient("Village3", 15),
      new Recipient("Village3", 71),
      new Recipient("Village4", 10),
      new Recipient("Village5", 25),
      new Recipient("Village6", 40),
      new Recipient("Village7", 50),
      new Recipient("Village8", 52),
    };

    foreach (var recipient in recipients) {
      ageCalculator.AddRecipient(recipient);
    }

    var dividers = new List<int> {17, 29, 44, 64};

    var countryAAnswer1 = ageCalculator.GetAgeBreakdownForGeo("CountryA", dividers);
    CollectionAssert.AreEqual(
      countryAAnswer1,
      new Dictionary<string, double>{
        {"0-17", 0.5},
        {"18-29", 0.0},
        {"30-44", 0.0},
        {"45-64", 0.25},
        {"65+", 0.25},
      },
      JsonSerializer.Serialize(countryAAnswer1)
    );

    var countryBAnswer1 = ageCalculator.GetAgeBreakdownForGeo("CountryB", dividers);
    CollectionAssert.AreEqual(
      countryBAnswer1,
      new Dictionary<string, double>{
        {"0-17", 0.2},
        {"18-29", 0.2},
        {"30-44", 0.2},
        {"45-64", 0.4},
        {"65+", 0.0},
      },
      JsonSerializer.Serialize(countryBAnswer1)
    );

    var state3Answer1 = ageCalculator.GetAgeBreakdownForGeo("State3", dividers);
    CollectionAssert.AreEqual(
      state3Answer1,
      new Dictionary<string, double>{
        {"0-17", 0.0},
        {"18-29", 0.0},
        {"30-44", 0.3333333333333333},
        {"45-64", 0.6666666666666666},
        {"65+", 0.0},
      },
      JsonSerializer.Serialize(state3Answer1)
    );

    ageCalculator.AddRecipient(new Recipient("Village8", 23));

    var countryAAnswer2 = ageCalculator.GetAgeBreakdownForGeo("CountryA", dividers);
    CollectionAssert.AreEqual(
      countryAAnswer2,
      new Dictionary<string, double>{
        {"0-17", 0.5},
        {"18-29", 0.0},
        {"30-44", 0.0},
        {"45-64", 0.25},
        {"65+", 0.25},
      },
      JsonSerializer.Serialize(countryAAnswer2)
    );

    var countryBAnswer2 = ageCalculator.GetAgeBreakdownForGeo("CountryB", dividers);
    CollectionAssert.AreEqual(
      countryBAnswer2,
      new Dictionary<string, double>{
        {"0-17", 0.16666666666666666},
        {"18-29", 0.3333333333333333},
        {"30-44", 0.16666666666666666},
        {"45-64", 0.3333333333333333},
        {"65+", 0.0},
      },
      JsonSerializer.Serialize(countryBAnswer2)
    );

    var state3Answer2 = ageCalculator.GetAgeBreakdownForGeo("State3", dividers);
    CollectionAssert.AreEqual(
      state3Answer2,
      new Dictionary<string, double>{
        {"0-17", 0.0},
        {"18-29", 0.25},
        {"30-44", 0.25},
        {"45-64", 0.5},
        {"65+", 0.0},
      },
      JsonSerializer.Serialize(state3Answer2)
    );

    Console.WriteLine("Tests Passed");
  }
}