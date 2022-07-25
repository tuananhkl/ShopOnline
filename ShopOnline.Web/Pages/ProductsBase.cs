using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages;

public class ProductsBase : ComponentBase
{
    [Inject] public IShoppingCartService ShoppingCartService{ get; set; }
    [Inject] public IProductService ProductService { get; set; }
    
    [Inject] public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }
    [Inject] public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }
    protected IEnumerable<ProductDto> Products { get; set; }
    protected string ErrorMessage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await ClearLocalStorage();

            Products = await ManageProductsLocalStorageService.GetCollection();

            var shoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
            var totalQty = shoppingCartItems.Sum(i => i.Qty);

            ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
    {
        return Products.GroupBy(p => p.CategoryId)
            .OrderBy(pg => pg.Key);
    }

    protected string GetCategoryName(IGrouping<int, ProductDto> groupedProductDtos)
    {
        return groupedProductDtos.FirstOrDefault(p => p.CategoryId == groupedProductDtos.Key).CategoryName;
    }

    private async Task ClearLocalStorage()
    {
        await ManageProductsLocalStorageService.RemoveCollection();
        await ManageCartItemsLocalStorageService.RemoveCollection();
    }
}