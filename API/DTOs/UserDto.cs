using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UserDto
    {
        public required string Username { get; set; }
        public required string KnownAs { get; set; }
        public required string Token { get; set; }
        public string? PhotoUrl { get; set; }
    }
}