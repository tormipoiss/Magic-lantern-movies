using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public enum Categories
    {
        Action,
        Comedy,
        Drama,
        Horror,
        SciFi,
        Documentary,
        Animation,
        Adventure,
        Fantasy,
        Thriller
    }
    public enum AgeRatings
    {
        G,
        PG,
        PG_13,
        R,
        X
    }
    public enum Ratings
    {
        VeryNegative,
        Negative,
        Neutral,
        Good,
        VeryGood
    }
    public class Film
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Ratings Rating { get; set; }
        public Categories Category { get; set; }
        public IEnumerable<string> Actors { get; set; }
        public DateTime PublicationDate { get; set; }
        public string OriginalLanguage { get; set; }
        public string Director { get; set; }
        public TimeSpan Duration { get; set; }
        public AgeRatings AgeRating { get; set; }
        public IEnumerable<Comment>? Comments { get; set; }
    }
}
