using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Objects
{
    public class coUserLoginCheck
    {

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = "";

        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }
}
