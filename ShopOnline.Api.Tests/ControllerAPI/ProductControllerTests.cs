using FakeItEasy;
using ShopOnline.Api.Repositories;
using ShopOnline.Api.Repositories.Contracts;

namespace ShopOnline.Api.Tests.ControllerAPI;

public class ProductControllerTests
{
    private readonly IProductRepository _productRepository;

    public ProductControllerTests()
    {
        _productRepository = A.Fake<IProductRepository>();
    }
}