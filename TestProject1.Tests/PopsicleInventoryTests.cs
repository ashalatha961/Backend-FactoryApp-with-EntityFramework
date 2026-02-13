using PopsicleFactoryCo.Data;
using PopsicleFactoryCo.Models;

namespace PopsicleFactoryCo.Tests
{
    public class PopsicleInventoryTests
    {

        [Fact]
        public async Task CreatePopsicle_WhenValidDataIsProvided()
        {
            // Arrange
            var popsicleInventory = new PopsicleInventory();
            var newPopsicle = new PopsicleModel
            {
                Id = 6,
                Name = "Test Popsicle",
                Flavour = "Test Flavour",
                Price = 11,
                Quantity = 9
            };
            // Act
            var createdPopsicle = await popsicleInventory.CreatePopsicleAsync(newPopsicle);
            // Assert
            Assert.NotNull(createdPopsicle);
            Assert.Equal(newPopsicle.Name, createdPopsicle.Name);
        }

        [Fact]
        public async Task CreatePopsicle_WhenIdProvidedAlreadyExists()
        {
            // Arrange
            var popsicleInventory = new PopsicleInventory();
            var idAlreadyExists = new PopsicleModel
            {
                Id = 1,
                Name = "Invalid Name",
                Flavour = "Invalid Flavour",
                Price = 10, 
                Quantity = 0
            };
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => popsicleInventory.CreatePopsicleAsync(idAlreadyExists));

        }

        [Fact]
        public async Task GetListOfAllPopsiclesPresent()
        {
            // Arrange
            var popsicleInventory = new PopsicleInventory();
            // Act
            var popsicles = await popsicleInventory.GetListOfAllPopsicles();
            // Assert
            Assert.NotNull(popsicles);
            Assert.NotEmpty(popsicles);
        }

        [Fact]
        public async Task GetPopsicleById_FromExistingInventory()
        {
            // Arrange
            var popsicleInventory = new PopsicleInventory();
            int popsicleId = 1;

            // Act
            var popsicle = await popsicleInventory.GetPopsicleByIdAsync(popsicleId);

            // Assert
            Assert.NotNull(popsicle);
            Assert.Equal(popsicleId, popsicle.Id);
        }

        [Fact]
        public async Task GetPopsicleById_WhenPopsicleIdDoesNotExist()
        {
            // Arrange
            var popsicleInventory = new PopsicleInventory();
            int nonExistentPopsiscleId = 999;
            // Act
            var popsicle = await popsicleInventory.GetPopsicleByIdAsync(nonExistentPopsiscleId);
            // Assert
            Assert.Null(popsicle);
        }

        [Fact]
        public async Task SearchPopsicles_WhenSearchItemMatchesWithExistingInventory()
        {
            // Arrange
            var popsicleInventory = new PopsicleInventory();
            string searchItem = "Chocolate";
            // Act
            var result = await popsicleInventory.SearchForPopsiclesAsync(searchItem);
            // Assert
            var resultList = result.ToList();
            Assert.Single(resultList);
            Assert.Equal("Chocolate", resultList[0].Flavour);
        }

        [Fact]
        public async Task SearchPopsicles_WhenSearchItemDoesNotMatchWithExistingInventory()
        {
            // Arrange
            var popsicleInventory = new PopsicleInventory();
            string searchItem = "unknownFlavour";
            // Act
            var result = await popsicleInventory.SearchForPopsiclesAsync(searchItem);
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task UpdatePopsicle_WhenPopsicleExists()
        {
            // Arrange
            var popsicleInventory = new PopsicleInventory();
            var updatedPopsicle = new PopsicleModel
            {
                Id = 1,
                Name = "Updated New Popsicle",
                Flavour = "Updated new Flavour",
                Price = 11,
                Quantity = 9
            };
            // Act
            var result = await popsicleInventory.UpdatePopsicleAsync(updatedPopsicle);
            // Assert
            Assert.Equal(updatedPopsicle.Name, result.Name);
            Assert.Equal(updatedPopsicle.Flavour, result.Flavour);
            Assert.Equal(updatedPopsicle.Price, result.Price);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task DeletePopsicle_WhenPopsicleExists()
        {
            // Arrange
            var popsicleInventory = new PopsicleInventory();
            int popsicleId = 1;
            // Act
            var result = await popsicleInventory.DeletePopsicleAsync(popsicleId);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeletePopsicle_WhenPopsicleDoesNotExist()
        {
            // Arrange
            var popsicleInventory = new PopsicleInventory();
            int nonExistentPopsicleId = 9;
            // Act
            var result = await popsicleInventory.DeletePopsicleAsync(nonExistentPopsicleId);
            // Assert
            Assert.False(result);
        }
    }
}