using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using AddressBook.Contracts;
using AddressBook.Entities.DTOs;
using AddressBook.Entities.Models;
using Microsoft.Extensions.Configuration;
using System.Text;
using AddressBook.Logger.IManager;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using AddressBook.Util;

namespace AddressBook.Service
{
    public class UserService : IUserService
    {
        private readonly ILoggerManager _logger;
        private readonly AddressBookContext _context;

        
        public UserService( ILoggerManager logger, AddressBookContext context)
        {
            _logger = logger; 
            _context = context;
        }

        public Tuple<int,Guid> CreateUser(UserDto userDto){
            
            _logger.LogDebug("Entering into CreateUser method in UserService");
            var userRecords = _context.User.Where(x=> x.IsActive == true).Select(x=> x.UserId).ToList<string>();
            bool userCheck = userRecords.Any(userDto.UserId.Contains);
            if(userCheck && userRecords.Count != 0)
                return Tuple.Create(1,Guid.Empty);
            var refset = _context.RefSet.Where(x => x.IsActive == true).Select(x=>x.RefSetKey).ToList();
            var refterm = _context.RefTerm.Where(x => x.IsActive == true).Count();
            if(!(refset.Contains("PHONE_TYPE")) || !(refset.Contains("EMAIL_TYPE")) || !(refset.Contains("ADDRESS_TYPE")) || refterm == 0)
                return Tuple.Create(2,Guid.Empty);
            
            
            var input = userDto.Password;
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");
            var isValidated = hasNumber.IsMatch(input) && hasUpperChar.IsMatch(input) && hasMinimum8Chars.IsMatch(input);
            
            //Check password if it matches with the required condition for the passwords
            if(isValidated)   
            {
                var userId = Guid.NewGuid();
               //encrytping password to store it in Database
                var encryptedData = StringExtensions.Encode(userDto.Password);
                _context.User.Add(new UserModel{
                Id = userId,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                UserId = userDto.UserId,
                Password = encryptedData
                });
                var listOfEmail = userDto.Email;
                var listOfPhone = userDto.Phone;
                var listOfAddress = userDto.Address;
                var emailCheck = InsertEmailDetails(listOfEmail,userId);
                //Check email already exists or not
                if(!emailCheck)
                    return Tuple.Create(3,Guid.Empty);
                InsertPhoneDetails(listOfPhone,userId);
                InsertAddressDetails(listOfAddress,userId);
                _context.SaveChanges();
                _logger.LogDebug("Exiting from CreateUser method in UserService");
                return Tuple.Create(4,userId);
            }           
            else{
                _logger.LogDebug("Exiting from CreateUser method in UserService");
                return Tuple.Create(5,Guid.Empty);
            }
        }
    
        public Tuple<bool,string> UpdateUser(Guid id, UserDto userDto){
            _logger.LogDebug("Entering into UpdateUser method in UserService");
            var userDetails = _context.User.Where(x=> x.Id == id && x.IsActive == true).FirstOrDefault();
            if(userDetails == null)
            {
                _logger.LogDebug("Exiting from UpdateUser method in UserService");
                return Tuple.Create(false,"User Not Found");
            }
                userDetails.FirstName=userDto.FirstName;
                userDetails.LastName=userDto.LastName;
                userDetails.UserId=userDto.UserId;
                userDetails.Password=userDto.Password;
                userDetails.DateUpdated = DateTime.Now;
            _context.User.Update(userDetails);

            var emailDetails = _context.Email.Where(x=> x.UserId == id && x.IsActive == true).ToList();
            var phoneDetails = _context.Phone.Where(x=> x.UserId == id && x.IsActive == true).ToList();
            var addressDetails = _context.Address.Where(x=> x.UserId == id && x.IsActive == true).ToList();
            var listOfInputEmail = userDto.Email;
            var listOfInputPhone = userDto.Phone;
            var listOfInputAddress = userDto.Address;

            if(emailDetails != null)
            {
                for (int i=0; i<listOfInputEmail.Count; i++)
                {
                    emailDetails[i].EmailAddress = listOfInputEmail[i].EmailAddress;
                    emailDetails[i].Type = FindEmailType(listOfInputEmail[i].Type);
                    emailDetails[i].DateUpdated = DateTime.Now;
                }
                _context.Email.UpdateRange(emailDetails);
            }
            else
            {
                bool emailCheck = InsertEmailDetails(listOfInputEmail, userDetails.Id);
                //Check email already exists or not
                if(!emailCheck)
                {
                    _logger.LogDebug("Exiting from UpdateUser method in UserService");
                    return Tuple.Create(false, "Email already exists");
                }
            }
            
            if(phoneDetails != null)
            {
                for (int i=0; i<listOfInputPhone.Count; i++)
                {
                    phoneDetails[i].PhoneNumber = listOfInputPhone[i].PhoneNumber;
                    phoneDetails[i].Type = FindPhoneNumberType(listOfInputPhone[i].Type);
                    phoneDetails[i].DateUpdated = DateTime.Now;
                }
                _context.Phone.UpdateRange(phoneDetails);
            }
            else
                InsertPhoneDetails(listOfInputPhone, userDetails.Id);
             
            if(addressDetails != null)
            {
                for (int i=0; i<listOfInputAddress.Count; i++)
                {
                    addressDetails[i].DoorNo = listOfInputAddress[i].DoorNo;
                    addressDetails[i].StreetName = listOfInputAddress[i].StreetName;
                    addressDetails[i].City = listOfInputAddress[i].City;
                    addressDetails[i].State = listOfInputAddress[i].State;
                    addressDetails[i].Country = listOfInputAddress[i].Country;
                    addressDetails[i].ZipCode = listOfInputAddress[i].ZipCode;
                    addressDetails[i].Type = FindAddressType(listOfInputAddress[i].Type);
                    addressDetails[i].DateUpdated = DateTime.Now;
                }
                _context.Address.UpdateRange(addressDetails);
            }
            else 
                InsertAddressDetails(listOfInputAddress, userDetails.Id);
            _context.SaveChanges();
            _logger.LogDebug("Exiting from UpdateUser method in UserService");
            return Tuple.Create(true,"");
        }


        public Guid FindEmailType(string type){
            List<ReftermModel> refTerm = _context.RefTerm.Where(x=> x.IsActive == true).ToList();
            Guid id = Guid.Empty;
            foreach(var key in refTerm)
            {
                if(key.IsActive == true && key.RefTermKey == type)
                {
                    return key.Id;
                }
            }
            return id;
            
        }

        public Guid FindPhoneNumberType(string type){
            List<ReftermModel> refTerm = _context.RefTerm.Where(x=> x.IsActive == true).ToList();
            Guid id = Guid.Empty;
            foreach(var key in refTerm)
            {
                if(key.IsActive == true && key.RefTermKey == type)
                {
                    return key.Id;
                }
            }
            return id;
            
        }

        public Guid FindAddressType(string type){
            List<ReftermModel> refTerm = _context.RefTerm.Where(x=> x.IsActive == true).ToList();
            Guid id = Guid.Empty;
            foreach(var key in refTerm)
            {
                if(key.IsActive == true && key.RefTermKey == type)
                {
                    return key.Id;
                }
            }
            return id;
            
        }

        //Method to check whether email already exits
        public string EmailCheck(Guid userId,string email)
        {
            _logger.LogDebug("Entering into EmailCheck method in UserService");
            var emailDetails = _context.Email.Where(x=>x.EmailAddress == email && x.UserId != userId && x.IsActive == true).FirstOrDefault();
            if(emailDetails != null)
            {
                 _logger.LogDebug("Exiting from EmailCheck method in UserService");
                return null;
            }
            else
            {
                _logger.LogDebug("Exiting from EmailCheck method in UserService");
                return email;
            }
                
        }
    
        public bool InsertEmailDetails(List<EmailDto> emailDetails, Guid userId)
        {
            _logger.LogDebug("Entering into InsertEmailDetails method in UserService");
            List<EmailModel> emailModel = new List<EmailModel>();
            foreach(var emailDetail in emailDetails)
            {
                var emailCheck = EmailCheck(userId, emailDetail.EmailAddress);
                //Check email already exists or not
                if(emailCheck == null)
                {
                    _logger.LogDebug("Exiting from InsertEmailDetails method in UserService");
                    return false;
                }
                emailModel.Add(new EmailModel{
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    EmailAddress = emailCheck,
                    Type = FindEmailType(emailDetail.Type)
                });
            }
            _context.Email.AddRange(emailModel);
            _logger.LogDebug("Exiting from InsertEmailDetails method in UserService");
            return true;
        }

        public void InsertPhoneDetails(List<PhoneDto> phoneDetails, Guid userId)
        {
            _logger.LogDebug("Entering into InsertPhoneDetails method in UserService");
            List<PhoneModel> phoneModel = new List<PhoneModel>();
            foreach(var phoneDetail in phoneDetails)
            {
                phoneModel.Add(new PhoneModel{
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    PhoneNumber = phoneDetail.PhoneNumber,
                    Type = FindPhoneNumberType(phoneDetail.Type)
                });
            }
            _context.Phone.AddRange(phoneModel);
            _logger.LogDebug("Exiting from InsertPhoneDetails method in UserService");
        }

        public void InsertAddressDetails(List<AddressDto> addressDetails, Guid userId)
        {
            _logger.LogDebug("Entering into InsertAddressDetails method in UserService");
            List<AddressModel> addressModel = new List<AddressModel>(); 
            foreach( var addressDetail in addressDetails)
            {
                addressModel.Add(new AddressModel{
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    DoorNo = addressDetail.DoorNo,
                    StreetName = addressDetail.StreetName,
                    City = addressDetail.City,
                    State = addressDetail.State,
                    Country = addressDetail.Country,
                    ZipCode = addressDetail.ZipCode,
                    Type = FindAddressType(addressDetail.Type)
                });
            }
            _context.Address.AddRange(addressModel);
            _logger.LogDebug("Exiting from InsertAddressDetails method in UserService");
        }
    
        public Tuple<bool,List<UserDto>> GetUserDetails()
        {
            try{
                _logger.LogDebug("Entering into GetUserDetails method in UserService");
            var userDetails = _context.User.Where(x=>x.IsActive == true).ToList();
            List<UserDto> addressBookDetails = new List<UserDto>();
            UserDto addressBook = new UserDto();
            foreach(var userDetail in userDetails)
            {
                addressBook.FirstName = userDetail.FirstName;
                addressBook.LastName = userDetail.LastName;
                addressBook.UserId = userDetail.UserId;
                addressBook.Password = userDetail.Password;
                addressBook.Email = GetEmailDetails(userDetail.Id);
                addressBook.Phone = GetPhoneDetails(userDetail.Id);
                addressBook.Address = GetAddressDetails(userDetail.Id);
                addressBookDetails.Add(addressBook);
            }
            _logger.LogDebug("Exiting from GetUserDetails method in UserService");
            return Tuple.Create(true,addressBookDetails);
            }
            catch{
                List<UserDto> myList = new List<UserDto>();
                return Tuple.Create(false,myList);
            }
            
        }

        public Tuple<bool,UserDto> GetUserDetails(Guid userId)
        {
            try{
                _logger.LogDebug("Entering into GetUserDetails method in UserService");
            var userDetails = _context.User.Where(x=>x.Id == userId && x.IsActive == true).FirstOrDefault();
            UserDto addressBook = new UserDto();

                addressBook.FirstName = userDetails.FirstName;
                addressBook.LastName = userDetails.LastName;
                addressBook.UserId = userDetails.UserId;
                addressBook.Password = userDetails.Password;
                addressBook.Email = GetEmailDetails(userDetails.Id);
                addressBook.Phone = GetPhoneDetails(userDetails.Id);
                addressBook.Address = GetAddressDetails(userDetails.Id);
            _logger.LogDebug("Exiting from GetUserDetails method in UserService");
            return Tuple.Create(true,addressBook);
            }
            catch{
                UserDto addressBook = new UserDto();
                return Tuple.Create(false,addressBook);
            }
            
        }

        public int GetUserCount()
        {
            _logger.LogDebug("Entering into GetUserCount method in UserService");
           return _context.User.Where(x=>x.IsActive == true).Count();
        }
        public List<EmailDto> GetEmailDetails(Guid userId)
        {
            _logger.LogDebug("Entering into GetEmailDetails method in UserService");
            var emailDetails = _context.Email.Where(x=>x.UserId == userId && x.IsActive == true).ToList();
            List<EmailDto> emailDtos = new List<EmailDto>();
            EmailDto email = new EmailDto();
            foreach(var emailDetail in emailDetails)
            {
                email.EmailAddress = emailDetail.EmailAddress;
                email.Type = _context.RefTerm.Where(x=> x.Id == emailDetail.Type && x.IsActive == true).Select(x=> x.RefTermKey).First();
                emailDtos.Add(email);
            }
            _logger.LogDebug("Exiting from GetEmailDetails method in UserService");
            return emailDtos;
            
        }

        public List<PhoneDto> GetPhoneDetails(Guid userId)
        {
            _logger.LogDebug("Entering into GetPhoneDetails method in UserService");
            var phoneDetails = _context.Phone.Where(x=>x.UserId == userId && x.IsActive == true).ToList();
            List<PhoneDto> phoneDtos = new List<PhoneDto>();
            PhoneDto phone = new PhoneDto();
            foreach(var phoneDetail in phoneDetails)
            {
                phone.PhoneNumber = phoneDetail.PhoneNumber;
                phone.Type = _context.RefTerm.Where(x=> x.Id == phoneDetail.Type && x.IsActive == true).Select(x=> x.RefTermKey).First();
                phoneDtos.Add(phone);
            }
            _logger.LogDebug("Exiting from GetPhoneDetails method in UserService");
            return phoneDtos;
        }

        public List<AddressDto> GetAddressDetails(Guid userId)
        {
            _logger.LogDebug("Entering into GetAddressDetails method in UserService");
            var addressDetails = _context.Address.Where(x=>x.UserId == userId && x.IsActive == true).ToList();
            List<AddressDto> addressDtos = new List<AddressDto>();
            AddressDto address = new AddressDto();
            foreach(var addressDetail in addressDetails)
            {
                address.DoorNo = addressDetail.DoorNo;
                address.StreetName = addressDetail.StreetName;
                address.City = addressDetail.City;
                address.State = addressDetail.State;
                address.Country = addressDetail.Country;
                address.ZipCode = addressDetail.ZipCode;
                address.Type = _context.RefTerm.Where(x=> x.Id == addressDetail.Type && x.IsActive == true).Select(x=> x.RefTermKey).First();
                addressDtos.Add(address);
            }
            _logger.LogDebug("Exiting from GetAddressDetails method in UserService");
            return addressDtos;
        }

        public bool DeleteUser(Guid userId)
        {
            _logger.LogDebug("Entering into DeleteUser method in UserService");
            var userDetails = _context.User.Where(x=> x.Id == userId && x.IsActive == true).FirstOrDefault();
            if(userDetails == null)
            {
                _logger.LogDebug("Exiting from DeleteUser method in UserService");
                return false;
            }   
            else
            {
                userDetails.IsActive = false;
                userDetails.DateUpdated = DateTime.Now;
                _context.User.Update(userDetails);
                var emailDetails = _context.Email.Where(x=> x.UserId == userId && x.IsActive == true).ToList();
                var phoneDetails = _context.Phone.Where(x=> x.UserId == userId && x.IsActive == true).ToList();
                var addressDetails = _context.Address.Where(x=> x.UserId == userId && x.IsActive == true).ToList();

                foreach(var email in emailDetails)
                {
                    email.IsActive = false;
                    email.DateUpdated = DateTime.Now;
                    _context.Email.Update(email);
                }
                foreach(var phoneDetail in phoneDetails)
                {
                    phoneDetail.IsActive = false;
                    phoneDetail.DateUpdated = DateTime.Now;
                    _context.Phone.Update(phoneDetail);
                }
                foreach(var addressDetail in addressDetails)
                {
                    addressDetail.IsActive = false;
                    addressDetail.DateUpdated = DateTime.Now;
                    _context.Address.Update(addressDetail);
                }
                _context.SaveChanges();
                _logger.LogDebug("Exiting from DeleteUser method in UserService");
                return true;
            }
        }
    
        public bool UploadFile(Guid id, IFormFile file)
        {
            _logger.LogDebug("Entering into UploadFile method in UserService");
            var userDetails = _context.User.Where(x=> x.Id == id && x.IsActive == true).FirstOrDefault();
            if(userDetails == null)
            {
                _logger.LogDebug("Exiting from UploadFile method in UserService");
                return false;
            }
            var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            var fileInBytes = memoryStream.ToArray();
            string base64_string = Convert.ToBase64String(fileInBytes);
            string type = (string)file.ContentType.ToUpper();
            _context.File.Add(
                new FileModel{
                    FileContent = base64_string,
                    UserId = id,
                    Type = _context.RefTerm.Where(x=> x.RefTermKey == type && x.IsActive == true).Select(x=> x.Id).First()
            
                }
            );
            _context.SaveChanges();
            _logger.LogDebug("Exiting from UploadFile method in UserService");
            return true;
        }

        public Tuple<bool,FileDto> DownloadFile(Guid id)
        {
            _logger.LogDebug("Entering into DownloadFile method in UserService");
            var fileDetails = _context.File.FirstOrDefault(x=>x.Id == id && x.IsActive == true);
            var fileDto = new FileDto();
            if(fileDetails == null)
            {
                _logger.LogDebug("Exiting from DownloadFile method in UserService");
                return Tuple.Create(false,fileDto);
            }
            var byteFile = System.Convert.FromBase64String(fileDetails.FileContent);
            fileDto.FileContent = byteFile;
            fileDto.Type = _context.RefTerm.Where(x=>x.Id == fileDetails.Type && x.IsActive == true).Select(x=>x.RefTermKey).FirstOrDefault();
            _logger.LogDebug("Exiting from DownloadFile method in UserService");
            return Tuple.Create(true,fileDto);
        }
    }
}
