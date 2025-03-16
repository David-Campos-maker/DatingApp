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
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        public Task<IEnumerable<AppUser>> GetAllAsync();
        public Task<AppUser?> GetByIdAsync(int id);
        public Task<AppUser?> GetByUsernameAsync(string username);
        public Task<AppUser> RegisterUserAsync(RegisterDto registerDto);
        public Task<AppUser?> LoginAsync(LoginDto loginDto);
        public Task<bool> UserExistsAsync(string username);
        public Task<IEnumerable<MemberDto>> GetMembersAsync();
        public Task<MemberDto?> GetMemberAsync(string username);
    }
}