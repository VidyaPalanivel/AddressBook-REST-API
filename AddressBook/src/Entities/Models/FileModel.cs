using System.ComponentModel.DataAnnotations;
using AddressBook.Util.Models;

namespace AddressBook.Entities.Models
{
    public class FileModel : BaseModel
    {
        [Key]
        [Required]
        public Guid Id {get; set;}

        [Required]
        public Guid UserId { get; set;}

        [Required]
        public string FileContent {get; set;}

        [Required]
        public Guid Type {get; set;}
    }
}