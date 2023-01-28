using System.ComponentModel.DataAnnotations;
using AddressBook.Util.Models;

namespace AddressBook.Entities.Models
{
    public class AddressModel : BaseModel
    {
        [Key]
        [Required]
        public Guid Id {get; set;}

        [Required]
        public Guid UserId { get; set;}

        [Required]
        public string DoorNo {get; set;}

        [Required]
        public string StreetName {get; set;}

        [Required]
        public string City {get; set;}

        [Required]
        public string State {get; set;}

        [Required]
        public string Country {get; set;}

        [Required]
        public string ZipCode {get; set;}

        [Required]
        public Guid Type {get; set;}
    }
}