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
    public class StockTransactionsController : ControllerBase
    {
        private readonly IStockTransactionRepository _stockRepository;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        public StockTransactionsController(IStockTransactionRepository stockRepository, IMapper mapper, IProductRepository productRepository)

        {
            _mapper = mapper;
            _stockRepository = stockRepository;
            _productRepository = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var stocks = await _stockRepository.GetAllAsync("Supplier","User","Product");
            var dto = _mapper.Map<IEnumerable<StockTransactionListDto>>(stocks);
            return Ok(dto);
        }
        [HttpPost]
        //[Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Create([FromBody] StockTransactionAddDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (dto.Quantity <= 0)
                    return BadRequest("Miktar 0'dan büyük olmalı");

                var product = await _productRepository.GetByIdAsync(dto.ProductId);
                if (product == null)
                    return BadRequest("Ürün bulunamadı");

                int stockEffect = dto.Quantity *(int)dto.TransactionType;

                if (product.CurrentStock + stockEffect < 0)
                    return BadRequest("Yetersiz stok");
                var userId = int.Parse(
                    User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                var stock = new StockTransaction
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    TransactionType = (OfisUrunTakip.WebApi.Entity.TransactionType)dto.TransactionType,
                    UnitPrice = dto.UnitPrice,
                    SupplierId = dto.SupplierId,
                    UserId = userId,
                    TransactionDate = DateTime.Now,
                   // Description = dto.Description
                };

                product.CurrentStock += stockEffect;
                await _stockRepository.AddAsync(stock);
                await _productRepository.UpdateAsync(product);
                var transactions = await _stockRepository.GetAllAsync();
                var list = _mapper.Map<IEnumerable<StockTransactionListDto>>(transactions);
                return Ok(list);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, ex.Message);
            }

        }

        [HttpDelete]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _stockRepository.RemoveAsync(id);
            if (data == null) { return NotFound(); }
            return Ok(data);
        }
        
        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _stockRepository.GetByIdAsync(id);
            if (data == null) { return NotFound(); }
            return Ok(data);
        }
        [HttpGet("ByProduct/{productId}")]
        public async Task<IActionResult> GetByProductIdAsync(int productId)
        {
            var transactions=await _stockRepository.GetByProductIdAsync(productId, "Supplier", "User", "Product");
            if(transactions == null) { return NotFound("Bu Ürüne Ait Stok İşlemi Bulunamadı");
               
            }
            var list = _mapper.Map<IEnumerable<StockTransactionListDto>>(transactions);
            return Ok(list);
        }
       
    }
}
