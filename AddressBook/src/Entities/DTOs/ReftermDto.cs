using System.ComponentModel.DataAnnotations;
using AddressBook.Util.Models;

namespace AddressBook.Entities.DTOs
{
    public class ReftermDto 
    {

        [Required]
        public string RefTermKey { get; set;}

        [Required]
        public string Description {get; set;}
    }
}