using AddressBook.Entities.DTOs;

namespace AddressBook.Contracts
{
    public interface IAdminDetailsService{
        Tuple<int,TokenDto> Login(LoginDto LoginDto);
        Tuple<bool,string> InsertMetaData(List<MetadataDto> metadataDto);
        TokenDto GenerateJSONWebToken(string userId, string firstName, string lastName);
        void InsertSetRefTermData(Guid refsetId, Guid reftermId);
        Tuple<bool,string> AddRefTermToTheExistingRefset(MetadataDto metadataDto);
    }
}