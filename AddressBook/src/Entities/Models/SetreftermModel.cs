using System.ComponentModel.DataAnnotations;
using AddressBook.Util.Models;

namespace AddressBook.Entities.Models
{
    public class SetreftermModel : BaseModel
    {
        [Key]
        [Required]
        public Guid Id {get; set;}

        [Required]
        public Guid RefSetId { get; set;}

        [Required]
        public Guid RefTermId {get; set;}
    }
}