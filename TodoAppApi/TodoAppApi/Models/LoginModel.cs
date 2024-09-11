﻿using System.ComponentModel.DataAnnotations;

namespace TodoAppApi.Models
{
    public class LoginModel
    {
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(250)]
        public string Password { get; set; }
    }
}
