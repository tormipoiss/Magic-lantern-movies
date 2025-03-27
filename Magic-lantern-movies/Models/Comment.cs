using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public enum CommentRatings
    {
        VeryNegative,
        Negative,
        Neutral,
        Good,
        VeryGood
    }
    [Table("comment")]
    public class Comment
    {
        [PrimaryKey, AutoIncrement]
        public Guid ID { get; set; } // Stored as TEXT in SQLite

        public string CommentorName { get; set; }
        public string CommentText { get; set; }

        public string RatingString
        {
            get => Rating.ToString();
            set => Rating = Enum.Parse<CommentRatings>(value);
        }

        public long DateTicks { get; set; } // Store DateTime as ticks

        public Guid MovieID { get; set; } // Foreign key to Movie

        [Ignore]
        public CommentRatings Rating { get; set; }

        [Ignore]
        public DateTime Date
        {
            get => new DateTime(DateTicks);
            set => DateTicks = value.Ticks;
        }
    }
}