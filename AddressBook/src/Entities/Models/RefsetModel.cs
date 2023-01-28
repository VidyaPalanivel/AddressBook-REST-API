using System.ComponentModel.DataAnnotations;
using AddressBook.Util.Models;

namespace AddressBook.Entities.Models
{
    public class RefsetModel : BaseModel
    {
        [Key]
        [Required]
        public Guid Id {get; set;}

        [Required]
        public string RefSetKey { get; set;}

        [Required]
        public string Description {get; set;}
    }
}