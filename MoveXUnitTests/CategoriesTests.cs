using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic_lantern_movies;
using Models;

namespace MoveXUnitTests
{
    public class CategoriesTests
    {
        [Fact]
        public void AddCategory_ShouldAddcategoryToList()
        {
            // Arrangee

            var categories = new List<string>();
            var categoryToAdd = "Sports";

            // ACT

            categories.Add(categoryToAdd);

            // Assert

            Assert.Contains(categoryToAdd, categories);
        }

        [Fact]
        public void RemoveCategory_ShouldremoveCategoryFromList()
        {
            // Arrange

            var categories = new List<string> { "Sports", "Music", "Movies" };
            var categoryToRemove = "Music";

            // Acct

            categories.Remove(categoryToRemove);

            // Asserty

            Assert.DoesNotContain(categoryToRemove, categories);
        }


        [Fact]
        public void ClearCategories_ShouldemptyThelist()
        {
            // Arrange

            var categories = new List<string> { "Sports", "Music", "Movies" };

            // ACT

            categories.Clear();

            // Assert
            Assert.Empty(categories);
        }
    }
}
