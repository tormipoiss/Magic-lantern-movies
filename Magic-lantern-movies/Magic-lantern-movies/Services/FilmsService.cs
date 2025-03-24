using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    class FilmsService
    {
        private static List<Film> films = new()
        {
            new()
            {
                Name = "The Shawshank Redemption",
                Description = "A banker convicted of uxoricide forms a friendship over a quarter century with a hardened convict, while maintaining his innocence and trying to remain hopeful through simple compassion.",
                Rating = Ratings.VeryGood,
                Image = "https://upload.wikimedia.org/wikipedia/en/8/81/ShawshankRedemptionMoviePoster.jpg",
                Categories = new()
                {
                    Categories.Drama
                },
                Actors = new()
                {
                    "Tim Robbins", "Morgan Freeman", "Bob Gunton"
                },
                PublicationDate = new DateTime(1994, 9, 10),
                OriginalLanguage = "English",
                Director = "Frank Darabont",
                Duration = new TimeSpan(2, 22, 0),
                AgeRating = AgeRatings.R,
            },
            new()
            {
                Name = "The Godfather",
                Description = "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.",
                Rating = Ratings.VeryGood,
                Image = "https://upload.wikimedia.org/wikipedia/en/1/1c/Godfather_ver1.jpg",
                Categories = new()
                {
                    Categories.Drama, Categories.Tragedy, Categories.Crime
                },
                Actors = new ()
                {
                    "Marlon Brando", "Al Pacino", "James Caan"
                },
                PublicationDate = new DateTime(1972, 3, 14),
                OriginalLanguage = "English",
                Director = "Francis Ford Coppola",
                Duration = new TimeSpan(2, 55, 0),
                AgeRating = AgeRatings.R,
            }
        };
    }
}
