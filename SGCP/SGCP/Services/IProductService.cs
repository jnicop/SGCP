using SGCP.DTOs;
using SGCP.DTOs.Product;
using SGCP.Models;
using System.Threading.Tasks;

namespace SGCP.Services
{
    public interface IProductService
    {
        //Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<PagedResult<ProductDto>> GetPagedProductsAsync(PaginationQueryDto paginationQueryDto );
        Task<ProductDto> GetByIdAsync(long id);
        Task<ProductDto> CreateAsync(ProductCreateDto product);
        Task<bool> UpdateAsync(long id, ProductUpdateDto product);
        Task<bool> DeleteAsync(long id);
        Task<ProductPrice> CalculateAsync(long productId);
        Task<ProductPriceDto> RecalculatePricesAsync(long productId);
        Task<ProductBuilderDto> GetBuilderAsync(long id);
        Task<ProductBuilderDto> SaveBuilderAsync(ProductBuilderDto dto);

        Task<bool> SetEnableStatusAsync(long id, bool enable);
    }
}
    