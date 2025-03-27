using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

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
        Thriller,
        Crime,
        Tragedy
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
    [Table("movie")]
    public class Movie
    {
        [PrimaryKey, AutoIncrement]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string RatingString
        {
            get => Rating.ToString();
            set => Rating = Enum.Parse<Ratings>(value);
        }
        public string CategoriesJson
        {
            get => JsonSerializer.Serialize(Categories);
            set => Categories = JsonSerializer.Deserialize<List<Categories>>(value);
        }
        public string ActorsJson
        {
            get => JsonSerializer.Serialize(Actors);
            set => Actors = JsonSerializer.Deserialize<List<string>>(value);
        }
        public long PublicationDateTicks { get; set; }
        public string OriginalLanguage { get; set; }
        public string Director { get; set; }
        public long DurationTicks { get; set; }
        public string AgeRatingString
        {
            get => AgeRating.ToString();
            set => AgeRating = Enum.Parse<AgeRatings>(value);
        }

        [Ignore]
        public List<Categories> Categories { get; set; } = new();

        [Ignore]
        public List<string> Actors { get; set; } = new();

        [Ignore]
        public TimeSpan Duration
        {
            get => TimeSpan.FromTicks(DurationTicks);
            set => DurationTicks = value.Ticks;
        }

        [Ignore]
        public Ratings Rating { get; set; }

        [Ignore]
        public AgeRatings AgeRating { get; set; }

        [Ignore]
        public DateTime PublicationDate
        {
            get => new DateTime(PublicationDateTicks);
            set => PublicationDateTicks = value.Ticks;
        }

        [Ignore]
        public IEnumerable<Comment>? Comments { get; set; }
    }
}