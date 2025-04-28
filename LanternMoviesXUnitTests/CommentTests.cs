using System.Xml.Linq;
using Models;

namespace LanternMoviesXUnitTests
{
    public class CommentTests
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

            [Fact]
            public void Constructor_ShouldIntializeProperties()
            {
                // Arrange and ACT as well
                var comment = new Comment
                {
                    CommentorName = "Mark Hamil",
                    CommentText = "Good movie!",
                    Rating = CommentRatings.Good,
                    Date = DateTime.Now,
                    MovieID = Guid.NewGuid()
                };

                // Assert

                Assert.NotNull(comment.CommentorName);
                Assert.NotNull(comment.CommentText);
                Assert.NotEqual(Guid.Empty, comment.MovieID);
            }
        }

    }
}