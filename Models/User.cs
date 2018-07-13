using System;//added this dependcy
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Globalization;//added this dependency
using System.ComponentModel.DataAnnotations;//added this dependency
using System.ComponentModel.DataAnnotations.Schema;//added this dependency

namespace BankAccount.Models//you project namespace

{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter between 2 and 25 characters")]
        [MinLength(2),MaxLength(25)]
        //[RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string first { get; set; }

        [Required(ErrorMessage = "Please enter between 2 and 25 characters")]
        [MinLength(2),MaxLength(25)]
        //[RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string last { get; set; }

        [Required(ErrorMessage = "Please enter between 10 and 255 characters")]
        [MinLength(8),MaxLength(45)]
        public string email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string password { get; set; }

        [NotMapped]
        [CompareAttribute("password")]
        public string ConfirmPassword { get; set; }

        //public int integer { get; set; }

        //[Required]
        //public DateTime date { get; set; }
    }
}