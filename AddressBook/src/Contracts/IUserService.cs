using System.Runtime.Serialization.Formatters;
using AddressBook.Entities.DTOs;
using AddressBook.Entities.Models;
using Microsoft.AspNetCore.Http;

namespace AddressBook.Contracts
{
    public interface IUserService{
        Tuple<int,Guid> CreateUser(UserDto userDto);
        Tuple<bool,List<UserDto>> GetUserDetails();
        int GetUserCount();
        Tuple<bool,UserDto> GetUserDetails(Guid id);
        Tuple<bool,string> UpdateUser(Guid id, UserDto userDto);
        bool DeleteUser(Guid id);
        bool UploadFile(Guid id, IFormFile file);
        Tuple<bool,FileDto> DownloadFile(Guid id);

    }
}