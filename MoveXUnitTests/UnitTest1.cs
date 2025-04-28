using System;
using Magic_lantern_movies;
using Models;
using Xunit;


namespace MoveXUnitTests

{
    public class CommentsTest
    {
        [Fact]
        public void RatingString_SetValidValue_ShouldUpdateRating()
        {

            //Arrange
            var comment = new Comment();

            //ACT
            comment.RatingString = "Good";


            //Assert

            Assert.Equal(CommentRatings.Good, comment.Rating);
        }

        [Fact]
        public void RaingString_SetInvalidValue_ShouldThrowException()
        {
            //Arrange
            var comment = new Comment();
            
            //ACT
            Action act = () => comment.RatingString = "InvalidRating";
            
            //Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void Date_SetAndGet_ShouldUpdateDateTicks()
        {
            // Arrange

            var comment = new Comment();
            var expectedDate = new DateTime(2026, 5, 15);

            // Act
            comment.Date = expectedDate;

            // Assert
            Assert.Equal(expectedDate.Ticks, comment.DateTicks);
            Assert.Equal(expectedDate, comment.Date);
        }

    }
}