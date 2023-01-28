using System.ComponentModel.DataAnnotations;
using AddressBook.Util.Models;

namespace AddressBook.Entities.Models
{
    public class ReftermModel : BaseModel
    {
        [Key]
        [Required]
        public Guid Id {get; set;}

        [Required]
        public string RefTermKey { get; set;}

        [Required]
        public string Description {get; set;}
    }
}