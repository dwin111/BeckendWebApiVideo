namespace TestWebApiOnlineMove.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string URLIMG { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }      
        public string URLVideo { get; set; }
        public long NumberViews { get; set; }
        public double Rate { get; set; }
        public long RateCount { get; set; }
        public List<string> Tags { get; set; }


    }
}
