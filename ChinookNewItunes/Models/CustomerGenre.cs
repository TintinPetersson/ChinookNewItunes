namespace ChinookNewItunes.Models
{
    public class CustomerGenre
    {
        public int CustomerId { get; set; }
        public string GenreName { get; set; }
        public int GenreCount { get; set; }
    }
    //public readonly record struct CustomerGenre(int CustomerId, string GernreName, int GernreCount);
}