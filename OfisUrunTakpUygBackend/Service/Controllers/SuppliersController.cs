using AutoMapper;
using DTO.SupplierDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfisUrunTakip.WebApi.Entity;
using Repositories.Contracts;

namespace OfisUrunTakip.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly IGenericRepository<Supplier> _supplierRepository;
        private readonly IMapper _mapper;

        public SuppliersController(IGenericRepository<Supplier> supplierRepository, IMapper mapper)
        {
            _mapper = mapper;
            _supplierRepository = supplierRepository;
        }
        [HttpGet]
       
        public async Task<IActionResult> Get()
        {
            var list = await _supplierRepository.GetAllAsync();
            var supplierdto = _mapper.Map<IEnumerable<Supplier>>(list);
            return Ok(supplierdto);
        }
        [HttpPost]
        [Authorize(Roles = "Asistan")]

        public async Task<IActionResult> Create([FromBody] SupplierAddDto dto)
        {
            if (await _supplierRepository.ExistsAsync(x => x.Name == dto.Name || x.TaxNumber == dto.TaxNumber))
                return BadRequest("Eklediğiniz kişi zaten mevcut");
            if (dto == null) return BadRequest();
            var data = _mapper.Map<Supplier>(dto);
            await _supplierRepository.AddAsync(data);
            return Ok(data);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _supplierRepository.RemoveAsync(id);
            if (data == null) return BadRequest();
            return Ok(data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Update(int id, [FromBody] SupplierUpdateDto dto)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null) return NotFound();
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
