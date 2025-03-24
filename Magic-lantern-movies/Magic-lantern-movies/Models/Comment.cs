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
    public class Comment
    {
        public Guid ID { get; set; }
        public string CommentorName { get; set; }
        public string CommentText { get; set; }
        public CommentRatings Rating { get; set; }
        public DateTime Date { get; set; }
        //public Film Film { get; set; }
        public Guid FilmId { get; set; }
    }
}
