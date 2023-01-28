using System.ComponentModel.DataAnnotations;

namespace AddressBook.Entities.DTOs{
    public class LoginDto{
        
        public string UserId { get; set;}

        public string Password {get; set;}
    }
}