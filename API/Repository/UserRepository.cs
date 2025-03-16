using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using System.Security.Cryptography;
using System.Text;
using API.Interfaces;
using API.Entities;
using API.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        
        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<AppUser?> GetByIdAsync(int id) {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Photos)
                .SingleOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetAllAsync() {
            return await _context.Users
                .Include(u => u.Photos)
                .ToListAsync();
        }

        public async Task<AppUser> RegisterUserAsync(RegisterDto registerDto)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == 1);
            // using var hmac = new HMACSHA512();

            // var user = new AppUser
            // {
            //     UserName = registerDto.Username.ToLower(),
            //     PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            //     PasswordSalt = hmac.Key
            // };

            // _context.Users.Add(user);
            // await _context.SaveChangesAsync();

            // return user;
        }

        public async Task<AppUser?> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => 
                x.UserName == loginDto.Username.ToLower());

            if (user == null) return null;

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return null;
            }

            return user;
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await _context.Users
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<MemberDto?> GetMemberAsync(string username)
        {
            return await _context.Users
                .Where(u => u.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }
    }
}