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

        private Location[] _locations = new[]
        {
            new Location("Two Brother's BBQ", "https://www.2brosbbq.com/wp-content/uploads/2017/04/homepage_slide_five.png", "https://www.2brosbbq.com/"),
            new Location("Giant Eagle: Market District", "http://i.pinimg.com/736x/41/15/63/411563f102f78ce68970743777090b26.jpg", "https://www.marketdistrict.com/"),
            new Location("Central Diner", "https://s3-media3.fl.yelpcdn.com/bphoto/1AuRbMR0LLKZN8OqXyZzXQ/o.jpg", "http://www.centraldinerandgrille.com/"),
            new Location("Thai Foon", "https://s3-media2.fl.yelpcdn.com/bphoto/1HrOU3nHRPwQFnQoXkYGRw/o.jpg", "https://www.yelp.com/biz/thai-foon-pittsburgh"),
            new Location("Anthony's Coal Fired Pizza", "https://acfp.com/wp-content/uploads/2016/11/Settlers-Ridge-Exterior.jpg", "https://acfp.com/"),
            new Location("Blaze", "http://www.trbimg.com/img-58ef629d/turbine/os-bz-blaze-pizza-20170411", "http://blazepizza.com/"),
            new Location("Tamarind", "https://media-cdn.tripadvisor.com/media/photo-s/05/1a/e8/48/tamarind-savoring-india.jpg", "http://www.tamarindpa.com/"),
            new Location("Sheetz", "http://static2.businessinsider.com/image/5368f959ecad044607c385be-800-/1523726_10153808948220501_2128121275_o.jpg", "https://www.sheetz.com/"),
            new Location("Baby Face Cafe", "https://scontent.fagc3-1.fna.fbcdn.net/v/t1.0-9/1380613_536549299758117_123606619_n.jpg?_nc_cat=0&oh=3e669d4f7f4d94c245275b951345d4c3&oe=5B58193D", "https://www.facebook.com/BabyFacesOmegaCafe"),
            new Location("Cocina Latina", "https://static.wixstatic.com/media/d85962_c8c7fcabd2c04a079369d6ed0f6ba9a9~mv2.jpg/v1/fill/w_409,h_272,al_c,q_90,usm_0.66_1.00_0.01/d85962_c8c7fcabd2c04a079369d6ed0f6ba9a9~mv2.jpg", "https://www.cocinalatinatakeout.com/")
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
                    Location = _locations.Single(p => p.Name == grouping.Key),
                    Rating = grouping.Average(p => p.FoodRating)
                })
                .OrderByDescending(p => p.Rating)
                .FirstOrDefault();
        }
    }
}