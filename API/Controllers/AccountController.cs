using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController
    (
        IUserRepository userRepository, 
        ITokenService tokenService, 
        IMapper mapper
    ) : BaseApiController
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IMapper _mapper = mapper;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterDto registerDto)
        {
            if (await _userRepository.UserExistsAsync(registerDto.Username)) 
                return BadRequest("User already registered");

            var user = await _userRepository.RegisterUserAsync(registerDto);

            var result = new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs
            };

            return Ok(result);
        } 

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto loginDto)
        {
            var user = await _userRepository.LoginAsync(loginDto);

            if (user == null) return Unauthorized("Invalid username or password");

            var result = new UserDto
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url
            };

            return Ok(result);
        }
    }
}