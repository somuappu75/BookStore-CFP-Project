using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Model
{
  public  class UserModel
    {
        public int UserId { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Your FirstName should only contain Alphabets!")]
        public string FullName { get; set; }
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string MobileNumber { get; set; }
    }
}
