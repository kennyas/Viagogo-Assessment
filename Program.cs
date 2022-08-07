using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Viagogo_Assessment.Models;

namespace Viagogo_Assessment
{
    class Program
    {
        static void Main(string[] args)
        {

            var events = new List<Event>
            {
                new Event{ Name= "Phantom of the Opera", City = "New York City"},
                new Event {Name = "Metallica", City = "Los Angeles"},
                new Event{ Name = "Metallica", City = "New York City"},
                new Event{ Name = "Metallica", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "New York City"},
                new Event{ Name = "LadyGaGa", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "Chicago"},
                new Event{ Name = "LadyGaGa", City = "San Francisco"},
                new Event{ Name = "LadyGaGa", City = "Washington DC"}
            };

            var customer = new Customer { Name = "John Smith", City = "New York" };

            //var q = events.Where(x => x.City.Equals("New York"));

            foreach(var item in events)
            {
                //Check that the event to be sent matches or is close with the customer's city
                //The expected output for the Customer is that the event is being sent individually. The code is open to refactoring
                if (item.City.Equals(customer.City))
                {
                    AddToEmail(customer, item);
                }
                //AddToEmail(customer, item);
            }

            // You do not need to know how these methods work
            static void AddToEmail(Customer c, Event e, int? price = null)
            {
                var distance = GetDistance(c.City, e.City);
                Console.Out.WriteLine($"{c.Name} in {c.City}: {e.Name} in {e.City}"
                + (distance > 0 ? $" ({distance} miles away)" : "0")
                + (price.HasValue ? $" for ${price}" : ""));
            }
            //refactored GetDistance method
            static int GetDistance(string fromCity, string toCity)
            {
                double distanceInKilometers = GeoLocationDistanceBtwCitiesWithGoogleAPI(fromCity, toCity);
                double distanceInMiles = convertKilometersToMiles(distanceInKilometers);
                int distance = Convert.ToInt32(distanceInMiles);
                return distance; //< 250 ? distance  : distance;
                //return AlphebiticalDistance(fromCity, toCity);
            }
            //static int AlphebiticalDistance(string s, string t)
            //{
            //    var result = 0;
            //    var i = 0;
            //    if (s.Equals(t))
            //    {
            //        return 0;
            //    }
            //    for (i = 0; i < Math.Min(s.Length, t.Length); i++)
            //    {
            //        // Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
            //        result += Math.Abs(s[i] - t[i]);
            //    }
            //    for (i = 0; i < Math.Max(s.Length, t.Length); i++)
            //    {
            //        // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
            //        result += s.Length > t.Length ? s[i] : t[i];
            //    }
            //    return result;
            //}

            //Find distance between two cities using googlemap API
            static double GeoLocationDistanceBtwCitiesWithGoogleAPI(string fromCity, string destinationCity)
            {
                double distance = 0;
                double distanceInKilometers = 0;
                var unitValue = "imperial";
                string url = "https://maps.googleapis.com/maps/api/distancematrix/xml?destinations=" + destinationCity + "&origins=" + fromCity + "&units =" + unitValue + "&key=AIzaSyB72Gzq_9_BZ7ymZBBdP_-EuaNxfbIX_nI";
                WebRequest request = WebRequest.Create(url);
                using (WebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        DataSet dsResult = new DataSet();
                        dsResult.ReadXml(reader);
                        var dist = reader.ReadLine();
                        distance = Convert.ToDouble(dsResult.Tables["distance"].Rows[0]["value"]);//dsResult.Tables["distance"].Rows[0]["text"].ToString();
                        distanceInKilometers = distance / 1000;
                    }
                }

                return distanceInKilometers;
            }
            //convert generated distance in kilometers to equivalent miles
            static double convertKilometersToMiles(double distanceInKilometers)
            {
                return distanceInKilometers / 1.6; //* 0.621371192;
            }

        }
        
    }
}
