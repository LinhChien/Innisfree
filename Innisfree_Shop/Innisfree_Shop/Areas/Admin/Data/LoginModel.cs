using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Innisfree_Shop.Areas.Admin.Data
{
    public class LoginModel
    {
        [Required(ErrorMessage = "mời nhâp vào user name")]
        public string UserName { set; get; }

        [Required(ErrorMessage = "mời nhâp vào password")]
        public string Password { set; get; }

        public bool RememberMe { set; get; }
    }
}