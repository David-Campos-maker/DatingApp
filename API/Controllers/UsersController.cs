using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UsersController
    (
        IUserRepository userRepository,
        IMapper mapper,
        IPhotoService photoService
    ) : BaseApiController
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IPhotoService _photoService = photoService;

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();

            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByUsername([FromRoute] string username)
        {
            var user = await _userRepository.GetMemberAsync(username);

            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(MemberUpdateDto memberDto)
        {
            var user = await _userRepository.GetByUsernameAsync(User.GetUsername());
            if (user == null) return BadRequest("User not found");

            _mapper.Map(memberDto, user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetByUsernameAsync(User.GetUsername());
            if (user == null) return BadRequest("User not found");

            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };

            user.Photos.Add(photo);

            if (await _userRepository.SaveAllAsync())
                return CreatedAtAction
                (
                    nameof(GetUserByUsername),
                    new { username = user.UserName }, _mapper.Map<PhotoDto>(photo)
                );

            return BadRequest("Failed to add photo");
        }

        [HttpPut("set-main-photo/{photoId:int}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetByUsernameAsync(User.GetUsername());
            if (user == null) return BadRequest("Could not find user");

            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
            if (photo == null || photo.IsMain) return BadRequest("Cannot use this photo as main");

            var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);
            if (currentMain != null) currentMain.IsMain = false;

            photo.IsMain = true;

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");
        }
    }
}