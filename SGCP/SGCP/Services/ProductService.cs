using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using SGCP.DTOs;
using SGCP.DTOs.LaborCost;
using SGCP.DTOs.Product;
using SGCP.DTOs.ProductComponent;
using SGCP.Models;
using SGCP.Services.Logger;

namespace SGCP.Services
{
    public class ProductService : IProductService
    {
        private readonly SGCP_DbContext _context;
        private readonly ILoggingService<ProductService> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public ProductService(SGCP_DbContext context, ILoggingService<ProductService> logger, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
        }

        //public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        //{
        //    var products = await _context.Products.Where(p => p.Enable).ToListAsync();
        //    return _mapper.Map<IEnumerable<ProductDto>>(products);
        //}
        public async Task<PagedResult<ProductDto>> GetPagedProductsAsync(PaginationQueryDto query)
        {
            var productsQuery = _context.Products
                .Where(p =>
                    (!query.Enable.HasValue || p.Enable == query.Enable.Value) &&
                    (string.IsNullOrEmpty(query.Search) || p.Name.Contains(query.Search))
                )
                .OrderBy(p => p.Name);

            var totalItems = await productsQuery.CountAsync();

            var products = await productsQuery
                .Skip(query.PageIndex * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var items = _mapper.Map<IEnumerable<ProductDto>>(products);

            return new PagedResult<ProductDto>
            {
                TotalItems = totalItems,
                Items = items
            };
        }


        public async Task<ProductDto> GetByIdAsync(long id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && p.Enable);
            if (product is null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateAsync(ProductCreateDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> UpdateAsync(long id, ProductUpdateDto productDto)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct is null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            _mapper.Map(productDto, existingProduct);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var existingProduct = await _context.Products
                .Include(p => p.LaborCosts)
                .Include(p => p.ProductComponents)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct is null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            // Eliminar relaciones primero
            _context.LaborCosts.RemoveRange(existingProduct.LaborCosts);
            _context.ProductComponents.RemoveRange(existingProduct.ProductComponents);

            // Luego el producto
            _context.Products.Remove(existingProduct);

            await _context.SaveChangesAsync();

            _logger.Info($"Deleting product {id} with its components and labor costs.");

            return true;
        }


        public async Task<ProductBuilderDto> GetBuilderAsync(long id)
        {
            var product = await _context.Products
                .Include(p => p.ProductComponents)
                    .ThenInclude(pc => pc.Component)
                .Include(p => p.LaborCosts)
                    .ThenInclude(l => l.LaborType) // 👈 esto faltaba
                .FirstOrDefaultAsync(p => p.Id == id && p.Enable);

            if (product == null)
                throw new InvalidOperationException("Producto no encontrado");

            // (Opcional) Filtrar solo los componentes y costos habilitados
            product.ProductComponents = product.ProductComponents
                .Where(pc => pc.Enable && pc.Component.Enable)
                .ToList();

            product.LaborCosts = product.LaborCosts
                .Where(l => l.Enable && l.LaborType.Enable)
                .ToList();

            return _mapper.Map<ProductBuilderDto>(product);
        }

        public async Task<ProductBuilderDto> SaveBuilderAsync(ProductBuilderDto builderDto)
        {
            _logger.Info(nameof(SaveBuilderAsync), $"Saving product builder for Id={builderDto.ProductId}");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                Product product;

                if (builderDto.ProductId == 0)
                {
                    product = _mapper.Map<Product>(builderDto);
                    product.InsertDate = DateTime.UtcNow;
                    product.Enable = true;
                    _context.Products.Add(product);
                }
                else
                {
                    product = await _context.Products
                        .Include(p => p.ProductComponents)
                        .Include(p => p.LaborCosts)
                        .FirstOrDefaultAsync(p => p.Id == builderDto.ProductId);

                    if (product == null)
                        throw new Exception("Product not found");

                    _mapper.Map(builderDto, product);
                    product.UpdateDate = DateTime.UtcNow;

                    _context.ProductComponents.RemoveRange(product.ProductComponents);
                    _context.LaborCosts.RemoveRange(product.LaborCosts);
                }

                await _context.SaveChangesAsync();

                // Agregar nuevos componentes
                var newComponents = builderDto.Components.Select(c => new ProductComponent
                {
                    ProductId = product.Id,
                    ComponentId = c.ComponentId,
                    Quantity = c.Quantity,
                    UnitId = c.UnitId,
                    Enable = true,
                    InsertDate = DateTime.UtcNow
                }).ToList();
                _context.ProductComponents.AddRange(newComponents);

                // Agregar nuevos costos de mano de obra (según estructura real de tu tabla)
                var newLaborCosts = builderDto.LaborCosts.Select(l => new LaborCost
                {
                    ProductId = product.Id,
                    LaborTypeId = l.LaborTypeId,
                    UnitId = l.UnitId,
                    Quantity = l.Quantity,
                    InsertDate = DateTime.UtcNow,
                    Enable = true
                }).ToList();
                _context.LaborCosts.AddRange(newLaborCosts);

                await _context.SaveChangesAsync();

                // Calcular el precio final
                var componentIds = builderDto.Components.Select(c => c.ComponentId).ToList();
                var componentPrices = await _context.Components
                    .Where(c => componentIds.Contains(c.Id))
                    .ToDictionaryAsync(c => c.Id, c => c.UnitCost);

                var totalComponentCost = builderDto.Components.Sum(c =>
                    c.Quantity * (componentPrices.TryGetValue(c.ComponentId, out var price) ? price : 0));

                var laborTypeIds = builderDto.LaborCosts.Select(l => l.LaborTypeId).ToList();
                var laborRates = await _context.LaborTypes
                    .Where(l => laborTypeIds.Contains(l.Id))
                    .ToDictionaryAsync(l => l.Id, l => l.HourlyCost); // Usás HourlyCost como precio por unidad

                var totalLaborCost = builderDto.LaborCosts.Sum(l =>
                    l.Quantity * (laborRates.TryGetValue(l.LaborTypeId, out var cost) ? cost : 0));

                product.FinalPrice = totalComponentCost + totalLaborCost;

                // Calcular y guardar precios sugeridos
                var calculatedPrice = await CalculateAsync(product.Id);

                var existingPrice = await _context.ProductPrices
                    .FirstOrDefaultAsync(p => p.ProductId == product.Id);

                if (existingPrice == null)
                {
                    _context.ProductPrices.Add(calculatedPrice);
                }
                else
                {
                    existingPrice.Cost = calculatedPrice.Cost;
                    existingPrice.WholesaleSuggestedPrice = calculatedPrice.WholesaleSuggestedPrice;
                    existingPrice.RetailSuggestedPrice = calculatedPrice.RetailSuggestedPrice;
                    existingPrice.UpdateDate = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Preparar salida
                var resultDto = _mapper.Map<ProductBuilderDto>(product);
                resultDto.Components = builderDto.Components;
                resultDto.LaborCosts = builderDto.LaborCosts;

                return resultDto;
            }
            catch (Exception ex)
            {
                try
                {
                    if (transaction.GetDbTransaction().Connection != null)
                    {
                        await transaction.RollbackAsync();
                    }
                }
                catch (Exception rollbackEx)
                {
                    _logger.Error(nameof(SaveBuilderAsync), $"Error al hacer rollback: {rollbackEx.Message}", rollbackEx);
                }

                _logger.Error(nameof(SaveBuilderAsync), ex.Message, ex);
                throw;
            }
        }

        public async Task<bool> SetEnableStatusAsync(long id, bool enable)
        {
            var product = await _context.Products
                .Include(p => p.LaborCosts)
                .Include(p => p.ProductComponents)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            product.Enable = enable;
            product.UpdateDate = DateTime.UtcNow;

            foreach (var labor in product.LaborCosts)
            {
                labor.Enable = enable;
                labor.UpdateDate = DateTime.UtcNow;
            }

            foreach (var component in product.ProductComponents)
            {
                component.Enable = enable;
                component.UpdateDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ProductPrice> CalculateAsync(long productId)
        {
            var product = await _context.Products
                .Include(p => p.ProductComponents)
                .ThenInclude(pc => pc.Component)
                .Include(p => p.LaborCosts)
                .ThenInclude(lc => lc.LaborType)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                throw new Exception("Product not found");

            // 1. Calcular costo de componentes
            var componentCost = product.ProductComponents.Sum(pc =>
                (pc.Quantity * (pc.Component?.UnitCost ?? 0)));

            // 2. Calcular costo de mano de obra
            var laborCost = product.LaborCosts.Sum(lc =>
                (lc.Quantity * (lc.LaborType?.HourlyCost ?? 0)));

            var totalCost = componentCost + laborCost;

            // 3. Obtener márgenes desde configuración
            var wholesaleMarkup = _configuration.GetValue<decimal>("Pricing:WholesaleMarkup", 100m);
            var retailMarkup = _configuration.GetValue<decimal>("Pricing:RetailMarkup", 50m);

            var wholesaleSuggested = totalCost * (1 + wholesaleMarkup / 100);
            var retailSuggested = wholesaleSuggested * (1 + retailMarkup / 100);

            var price = new ProductPrice
            {
                ProductId = productId,
                Cost = Math.Round(totalCost, 2),
                WholesaleSuggestedPrice = Math.Round(wholesaleSuggested, 2),
                RetailSuggestedPrice = Math.Round(retailSuggested, 2),
                InsertDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                Enable = true
            };

            return price;
        }


        public async Task<ProductPriceDto> RecalculatePricesAsync(long productId)
        {
            var price = await CalculateAsync(productId);

            var existing = await _context.ProductPrices
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (existing == null)
            {
                _context.ProductPrices.Add(price);
            }
            else
            {
                existing.Cost = price.Cost;
                existing.WholesaleSuggestedPrice = price.WholesaleSuggestedPrice;
                existing.RetailSuggestedPrice = price.RetailSuggestedPrice;
                existing.UpdateDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return new ProductPriceDto
            {
                ProductId = price.ProductId,
                Cost = price.Cost,
                WholesaleSuggestedPrice = price.WholesaleSuggestedPrice,
                WholesaleRealPrice = existing?.WholesaleRealPrice ?? 0,
                RetailSuggestedPrice = price.RetailSuggestedPrice,
                RetailRealPrice = existing?.RetailRealPrice ?? 0
            };
        }

    }
}
