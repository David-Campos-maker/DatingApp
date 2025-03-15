using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();

            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null) return NotFound();

            return Ok(user);
        }
    }
}