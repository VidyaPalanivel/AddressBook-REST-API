using System.Collections.Generic;
namespace AddressBook.Entities.DTOs{
public class MetadataDto
    {
        public RefsetDto Refset { get; set; }

        public List<ReftermDto> Refterm { get; set; }

        
       
    }
}