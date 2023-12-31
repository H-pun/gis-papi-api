﻿using GISPaPi.Base;
using GISPaPi.DataAccess.Entities;

namespace GISPaPi.DataAccess.Models
{
    public class RegisterUserRequest : BaseModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
