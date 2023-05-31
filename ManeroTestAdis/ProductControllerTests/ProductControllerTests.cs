using FakeItEasy;
using Manero_Backend.Controllers;
using Manero_Backend.Models.Interfaces.Repositories;
using Manero_Backend.Models.Interfaces.Services;
using Manero_Backend.Models.Schemas.Product;
using Microsoft.AspNetCore.Mvc;

namespace Manero_Backend.Tests.Controllers
{
    public class ProductControllerTests
    {
        [Fact]
        public async Task CreateAsync_WithValidProductSchema_ReturnsOkResult()
        {
            // Arrange
            var productService = A.Fake<IProductService>();
            var productRepository = A.Fake<IProductRepository>();
            var controller = new ProductController(productService, productRepository);

            var productSchema = new ProductSchema
            {
                Name = "TestProduct",
                Description = "TestProduct Description",
                Price = 99.99m,
                ImageUrl = "https://example.com/image.jpg",
                CategoryId = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                TagIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
                ColorIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
                SizeIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };

            // Act
            var result = await controller.CreateAsync(productSchema);

            // Assert
            NUnit.Framework.Assert.IsInstanceOf<OkResult>(result);
        }


    }
}
