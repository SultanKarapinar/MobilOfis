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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]

        public async Task<IActionResult> Get()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryListDto>>(categories);
            return Ok(categoryDtos);
        }
        [HttpPost]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Create([FromBody] CategoryAddDto dto)
        {
            if (await _categoryRepository.ExistsAsync(x => x.Name == dto.Name))
                return BadRequest("Bu kategori zaten mevcut!");
            if (dto == null)
            {
                return BadRequest();
            }
            var categories = _mapper.Map<Category>(dto);
            await _categoryRepository.AddAsync(categories);
            return Ok(categories);


        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _categoryRepository.RemoveAsync(id);
            if (data == null)
            {
                return NotFound();
            }

            // Kategoriye bağlı ürün var mı kontrol et
            var hasProducts = await _categoryRepository.HasProductsAsync(id); 
            if (hasProducts)
                return BadRequest("Bu kategoriye bağlı ürünler var, silinemez!");

            // Silme işlemi
            var deleted = await _categoryRepository.RemoveAsync(id);
            return Ok(deleted);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Asistan")]

        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            { return NotFound(); }

            _mapper.Map(dto, category);

            var updatedCategory = await _categoryRepository.UpdateAsync(category);
            return Ok(updatedCategory);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var data = await _categoryRepository.GetByIdAsync(id);
            if (data == null)
                return NotFound();

            return Ok(data);

        }
        [HttpGet("{categoryId}/products")]
        public IActionResult GetProductsByCategory(int categoryId)
        {
            var products = _categoryRepository.GetProductsByCategoryId(categoryId);
            return Ok(products);

        }

    }
}

