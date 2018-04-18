namespace LunchBox
{
    public class Location
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Website { get; set; }

        public Location(string name, string imageUrl, string website)
        {
            Name = name;
            ImageUrl = imageUrl;
            Website = website;
        }
    }
}