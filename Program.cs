using System;
using System.Collections.Generic;
using System.Linq;
using Viagogo_Assessment.Models;

namespace Viagogo_Assessment
{
    class Program
    {
        static void Main(string[] args)
        {

            var events = new List<Event>
            {
                new Event{ Name= "Phantom of the Opera", City = "New York"},
                new Event {Name = "Metallica", City = "Los Angeles"},
                new Event{ Name = "Metallica", City = "New York"},
                new Event{ Name = "Metallica", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "New York"},
                new Event{ Name = "LadyGaGa", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "Chicago"},
                new Event{ Name = "LadyGaGa", City = "San Francisco"},
                new Event{ Name = "LadyGaGa", City = "Washington"}
            };

            var customer = new Customer { Name = "John Smith", City = "New York" };

            //var q = events.Where(x => x.City.Equals("New York"));

            foreach(var item in events)
            {
                //Check that the event to be sent matches or is close with the customer's city
                //The expected output for the Customer is that the event is being sent individually. The code is open to refactoring
                if(item.City.Equals(customer.City))
                {
                    AddToEmail(customer, item);
                }
            }

            // You do not need to know how these methods work
            static void AddToEmail(Customer c, Event e, int? price = null)
            {
                var distance = GetDistance(c.City, e.City);
                Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}"
                + (distance > 0 ? $" ({distance} miles away)" : "")
                + (price.HasValue ? $" for ${price}" : ""));
            }

            static int GetDistance(string fromCity, string toCity)
            {
                return AlphebiticalDistance(fromCity, toCity);
            }
            static int AlphebiticalDistance(string s, string t)
            {
                var result = 0;
                var i = 0;
                for (i = 0; i < Math.Min(s.Length, t.Length); i++)
                {
                    // Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
                    result += Math.Abs(s[i] - t[i]);
                }
                for (i = 0; i < Math.Max(s.Length, t.Length); i++)
                {
                    // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
                    result += s.Length > t.Length ? s[i] : t[i];
                }
                return result;
            }
        }

    }
}
