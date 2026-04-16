using AutoMapper;
using DTO.StockTransactionDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfisUrunTakip.WebApi.Entity;
using Repositories.Contracts;
using System.Security.Claims;

namespace OfisUrunTakip.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockTransactionsController(IStockTransactionRepository stockRepository, IMapper mapper, IProductRepository productRepository, ILogger<ProductsController> logger) : ControllerBase
    {
        ILogger<ProductsController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Tüm stok işlemleri listeleniyor.");
            var stocks = await stockRepository.GetAllAsync("Supplier","User","Product");
            var dto = mapper.Map<IEnumerable<StockTransactionListDto>>(stocks);
            _logger.LogInformation("Toplam {Count} stok işlemleri getirildi.", dto.Count());
            return Ok(dto);
        }
        [HttpPost]
        
        public async Task<IActionResult> Create([FromBody] StockTransactionAddDto dto)
        {
            _logger.LogInformation("Stok işlemlerine ekleme isteği alındı. Kullanıcı: {User}, ÜrünId: {ProductId}, Miktar: {Quantity}, Fiyat: {UnitPrice}",
                User.Identity?.Name, dto.ProductId, dto.Quantity, dto.UnitPrice);

            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("ModelState geçersiz. Kullanıcı: {User}", User.Identity?.Name);
                    return BadRequest(ModelState);
                }

                if (dto.Quantity <= 0)
                {
                    _logger.LogWarning("Geçersiz miktar. Kullanıcı: {User}, Miktar: {Quantity}", User.Identity?.Name, dto.Quantity);
                    return BadRequest("Miktar 0'dan büyük olmalı");
                }

                var product = await productRepository.GetByIdAsync(dto.ProductId);
                if (product == null)
                {
                    _logger.LogWarning("Ürün bulunamadı. Kullanıcı: {User}, ÜrünId: {ProductId}", User.Identity?.Name, dto.ProductId);
                    return BadRequest("Ürün bulunamadı");
                }

                int stockEffect = (int)dto.Quantity * (int)dto.TransactionType;
                var total = dto.Quantity * dto.UnitPrice;

                if (product.CurrentStock + stockEffect < 0)
                {
                    _logger.LogWarning("Yetersiz stok. Kullanıcı: {User}, ÜrünId: {ProductId}, MevcutStok: {CurrentStock}, İstekMiktar: {Quantity}",
                        User.Identity?.Name, dto.ProductId, product.CurrentStock, dto.Quantity);
                    return BadRequest("Yetersiz stok");
                }

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                var stock = new StockTransaction
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    TransactionType = (OfisUrunTakip.WebApi.Entity.TransactionType)dto.TransactionType,
                    UnitPrice = dto.UnitPrice,
                    SupplierId = dto.SupplierId,
                    UserId = userId,
                    Totalcons = total,
                    TransactionDate = DateTime.Now,
                     Description = dto.Description
                };

                product.CurrentStock += stockEffect;

                await stockRepository.AddAsync(stock);
                await productRepository.UpdateAsync(product);

                _logger.LogInformation("Stok işlemi başarıyla eklendi. Kullanıcı: {User}, ÜrünId: {ProductId}, ToplamTutar: {Total}",
                    User.Identity?.Name, dto.ProductId, total);

                var transactions = await stockRepository.GetAllAsync();
                var list = mapper.Map<IEnumerable<StockTransactionListDto>>(transactions);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Stok ekleme sırasında hata oluştu. Kullanıcı: {User}, ÜrünId: {ProductId}",
                    User.Identity?.Name, dto.ProductId);
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Stok işlemleri silme isteği alındı. Id: {Id}", id);
            var data = await stockRepository.RemoveAsync(id);
            if (data == null) {
                _logger.LogWarning("Silinmek istenen stokişlemi bulunamadı. Id: {Id}", id); 
                return NotFound(); }
            _logger.LogInformation("Stok işlemi silindi. Id: {Id}", id);
            return Ok(data);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Stok işlemi getirme isteği alındı. ID: {Id}, Kullanıcı: {User}", id, User.Identity?.Name);

            var data = await stockRepository.GetByIdAsync(id);
            if (data == null)
            {
                _logger.LogWarning("Stok işlemi bulunamadı. ID: {Id}, Kullanıcı: {User}", id, User.Identity?.Name);
                return NotFound();
            }

            _logger.LogInformation("Stok işlemi başarıyla getirildi. ID: {Id}, Kullanıcı: {User}", id, User.Identity?.Name);
            return Ok(data);
        }


        [HttpGet("ByProduct/{productId}")]
        public async Task<IActionResult> GetByProductIdAsync(int productId)
        {
            _logger.LogInformation("Belirli ürüne ait stok işlemleri getirme isteği alındı. ÜrünId: {ProductId}, Kullanıcı: {User}", productId, User.Identity?.Name);

            var transactions = await stockRepository.GetByProductIdAsync(productId, "Supplier", "User", "Product");
            if (transactions == null || !transactions.Any())
            {
                _logger.LogWarning("Bu ürüne ait stok işlemi bulunamadı. ÜrünId: {ProductId}, Kullanıcı: {User}", productId, User.Identity?.Name);
                return NotFound("Bu Ürüne Ait Stok İşlemi Bulunamadı");
            }

            var list = mapper.Map<IEnumerable<StockTransactionListDto>>(transactions);
            _logger.LogInformation("{Count} adet stok işlemi başarıyla getirildi. ÜrünId: {ProductId}, Kullanıcı: {User}", list.Count(), productId, User.Identity?.Name);

            return Ok(list);
        }


    }
}
