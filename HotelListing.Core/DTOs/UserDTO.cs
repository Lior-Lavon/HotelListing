using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.Core.DTOs
{
    public class LoginUserDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, ErrorMessage = "Your Password is limited to 15 characters", MinimumLength = 2)]
        public string Password { get; set; }
    }

    public class UserDTO : LoginUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public List<string> Roles { get; set; }
    }
}
