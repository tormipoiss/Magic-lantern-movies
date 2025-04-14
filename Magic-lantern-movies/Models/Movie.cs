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
        Tragedy,
        Superhero,
        Epic,
        Disaster,
        Parody,
        Monster,
        Alien,
        Sport,
        Family,
        Motorsport,
        Musical,
        Mystery,
        Romance,
        Western
    }
    public enum AgeRatings
    {
        G,
        PG,
        PG_13,
        R,
        X,
        Not_Rated
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
        public string DirectorsJson
        {
            get => JsonSerializer.Serialize(Directors);
            set => Directors = JsonSerializer.Deserialize<List<string>>(value);
        }
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
        public List<string> Directors { get; set; } = new();

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

        [Ignore]
        public string FormattedPublicationDate
        {
            get
            {
                var culture = new System.Globalization.CultureInfo("en-US");
                return PublicationDate.ToString("MMMM yyyy", culture);
            }
        }

        [Ignore]
        public string FormattedDuration
        {
            get
            {
                if (Duration.Hours > 0)
                {
                    return $"{Duration.Hours}h {Duration.Minutes}m";
                }
                else
                {
                    return $"{Duration.Minutes}m";
                }
            }
        }

        [Ignore]
        public string FormattedRating
        {
            get
            {
                switch (Rating)
                {
                    case Ratings.VeryNegative:
                        return "Very Negative";
                    case Ratings.VeryGood:
                        return "Very Good";
                    default:
                        return Rating.ToString();
                }
            }
        }

        [Ignore]
        public string FormattedAgeRating
        {
            get
            {
                switch (AgeRating)
                {
                    case AgeRatings.PG_13:
                        return "PG 13";
                    case AgeRatings.Not_Rated:
                        return "Not Rated";
                    default:
                        return AgeRating.ToString();
                }
            }
        }

        [Ignore]
        public Color FormattedRatingColor
        {
            get
            {
                switch (Rating)
                {
                    case Ratings.VeryNegative:
                        return Colors.Red;
                    case Ratings.Negative:
                        return Colors.OrangeRed;
                    case Ratings.Neutral:
                        return Colors.Yellow;
                    case Ratings.Good:
                        return Colors.Green;
                    case Ratings.VeryGood:
                        return Colors.GreenYellow;
                    default:
                        return Colors.White;
                }
            }
        }
    }
}