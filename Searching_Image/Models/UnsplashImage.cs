namespace Searching_Image.Models
{
    public class UnsplashImage
    {
        public string Id { get; set; }
        public Urls Urls { get; set; }
        public string Description { get; set; }
    }

    public class Urls
    {
        public string Regular { get; set; }
        public string Small { get; set; }
    }

}
