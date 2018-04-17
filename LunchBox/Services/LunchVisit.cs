namespace LunchBox.Services
{
    public class LunchVisit
    {
        public string Person { get; set; }
        public string Location { get; set; }
        public byte FoodRating { get; set; }
        public string Review { get; set; }

        public LunchVisit(string person, string location, byte foodRating, string review = null)
        {
            Person = person;
            Location = location;
            FoodRating = foodRating;
            Review = review;
        }
    }
}