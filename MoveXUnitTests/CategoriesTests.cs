using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Magic_lantern_movies;
using Models;
using ViewModels;

namespace MoveXUnitTests
{
    

    public class CategoriesViewModelTests
    {
        private readonly DatabaseContext _mockDb;

        public CategoriesViewModelTests()
        {
            _mockDb = new DatabaseContext("mockDbPath");
        }

        [Fact]
        public async Task InitializeAsync_ShouldLoadCategories()
        {
            // Arrange
            var viewModel = new CategoriesViewModel(_mockDb);

            // Act
            await viewModel.InitializeAsync();



            // Assert
            Assert.NotEmpty(viewModel.Categories);
            Assert.Contains("Action", viewModel.Categories); 
        }

        [Fact]
        public void UpdateSpan_ShouldSetColumnSpanBasedOnWidth()
        {
            // Arrange
            var viewModel = new CategoriesViewModel(_mockDb);

            // ACT
            viewModel.UpdateSpan(800);

            // Assert
            Assert.Equal(2, viewModel.ColumnSpan);

            // ACT
            viewModel.UpdateSpan(500); 

            // Assert
            Assert.Equal(1, viewModel.ColumnSpan);
        }


        [Fact]
        public void Categories_ShouldBeEmptyInitially()
        {
            // Arrange
            var viewModel = new CategoriesViewModel(_mockDb);

            // Assert
            Assert.Empty(viewModel.Categories);
        }
    }
}
