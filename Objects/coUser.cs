using Microsoft.VisualBasic;
using Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Util;
using static Util.Statics.Enums;

namespace BlazorApp.Shared
{
    public class coUser : coBaseObject
    {
        public int UserID { get; set; }


        //[Required(ErrorMessage = "Kullanıcı adını girmediniz")]
        [Display(Name = "Kullanıcı Adı")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9]{5,13}$", ErrorMessage = "Kullanıcı adınız 6-14 karakter uzunluğunda ve sadece harf veya rakamlardan oluşmalıdır")]
        public string UserName { get; set; } = "";

        //[Required(ErrorMessage = "Adınızı girmediniz")]
        [Display(Name = "Ad")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\p{L}{2,}$", ErrorMessage = "Ad alanı sadece harflerden ve en az 2 karakterden oluşmalıdır.")]
        public string FirstName { get; set; } = "";

        //[Required(ErrorMessage = "Soyadınızı girmediniz")]
        [Display(Name = "Soyad")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\p{L}{2,}$", ErrorMessage = "Soyad alanı sadece harflerden ve en az 2 karakterden oluşmalıdır.")]
        public string LastName { get; set; } = "";

        //[Required(ErrorMessage = "Emailinizi girmediniz")]
        [Display(Name = "Email")]
        //[DataType(DataType.EmailAddress)]
        //[EmailAddress(ErrorMessage = "Lütfen geçerli bir email adresi girin")]
        public string UserEmail { get; set; } = "";

        /*[Required(ErrorMessage = "Emailinizi tekrar girmediniz")]
        [Display(Name = "Email Tekrar")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir email adresi girin")]
        [Compare("Email", ErrorMessage = "Girilen email adresleri uyuşmuyor")]*/
        public string ReEmail { get; set; } = "";

        //[Required(ErrorMessage = "Şifrenizi girmediniz")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?.!@$%^&*-]).{8,}$", ErrorMessage = "Şifreniz en az 1 büyük harf, 1 küçük harf, 1 rakam, 1 özel karakter içermeli ve en az 8 karakter uzunluğunda olmalıdır")]
        public string UserPassword { get; set; } = "";

        /*[Required(ErrorMessage = "Şifrenizi tekrar girmediniz")]
        [Display(Name = "Şifre Tekrar")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Girilen şifreler uyuşmuyor")]
        public string RePassword { get; set; } = "";*/

        //[Range(typeof(bool), "true", "true", ErrorMessage = "Kullanım koşullarını kabul etmediniz")]
        public bool ConfirmTerms { get; set; }
        public string ProfilePic { get; set; } = "";
        public long UserRole { get; set; }

        public long UserState { get; set; }
        public string UserGuid { get; set; }

        public DateFormat RegisterDate { get; set; }

        public string UserImageUrl()
        {
            return "https://instagram.fadb6-3.fna.fbcdn.net/v/t51.2885-19/269671416_497645684881259_9003318876448571170_n.jpg?stp=dst-jpg_s320x320&_nc_ht=instagram.fadb6-3.fna.fbcdn.net&_nc_cat=111&_nc_ohc=wDAQCX5KG-UAX_zJqbR&edm=ABfd0MgBAAAA&ccb=7-4&oh=00_AT9AXdwvpfOtCtnPQRfmH3AMozUjimNaWohjip3nliFZfw&oe=62675339&_nc_sid=7bff83";
        }

        public coUser()
        {

        }

        public coUser(coJWTToken token)
        {
            UserName = token.UserName;
            FirstName = token.FirstName;
            LastName = token.LastName;
            UserEmail = token.Email;
            UserRole = token.UserRole;
 
        }

    }
}
