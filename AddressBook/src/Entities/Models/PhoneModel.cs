using System.ComponentModel.DataAnnotations;
using AddressBook.Util.Models;

namespace AddressBook.Entities.Models
{
    public class PhoneModel : BaseModel
    {
        
        [Key]
        [Required]
        public Guid Id {get; set;}

        [Required]
        public Guid UserId {get; set;}
        [Required]
        public string PhoneNumber { get; set;}

        [Required]
        public Guid Type {get; set;}
    }
}