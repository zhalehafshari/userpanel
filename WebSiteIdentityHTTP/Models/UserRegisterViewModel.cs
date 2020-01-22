using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebSiteIdentityHTTP.Models
{
    public class UserRegisterViewModel
    {
        [Required]
        public string  Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required,DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ReEnterPassword { get; set; }
    }
}
