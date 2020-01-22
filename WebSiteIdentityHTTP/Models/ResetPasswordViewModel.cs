using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebSiteIdentityHTTP.Models
{
    public class ResetPasswordViewModel
    {
        public string ActiveCode { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required]
        
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [Required]
        
        [Compare("Password")]
        public string RePassword { get; set; }
    }
}
