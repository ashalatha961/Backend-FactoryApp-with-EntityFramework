using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PopsicleFactoryCo.Controllers;
using PopsicleFactoryCo.Data;
using PopsicleFactoryCo.Models;

namespace PopsicleFactoryCo.Tests
{
    public class PopsicleInventoryControllerTests
    {
        private readonly IValidator<PopsicleModel> _mockValidator;
        private readonly IPopsicleInventory _mockData;
        private readonly PopsicleInventoryController _controller;
        private readonly ILogger<PopsicleInventoryController> _mockLogger;

        // Constructor to initialize mocks and controller
        public PopsicleInventoryControllerTests()
        {
            // Using NSubstitute mocking library to create substitute objects (mocks) for dependencies
            _mockValidator = Substitute.For<IValidator<PopsicleModel>>();
            _mockData = Substitute.For<IPopsicleInventory>();
            _mockLogger = Substitute.For<ILogger<PopsicleInventoryController>>();
            _controller = new PopsicleInventoryController(_mockData, _mockValidator, _mockLogger);
        }

        [Fact]
        public async Task CreatePopsicle_WhenPopsicleDataIsValid()
        {
            // Arrange reference objects 
            var popsicle = new PopsicleModel 
            { 
                Id = 1, 
                Name = "New Test Popsicle", 
                Flavour = "cocoa", 
                Price = 10, 
                Quantity = 5 
            };
            var validationResult = new ValidationResult(); 

            // Setting up the mock validator to return a valid result
            _mockValidator.ValidateAsync(popsicle).Returns(new ValidationResult());
            _mockData.CreatePopsicleAsync(popsicle).Returns(popsicle);

            // Act means to perform the action being tested
            var result = await _controller.CreateNewPopsicle(popsicle);
            // Assert means to verify the result
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetPopsicleWithId", createdResult.ActionName);
            Assert.Equal(popsicle, createdResult.Value);
        }

        [Fact]
        public async Task CreatePopsicle_WhenPopsicleDataIsInValid()
        {
            // Arrange
            var popsicle = new PopsicleModel
            {
                Id = 0,
                Name = "",
                Flavour = "",
                Price = 0,
                Quantity = 0
            };
            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name is required"),
                new ValidationFailure("Flavour", "Flavour is required"),
                new ValidationFailure("Price", "Price must be greater than zero"),
                new ValidationFailure("quantity", "Quantity must be greater than zero")
            });
            _mockData.CreatePopsicleAsync(popsicle).Returns(popsicle);
            _mockValidator.ValidateAsync(popsicle).Returns(validationResult);

            // Act
            var result = await _controller.CreateNewPopsicle(popsicle);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(validationResult.Errors, badRequestResult.Value);
        }

        [Fact]
        public async Task GetListOfAllPopsicles_WithValidData()
        {
            // Arrange
            var popsicles = new List<PopsicleModel>
            {
                new PopsicleModel { Id = 1, Name = "Popsicle 1", Flavour = "Chocolate", Price = 10, Quantity = 5 },
                new PopsicleModel { Id = 2, Name = "Popsicle 2", Flavour = "Vanilla", Price = 15, Quantity = 10 },
                new PopsicleModel { Id = 3, Name = "Popsicle 3", Flavour = "Strawberry", Price = 20, Quantity = 15 }
            };
            // Setting up the mock data repository to return the list of popsicles
            _mockData.GetListOfAllPopsicles().Returns(popsicles);

            // Act
            var result = await _controller.GetListOfAllPopsicles();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(popsicles, okResult.Value);
        }

        [Fact]
        public async Task GetPopsicleWithId_WhenPopsicleIdExists()
        {
            // Arrange
            var popsicle = new PopsicleModel 
            { 
                Id = 1, 
                Name = "Test Popsicle", 
                Flavour = "Chocolate", 
                Price = 10, 
                Quantity = 5 
            };
            _mockData.GetPopsicleByIdAsync(1).Returns(popsicle);

            // Act
            var result = await _controller.GetPopsicleWithId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(popsicle, okResult.Value);
        }

        [Fact]
        public async Task GetPopsicleWithId_WhenPopsicleIdDoesNotExist()
        {
            // Arrange
            _mockData.GetPopsicleByIdAsync(777).Returns((PopsicleModel?)null);
            // Act
            var result = await _controller.GetPopsicleWithId(777);
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task SearchPopsicleInInventory_WhenSearchItemIsValid()
        {
            // Arrange
            var popsicles = new List<PopsicleModel>
            {
                new PopsicleModel { Id = 1, Name = "Chocolate Popsicle", Flavour = "Chocolate", Price = 15, Quantity = 5 },
                new PopsicleModel { Id = 2, Name = "Blueberry Popsicle", Flavour = "Blueberry", Price = 10, Quantity = 10 },
                new PopsicleModel { Id = 3, Name = "Strawberry Popsicle", Flavour = "Strawberry", Price = 15, Quantity = 15 },
                new PopsicleModel { Id = 4, Name = "Orange Popsicle", Flavour = "orange", Price = 20, Quantity = 20 }
            };
            _mockData.SearchForPopsiclesAsync("Popsicle").Returns(popsicles);
            // Act
            var result = await _controller.SearchForPopsicles("Popsicle");
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(popsicles, okResult.Value);
        }

        [Fact]
        public async Task SearchPopsicleInInventory_WhenSearchItemIsInvalid()
        {
            // Arrange - Setup is not required to test this scenario. So not creating any mock data.
            _mockData.SearchForPopsiclesAsync("NonExistentPopsicle").Returns(new List<PopsicleModel>());
            // Act
            var result = await _controller.SearchForPopsicles("NonExistentPopsicle");
            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task SearchPopsicleInInventory_WhenSearchItemIsNull()
        {
            // Arrange - Setup is not required
            _mockData.SearchForPopsiclesAsync(null).Returns(new List<PopsicleModel>());
            // Act
            var result = await _controller.SearchForPopsicles(null);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Search item cannot be null or empty", badRequestResult.Value);
        }

        [Fact]
        public async Task SearchPopsicleInInventory_WhenSearchItemIsEmptyl()
        {
            // Arrange - Setup is not required
            _mockData.SearchForPopsiclesAsync("").Returns(new List<PopsicleModel>());
            // Act
            var result = await _controller.SearchForPopsicles("");
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Search item cannot be null or empty", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdatePopsicle_WhenPopsicleDataRequestIsValid()
        {
            // Arrange
            var popsicle = new PopsicleModel 
            { 
                Id = 1, 
                Name = "Updated Popsicle", 
                Flavour = "Mint", 
                Price = 12, 
                Quantity = 6 
            };
            var existingPopsicle = new PopsicleModel 
            { 
                Id = 1, 
                Name = "Old Popsicle", 
                Flavour = "Chocolate", 
                Price = 10, 
                Quantity = 5 
            };
            var validationResult = new ValidationResult();

            _mockValidator.ValidateAsync(popsicle).Returns(new ValidationResult());
            _mockData.UpdatePopsicleAsync(popsicle).Returns(popsicle);
            _mockData.GetPopsicleByIdAsync(1).Returns(existingPopsicle);

            // Act
            var result = await _controller.UpdatePopsicle(1,popsicle);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(popsicle, okResult.Value);
        }

        [Fact]
        public async Task UpdatePopsicle_WhenPopsicleDataRequestIsInvalid()
        {
            // Arrange
            var popsicle = new PopsicleModel 
            { 
                Id = 0, 
                Name = "Update Popsicle", 
                Flavour = "New Flavour", 
                Price = 0, 
                Quantity = 0 
            };

            // Act
            var result = await _controller.UpdatePopsicle(1,popsicle);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid popsicle data", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdatePopsicle_WhenPopsicleIdDoesNotExist()
        {
            // Arrange
            var popsicle = new PopsicleModel 
            { 
                Id = 777, 
                Name = "Non-existent Popsicle In Inventory", 
                Flavour = "Unknown", 
                Price = 0, 
                Quantity = 0 
            };
            var validationResult = new ValidationResult();
            _mockValidator.ValidateAsync(popsicle).Returns(new ValidationResult());
            _mockData.GetPopsicleByIdAsync(777).Returns((PopsicleModel?)null);

            // Act
            var result = await _controller.UpdatePopsicle(777,popsicle);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeletePopsicle_WhenPopsicleIdExists()
        {
            // Arrange
            _mockData.DeletePopsicleAsync(1).Returns(true);
            // Act
            var result = await _controller.DeletePopsicle(1);
            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeletePopsicle_WhenPopsicleIdDoesNotExist()
        {
            // Arrange
            _mockData.DeletePopsicleAsync(999).Returns(false);
            // Act
            var result = await _controller.DeletePopsicle(999);
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}

/*
xUnit avoids attributes like [TestClass] and [TestMethod] that are used in other testing frameworks. Instead, it uses conventions to identify test classes 
and test methods. In xUnit, any class that contains public methods decorated with the [Fact] attribute is considered a test class, and any method decorated 
with the [Fact] attribute is considered a test method.
This approach allows for a more flexible and less verbose way of defining tests compared to traditional attribute-based frameworks.
In the provided code snippet, the test class PopsicleInventoryControllerTests contains multiple test methods, each decorated with the [Fact] attribute,
indicating that they are individual test cases to be executed by the xUnit testing framework.
The [Fact] attribute is used to denote that a method is a test method that should be executed by the test runner. 
Each test method represents a specific scenario or behavior that is being tested in the PopsicleInventoryController.
xunit creates a new instance of the test class for each test method, ensuring that tests are isolated and do not interfere with each other.
*/