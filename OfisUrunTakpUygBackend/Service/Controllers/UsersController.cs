using AutoMapper;
using DTO.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfisUrunTakip.WebApi.Entity;
using Repositories.Contracts;

namespace OfisUrunTakip.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _userRepository.GetAllAsync();
            var d = _mapper.Map<IEnumerable<User>>(data);
            return Ok(d);

        }
        [HttpPost]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Create([FromBody] UserAddDto dto)
        {
            if (await _userRepository.ExistsAsync(x => x.Email == dto.Email))
                return BadRequest("Bu kullanıcı zaten var!");
            if (dto == null) return BadRequest();
            var user = _mapper.Map<User>(dto);

            user.Password = _userRepository.HashPassword(dto.Password);
            await _userRepository.AddAsync(user);
            return Ok(user);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _userRepository.RemoveAsync(id);
            return Ok(data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Asistan")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            _mapper.Map(dto, user);

            var updateUser = await _userRepository.UpdateAsync(user);
            return Ok(updateUser);
        }



        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            var a = await _userRepository.GetByIdAsync(id);
            return Ok(a);
        }

    }
}
