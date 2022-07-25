using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories;
using ShopOnline.Models.Dtos;
using Xunit;

namespace ShopOnline.Api.Tests.Repository;

public class ProductRepositoryTests
{
    private async Task<ShopOnlineDbContext> GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ShopOnlineDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var databaseContext = new ShopOnlineDbContext(options);
        await databaseContext.Database.EnsureCreatedAsync();

        if (await databaseContext.Products.CountAsync() < 0)
        {
            databaseContext.Products.Add(
                new Product
                {
                    Id = 1,
                    Name = "Glossier - Beauty Kit",
                    Description = "A kit provided by Glossier, containing skin care, hair care and makeup products",
                    ImageURL = "/Images/Beauty/Beauty1.png",
                    Price = 100,
                    Qty = 100,
                    CategoryId = 1
                });
            databaseContext.Products.Add(
                new Product
                {
                    Id = 2,
                    Name = "Curology - Skin Care Kit",
                    Description = "A kit provided by Curology, containing skin care products",
                    ImageURL = "/Images/Beauty/Beauty2.png",
                    Price = 50,
                    Qty = 45,
                    CategoryId = 1
                });
            databaseContext.ProductCategories.Add(new ProductCategory
            {
                Id = 1,
                Name = "Beauty",
                IconCSS = "fas fa-spa"
            });

            await databaseContext.SaveChangesAsync();
        }

        return databaseContext;
    }

    [Fact]
    public async void ProductRepository_GetItem_ReturnProduct()
    {
        //Arrange
        int id = 1;
        var expected = new ProductDto
        {
            Id = 1,
            Name = "Glossier - Beauty Kit",
            Description = "A kit provided by Glossier, containing skin care, hair care and makeup products",
            ImageURL = "/Images/Beauty/Beauty1.png",
            Price = 100,
            Qty = 100,
            CategoryId = 1,
            CategoryName = "Beauty"
        };
        
        var dbContext = await GetDbContext();
        var productRepository = new ProductRepository(dbContext);

        //Act
        var product = await productRepository.GetItem(id);
        var actual = product.ConvertToDto();

        //Assert
        actual.Should().BeOfType<ProductDto>();
        actual.Should().NotBeNull();
        actual.Should().BeEquivalentTo(expected);

    }
}