using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        public Task<IEnumerable<AppUser>> GetAllAsync();
        public Task<AppUser?> GetByIdAsync(int id);
        public Task<AppUser> RegisterUserAsync(RegisterDto registerDto);
        public Task<AppUser?> LoginAsync(LoginDto loginDto);
        public Task<bool> UserExistsAsync(string username);
    }
}