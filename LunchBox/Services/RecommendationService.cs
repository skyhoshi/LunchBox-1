using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LunchBox.Services
{
    public class RecommendationService
    {
        private Dictionary<string, string[]> _cuisineOptions = new Dictionary<string, string[]>
        {
            {
                "american", new string[]
                {
                    "Central Diner",
                    "Giant Eagle: Market District"
                }
            },
            {
                "indian", new string[]
                {
                    "Tamarind"
                }
            },
            {
                "bbq", new string[]
                {
                    "Two Brother's BBQ",
                    "Giant Eagle: Market District"
                }
            },
            {
                "fast food", new string[]
                {
                    "Sheetz",
                    "McDonalds",
                    "Chic-fil-a"
                }
            },
            {
                "mexican", new string[]
                {
                    "Cocina Latina",
                    "El Campesino"
                }
            },
            {
                "pizza", new string[]
                {
                    "Anthony's Coal Fired Pizza",
                    "Giant Eagle: Market District",
                    "Juliano's"
                }
            }
        };

        private LunchVisit[] _lunchVisits = new[]
        {
            new LunchVisit("jason", "Two Brother's BBQ", 4),
            new LunchVisit("jason", "Giant Eagle: Market District", 2),
            new LunchVisit("jason", "Central Diner", 4),
            new LunchVisit("jason", "Thai Foon", 1),
            new LunchVisit("jason", "Anthony's Coal Fired Pizza", 5),
            new LunchVisit("jason", "Blaze", 3),
            new LunchVisit("jason", "Tamarind", 4),
            new LunchVisit("jason", "Sheetz", 2),
            new LunchVisit("jason", "Baby Face Cafe", 2),
            new LunchVisit("jason", "Cocina Latina", 4),

            new LunchVisit("justin", "Two Brother's BBQ", 4),
            new LunchVisit("justin", "Giant Eagle: Market District", 4),
            new LunchVisit("justin", "Central Diner", 1),
            new LunchVisit("justin", "Thai Foon", 3),
            new LunchVisit("justin", "Anthony's Coal Fired Pizza", 3),
            new LunchVisit("justin", "Blaze", 4),
            new LunchVisit("justin", "Tamarind", 3),
            new LunchVisit("justin", "Sheetz", 1),
            new LunchVisit("justin", "Baby Face Cafe", 1),
            new LunchVisit("justin", "Cocina Latina", 4),

            new LunchVisit("steve", "Two Brother's BBQ", 3),
            new LunchVisit("steve", "Giant Eagle: Market District", 4),
            new LunchVisit("steve", "Central Diner", 5),
            new LunchVisit("steve", "Thai Foon", 3),
            new LunchVisit("steve", "Anthony's Coal Fired Pizza", 4),
            new LunchVisit("steve", "Blaze",2),
            new LunchVisit("steve", "Tamarind", 3),
            new LunchVisit("steve", "Sheetz", 1),
            new LunchVisit("steve", "Baby Face Cafe", 1),
            new LunchVisit("steve", "Cocina Latina", 4),

            new LunchVisit("stephanie", "Two Brother's BBQ", 5),
            new LunchVisit("stephanie", "Giant Eagle: Market District", 4),
            new LunchVisit("stephanie", "Central Diner", 4),
            new LunchVisit("stephanie", "Thai Foon", 2),
            new LunchVisit("stephanie", "Anthony's Coal Fired Pizza", 2),
            new LunchVisit("stephanie", "Blaze",3),
            new LunchVisit("stephanie", "Tamarind", 4),
            new LunchVisit("stephanie", "Sheetz", 1),
            new LunchVisit("stephanie", "Baby Face Cafe", 1),
            new LunchVisit("stephanie", "Cocina Latina", 5)
        };

        public Recommendation GetRecommendation(Criteria criteria)
        {
            var restaurantsInCuisine = _cuisineOptions.Where(p => criteria.Cuisines.Contains(p.Key)).SelectMany(p => p.Value).ToArray();

            return _lunchVisits
                .Where(p => criteria.Attendees.Contains(p.Person))
                .Where(p => !restaurantsInCuisine.Any() || restaurantsInCuisine.Contains(p.Location))
                .GroupBy(p => p.Location)
                .Select(grouping => new Recommendation
                {
                    Location = grouping.Key,
                    Rating = grouping.Average(p => p.FoodRating)
                })
                .OrderByDescending(p => p.Rating)
                .FirstOrDefault();
        }
    }
}