using System.ComponentModel.DataAnnotations;

namespace AddressBook.Entities.DTOs{
    public class FileDto{
        
        [Required]
        public byte[] FileContent {get; set;}

        [Required]
        public string Type {get; set;}

    }
}