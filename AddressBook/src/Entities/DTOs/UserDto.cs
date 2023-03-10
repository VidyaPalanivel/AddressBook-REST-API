using System.ComponentModel.DataAnnotations;

namespace AddressBook.Entities.DTOs{
    public class UserDto{
        
        [Required]
        public string FirstName {get; set;}

        [Required]
        public string LastName {get; set;}

        [Required]
        public string UserId {get; set;}

        [Required]
        public string Password {get; set;}

        [Required]
        public List<EmailDto> Email {get; set;}

        [Required]
        public List<AddressDto> Address {get; set;}

        [Required]
        public List<PhoneDto> Phone {get; set;}

    }
}