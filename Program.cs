//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading;
//using Viagogo_Assessment.Models;

//namespace Viagogo_Assessment
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {

//            var events = new List<Event>
//            {
//                new Event{ Name= "Phantom of the Opera", City = "New York City"},
//                new Event {Name = "Metallica", City = "Los Angeles"},
//                new Event{ Name = "Metallica", City = "New York City"},
//                new Event{ Name = "Metallica", City = "Boston"},
//                new Event{ Name = "LadyGaGa", City = "New York City"},
//                new Event{ Name = "LadyGaGa", City = "Boston"},
//                new Event{ Name = "LadyGaGa", City = "Chicago"},
//                new Event{ Name = "LadyGaGa", City = "San Francisco"},
//                new Event{ Name = "LadyGaGa", City = "Washington DC"}
//            };

//            var customer = new Customer { Name = "John Smith", City = "New York City" };

//            //var q = events.Where(x => x.City.Equals("New York"));

//            foreach(var item in events)
//            {
//                //Check that the event to be sent matches or is close with the customer's city
//                //The expected output for the Customer is that the event is being sent individually. The code is open to refactoring
//                if (item.City.Equals(customer.City))
//                {
//                    AddToEmail(customer, item);
//                }
//                //AddToEmail(customer, item);
//            }

//            // You do not need to know how these methods work
//            static void AddToEmail(Customer c, Event e, int? price = null)
//            {
//                var distance = GetDistance(c.City, e.City);
//                Console.Out.WriteLine($"{c.Name} in {c.City}: {e.Name} in {e.City}"
//                + (distance > 0 ? $" ({distance} miles away)" : " 0 miles away")
//                + (price.HasValue ? $" for ${price}" : ""));
//            }
//            //refactored GetDistance method
//            static int GetDistance(string fromCity, string toCity)
//            {
//                double distanceInKilometers = GeoLocationDistanceBtwCitiesWithGoogleAPI(fromCity, toCity);
//                double distanceInMiles = convertKilometersToMiles(distanceInKilometers);
//                int distance = Convert.ToInt32(distanceInMiles);
//                return distance; //< 250 ? distance  : distance;
//                //return AlphebiticalDistance(fromCity, toCity);
//            }
//            //static int AlphebiticalDistance(string s, string t)
//            //{
//            //    var result = 0;
//            //    var i = 0;
//            //    if (s.Equals(t))
//            //    {
//            //        return 0;
//            //    }
//            //    for (i = 0; i < Math.Min(s.Length, t.Length); i++)
//            //    {
//            //        // Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
//            //        result += Math.Abs(s[i] - t[i]);
//            //    }
//            //    for (i = 0; i < Math.Max(s.Length, t.Length); i++)
//            //    {
//            //        // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
//            //        result += s.Length > t.Length ? s[i] : t[i];
//            //    }
//            //    return result;
//            //}

//            //Find distance between two cities using googlemap API
//            static double GeoLocationDistanceBtwCitiesWithGoogleAPI(string fromCity, string destinationCity)
//            {
//                double distance = 0;
//                double distanceInKilometers = 0;
//                var unitValue = "imperial";
//                try
//                {
//                    string url = "https://maps.googleapis.com/maps/api/distancematrix/xml?destinations=" + destinationCity + "&origins=" + fromCity + "&units =" + unitValue + "&key=AIzaSyB72Gzq_9_BZ7ymZBBdP_-EuaNxfbIX_nI";
//                    WebRequest request = WebRequest.Create(url);
//                    using (WebResponse response = (HttpWebResponse)request.GetResponse())
//                    {
//                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
//                        {
//                            DataSet dsResult = new DataSet();
//                            dsResult.ReadXml(reader);
//                            // returned zero when the API response returns null value, this prevents the API error from preventing the loop to keep running
//                            distance = dsResult != null ? Convert.ToDouble(dsResult.Tables["distance"].Rows[0]["value"]) : 0;//dsResult.Tables["distance"].Rows[0]["text"].ToString();
//                            distanceInKilometers = distance / 1000;
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error occured while fetchin distance {ex}");   
//                }

//                return distanceInKilometers;
//            }
//            //convert generated distance in kilometers to equivalent miles
//            static double convertKilometersToMiles(double distanceInKilometers)
//            {
//                return distanceInKilometers / 1.6; //* 0.621371192;
//            }

//        }

//    }
//}


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Viagogo_Assessment
{
    public class Event
    {
        public string Name { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }
    }
    public class Customer
    {
        public string Name { get; set; }
        public string City { get; set; }
        public DateTime BirthDate { get; set; }
    }
    public class EventNotification
    {
        public Event Event { get; set; }
        public int? distance { get; set; }
        public int? Price { get; set; }
        public int? BirthDayDifference { get; set; }
    }
    public class Solution
    {
        static readonly Dictionary<string, int> cacheCityDistanceDict = new Dictionary<string, int>();
        static void Main(string[] args)
        {
            var events = new List<Event>{
                                        new Event{ Name = "Phantom of the Opera", City = "New York", Date= new DateTime(2022, 8, 1)},
                                        new Event{ Name = "Metallica", City = "Los Angeles", Date= new DateTime(2022, 8, 2)},
                                        new Event{ Name = "Metallica", City = "New York" , Date= new DateTime(2022, 8, 3)},
                                        new Event{ Name = "Metallica", City = "Boston",  Date= new DateTime(2022, 7, 30)},
                                        new Event{ Name = "LadyGaGa", City = "New York",  Date= new DateTime(2022, 8, 5)},
                                        new Event{ Name = "LadyGaGa", City = "Boston", Date= new DateTime(2022, 7, 31)},
                                        new Event{ Name = "LadyGaGa", City = "Chicago",  Date= new DateTime(2022, 8, 4)},
                                        new Event{ Name = "LadyGaGa", City = "San Francisco", Date= new DateTime(2022, 8, 7)},
                                        new Event{ Name = "LadyGaGa", City = "Washington", Date= new DateTime(2022, 7, 31)}
                                        };
            //1. find out all events that arein cities of customer
            // then add to email.
            var customer = new Customer { Name = "Mr. Fake", City = "New York", BirthDate = new DateTime(1989, 8, 3) };
            //var query = from result in customer
            //            where result.Contains("New York")
            //            select result;
            // 1. TASK SendEventsInCustomerCity
            var eventsInCustomerCity = events.Where(x => x.City == customer.City).ToList();
            Console.WriteLine($"Fetch events in customer city");
            foreach (var item in eventsInCustomerCity)
            {
                AddToEmail(customer, item);
            }
            Console.WriteLine("=================================================");
            // Find events close to customer City
            var eventsCloseToCustomerCity = events.Select(e => new EventNotification { Event = e, distance = FetchDistanceInDictionaryCache(customer.City, e.City) }).Where(x => x.distance >= 0).OrderBy(e => e.distance).Take(5);

            Console.WriteLine($"Fetch events closest to customer's city");
            foreach (var item in eventsCloseToCustomerCity)
            {
                AddToEmail(customer, item.Event);
            }
            Console.WriteLine("=================================================");
            // Find cheapest events
            var cheapestEvents = events.Select(e => new EventNotification { Event = e, Price = GetPrice(e) }).OrderBy(e => e.distance).ThenBy(e=>e.Price).Take(5);
            Console.WriteLine($"Fetch cheapest events for the customer");
            foreach (var item in cheapestEvents)
            {
                AddToEmail(customer, item.Event);
            }
            Console.WriteLine("=================================================");

            ////Fetch Events closest to Customer BirthDay
            //var eventsClosetoCustomerBirthday = events.Select(e => new EventNotification { Event = e, BirthDayDifference = GetMonthDayDifference(customer.BirthDate, e.Date) }).Where(e => e.BirthDayDifference >= 0).OrderBy(e => e.BirthDayDifference);

            //Console.WriteLine($"Fetch events closest to customer's BirthDay");
            //foreach (var item in eventsClosetoCustomerBirthday)
            //{
            //    AddToEmail(customer, item.Event);
            //}
            //Console.WriteLine("=================================================");
            static int FetchDistanceInDictionaryCache(string origin, string destination)
            {

                var cacheKey = $"{origin}:{destination}";
                bool isKeyFound = cacheCityDistanceDict.TryGetValue(cacheKey, out int distance);
                if (isKeyFound)
                {
                    return distance;
                }
                else if (origin.Equals(destination))
                {
                    cacheCityDistanceDict.Add(cacheKey, 0);
                }
                else
                {
                    try
                    {
                        distance = GetDistance(origin, destination);
                        cacheCityDistanceDict.Add(cacheKey, distance);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An exception has occurred while fetching the GetDistance method/API payloads(fromCity: {origin}, toCity: {destination}) with exception message: {ex.Message} ");
                        return -1;
                    }
                }
                return distance;
            }

            //static int GetMonthDayDifference(DateTime baseDate, DateTime dateCompared)
            //{
            //    try
            //    {
            //        //baseDate = new DateTime(dateCompared.Year, dateCompared.Month, dateCompared.Day);
            //        var differenceInDays = Convert.ToInt32((dateCompared.Day - baseDate.Day));
            //        return differenceInDays;
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"An error occurred while checking birthmonth difference with this exception {ex}");
            //        return -1;
            //    }
            //}
            /**
We want you to send an email to this customer with all events in their city
            * Just call AddToEmail(customer, event) for each event you think they should get
*/
        } // You do not need to know how these methods work
        static void AddToEmail(Customer c, Event e, int? price = null)
        {
            var distance = GetDistance(c.City, e.City);
            Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}"
            + (distance > 0 ? $" ({distance} miles away)" : "")
            + (price.HasValue ? $" for ${price}" : ""));
        }
        static int GetPrice(Event e)
        {
            return (AlphebiticalDistance(e.City, "") + AlphebiticalDistance(e.Name, "")) / 10;
        }
        static int GetDistance(string fromCity, string toCity)
        {
            return AlphebiticalDistance(fromCity, toCity);
        }
        private static int AlphebiticalDistance(string s, string t)
        {
            var result = 0;
            var i = 0;
            for (i = 0; i < Math.Min(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
                result += Math.Abs(s[i] - t[i]);
            }
            for (; i
            <
            Math.Max(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
                result += s.Length > t.Length ? s[i] : t[i];
            }
            return result;
        }
    }
} /*
var customers = new List<Customer>{
new Customer{ Name = "Nathan", City = "New York"},
new Customer{ Name = "Bob", City = "Boston"},
new Customer{ Name = "Cindy", City = "Chicago"},
new Customer{ Name = "Lisa", City = "Los Angeles"}
};
*/