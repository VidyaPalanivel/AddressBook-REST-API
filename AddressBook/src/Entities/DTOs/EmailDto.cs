using System.ComponentModel.DataAnnotations;

namespace AddressBook.Entities.DTOs{
    public class EmailDto{
        public string EmailAddress { get; set;}

        public string Type {get; set;}
    }
}