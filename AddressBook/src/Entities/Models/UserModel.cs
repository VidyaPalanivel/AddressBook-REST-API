using System.ComponentModel.DataAnnotations;
using AddressBook.Util.Models;

namespace AddressBook.Entities.Models
{
    public class UserModel : BaseModel
    {
        [Key]
        [Required]
        public Guid Id {get; set;}

        [Required]
        public string FirstName { get; set;}

        [Required]
        public string LastName {get; set;}

        [Required]
        public string UserId {get; set;}

        [Required]
        [DataType(DataType.Password)]
        public string Password {get; set;}

    }
}