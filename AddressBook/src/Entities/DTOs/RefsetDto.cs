using System.ComponentModel.DataAnnotations;
using AddressBook.Util.Models;

namespace AddressBook.Entities.DTOs
{
    public class RefsetDto 
    {

        [Required]
        public string RefSetKey { get; set;}

        [Required]
        public string Description {get; set;}
    }
}