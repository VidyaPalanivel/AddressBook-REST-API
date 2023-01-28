using System.ComponentModel.DataAnnotations;

namespace AddressBook.Entities.DTOs{
    public class AddressDto{
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
        public string Type {get; set;}
    }
}