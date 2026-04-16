using AutoMapper;
using DTO.SupplierDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfisUrunTakip.WebApi.Entity;
using Repositories.Contracts;

namespace OfisUrunTakip.WebApi.Controllers
{[Route("api/[controller]")]
[ApiController]
public class SuppliersController : ControllerBase
    {
        private readonly IGenericRepository<Supplier> _supplierRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SuppliersController> _logger;

        public SuppliersController(
            IGenericRepository<Supplier> supplierRepository,
            IMapper mapper,
            ILogger<SuppliersController> logger)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Tüm tedarikçiler listeleniyor.");
            var list = await _supplierRepository.GetAllAsync();
            var supplierDto = _mapper.Map<IEnumerable<SupplierListDto>>(list);
            _logger.LogInformation("Toplam {Count} tedarikçi getirildi.", supplierDto.Count());
            return Ok(supplierDto);
        }
        [HttpPost]
       // [Authorize(Roles = "Asistan")]

        public async Task<IActionResult> Create([FromBody] SupplierAddDto dto)
        {
            _logger.LogInformation("Tedarikçi  ekleme isteği alındı. Kullanıcı: {User}", User.Identity?.Name);
            if (await _supplierRepository.ExistsAsync(x => x.Name == dto.Name || x.TaxNumber == dto.TaxNumber))
            {
                _logger.LogWarning("Aynı isimde tearikçi mevcut: {SupplierName}", dto.Name);
                return BadRequest("Eklediğiniz tedarikçi zaten mevcut");
            }
            if (dto == null) {
                _logger.LogWarning("Ürün ekleme null dto ile geldi."); 
                return BadRequest(); }
            var data = _mapper.Map<Supplier>(dto);
            await _supplierRepository.AddAsync(data);
            _logger.LogInformation("Yeni tedarikçi eklendi. Id: {Id}, Name: {Name}", data.Id, data.Name);
            return Ok(data);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Tedarikçi silme isteği alındı. Id: {Id}", id);
            var data = await _supplierRepository.RemoveAsync(id);
            if (data == null) {
                _logger.LogWarning("Silinmek istenen tedarikçi bulunamadı. Id: {Id}", id); 
                return BadRequest(); }
            _logger.LogInformation("Tedarikçi silindi. Id: {Id}", id);
            return Ok(data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Update(int id, [FromBody] SupplierUpdateDto dto)
        {
            _logger.LogInformation("Tedarikçi güncelleme isteği alındı. Id: {Id}", id);
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null) {
                _logger.LogWarning("Güncellenmek istenen Tedarikçi bulunamadı. Id: {Id}", id); 
                return NotFound(); }
            _mapper.Map(dto, supplier);
            var updateSupplier =await _supplierRepository.UpdateAsync(supplier);
            return Ok(updateSupplier);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = _supplierRepository.GetByIdAsync(id);
            return Ok(data);

        }
    }
}
