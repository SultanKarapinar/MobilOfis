using AutoMapper;
using DTO.CategoryDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfisUrunTakip.WebApi.Entity;
using Repositories.Contracts;

namespace OfisUrunTakip.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryRepository categoryRepository, IMapper mapper, ILogger<CategoryController> logger) : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CategoryController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Tüm kategoriler listeleniyor.");

            var categories = await _categoryRepository.GetAllAsync();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryListDto>>(categories);

            _logger.LogInformation("Toplam {Count} kategori getirildi.", categoryDtos.Count());

            return Ok(categoryDtos);
        }

        [HttpPost]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Create([FromBody] CategoryAddDto dto)
        {
            _logger.LogInformation("Kategori ekleme isteği alındı. Kullanıcı: {User}", User.Identity?.Name);

            if (await _categoryRepository.ExistsAsync(x => x.Name == dto.Name)) {
                _logger.LogWarning("Aynı isimde kategori mevcut: {CategoryName}", dto.Name);
                return BadRequest("Bu kategori zaten mevcut!");
            }

                
            if (dto == null)
            {
                _logger.LogWarning("Kategori ekleme isteği null dto ile geldi.");
                return BadRequest();
            }
            var categories = _mapper.Map<Category>(dto);
            await _categoryRepository.AddAsync(categories);
            _logger.LogInformation("Yeni kategori eklendi. Id: {Id}, Name: {Name}", categories.Id, categories.Name);
            return Ok(categories);


        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Kategori silme isteği alındı. Id: {Id}, Kullanıcı: {User}", id, User.Identity?.Name);
            var data = await _categoryRepository.GetByIdAsync(id);
            if (data == null)
            {
                _logger.LogWarning("Silinmek istenen kategori bulunamadı. Id: {Id}", id);
                return NotFound();
            }

            // Kategoriye bağlı ürün var mı kontrol et
            var hasProducts = await _categoryRepository.HasProductsAsync(id); 
            if (hasProducts) {
                _logger.LogWarning("Kategori silinemedi. Id: {Id} - Bağlı ürünler mevcut.", id);
                return BadRequest("Bu kategoriye bağlı ürünler var, silinemez!");
            }
               

            // Silme işlemi
            var deleted = await _categoryRepository.RemoveAsync(id);
            _logger.LogInformation("Kategori başarıyla silindi. Id: {Id}", id);
            return Ok(deleted);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Asistan")]

        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDto dto)
        {
            _logger.LogInformation("Kategori güncelleme isteği alındı. Id: {Id}", id);
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                _logger.LogWarning("Güncellenmek istenen kategori bulunamadı. Id: {Id}", id);
                return NotFound(); }

            _mapper.Map(dto, category);

            var updatedCategory = await _categoryRepository.UpdateAsync(category);
            _logger.LogInformation("Kategori güncellendi. Id: {Id}", id);
            return Ok(updatedCategory);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            _logger.LogInformation("Kategori detayı istendi. Id: {Id}", id);
            var data = await _categoryRepository.GetByIdAsync(id);
            if (data == null) {
                _logger.LogWarning("Kategori bulunamadı. Id: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Kategori detayı geldi. Id: {Id}", id);
            return Ok(data);

        }
        [HttpGet("{categoryId}/products")]
        public IActionResult GetProductsByCategory(int categoryId)
        {
            _logger.LogInformation("Kategoriye ait ürünler getiriliyor. CategoryId: {CategoryId}", categoryId);

            var products = _categoryRepository.GetProductsByCategoryId(categoryId);
            return Ok(products);

        }

    }
}

