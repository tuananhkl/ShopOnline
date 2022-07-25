using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Controllers;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;
using Xunit;

namespace ShopOnline.Api.Tests.Controller;

public class ProductControllerTests
{
    private readonly IProductRepository _productRepository;
    private readonly ProductController _productController;

    public ProductControllerTests()
    {
        //Dependencies
        _productRepository = A.Fake<IProductRepository>();
        
        //SUT
        _productController = new ProductController(_productRepository);
    }

    [Fact]
    public void ProductController_GetItems_ReturnsSuccess()
    {
        //Arrange - What do I need to bring in
        var products = A.Fake<IEnumerable<Product>>();
        A.CallTo(() => _productRepository.GetItems()).Returns(products);

        //Act
        var result = _productController.GetItems();

        //Assert
        result.Should().BeOfType<Task<ActionResult<IEnumerable<ProductDto>>>>();
    }

    [Fact]
    public void ProductController_GetItem_ReturnsSuccess()
    {
        //Arrange
        int id = 1;
        var product = A.Fake<Product>();
        A.CallTo(() => _productRepository.GetItem(id)).Returns(product);

        //Act
        var result = _productController.GetItem(id);

        //Assert
        result.Should().BeOfType<Task<ActionResult<ProductDto>>>();
    }
}