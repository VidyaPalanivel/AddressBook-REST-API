using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AddressBook.Entities.Models;
using System.Net.Http;
using AddressBook.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AddressBook.Controllers;
using AddressBook.Service;
using Microsoft.AspNetCore.Http;
using AddressBook.Entities.DTOs;

namespace AddressBookUnitTest.MockData
{
    public class MockDataDetails
    {
        public static List<MetadataDto> NewMetaData()
        {
            var metaDataList = new List<MetadataDto>();
            var refsetValue = new RefsetDto{
                RefSetKey = "PHONE_TYPE",
                Description = "Phone_Type"
            };
            var refTermValue = new ReftermDto{
                RefTermKey = "PERSONAL",
                Description = "Personal"
            };
            var refTermValue1 = new ReftermDto{
                RefTermKey = "HOME",
                Description = "Home"
            };
            var refTermValue2 = new ReftermDto{
                RefTermKey = "OFFICE",
                Description = "Office"
            };
            var refsetValue1 = new RefsetDto{
                RefSetKey = "ADDRESS_TYPE",
                Description = "Address_Type"
            };
            var refTermValue11 = new ReftermDto{
                RefTermKey = "PERSONAL",
                Description = "Personal"
            };
            var refTermValue12 = new ReftermDto{
                RefTermKey = "HOME",
                Description = "Home"
            };
            var refTermValue13 = new ReftermDto{
                RefTermKey = "OFFICE",
                Description = "Office"
            };
            var refsetValue2 = new RefsetDto{
                RefSetKey = "EMAIL_TYPE",
                Description = "Email_Type"
            };
            var refTermValue21 = new ReftermDto{
                RefTermKey = "PERSONAL",
                Description = "Personal"
            };
            var refTermValue22 = new ReftermDto{
                RefTermKey = "HOME",
                Description = "Home"
            };
            var refTermValue23 = new ReftermDto{
                RefTermKey = "OFFICE",
                Description = "Office"
            };
            var reftermValueList = new List<ReftermDto>();
            reftermValueList.Add(refTermValue);
            reftermValueList.Add(refTermValue1);
            reftermValueList.Add(refTermValue2);
            var metaData = new MetadataDto{
                Refset = refsetValue,
                Refterm = reftermValueList
            };
            var metaData1 = new MetadataDto{
                Refset = refsetValue1,
                Refterm = reftermValueList
            };
            var metaData2 = new MetadataDto{
                Refset = refsetValue2,
                Refterm = reftermValueList
            };
            metaDataList.Add(metaData);
            metaDataList.Add(metaData1);
            metaDataList.Add(metaData2);
            return metaDataList;
        }
    
        public static List<MetadataDto> NewMetaDataWithMissingData(){
            var metaDataList = new List<MetadataDto>();
            var refsetValue = new RefsetDto{
                RefSetKey = "PHONE_TYPE",
                Description = "Phone_Type"
            };
            var reftermValueList = new List<ReftermDto>();
            
           var metaData = new MetadataDto{
                Refset = refsetValue,
                Refterm = reftermValueList
            };
            metaDataList.Add(metaData);
            return metaDataList;
        }
        
       
        public static UserDto NewUserWithSameEmailId(){
             Guid userId = Guid.NewGuid();
            var addressList = new List<AddressDto>();
            var address = new AddressDto{
                DoorNo = "M-5",
                StreetName = "Anna Nagar",
                City = "Chennai",
                State = "Tamil Nadu",
                Country = "India",
                ZipCode = "637289",
                Type = "Home"
            };
            addressList.Add(address);
            var phoneList = new List<PhoneDto>();
            var phone = new PhoneDto{
                PhoneNumber = "9081723465",
                Type = "Home"
            };
            phoneList.Add(phone);
            var emailList = new List<EmailDto>();
            var email = new EmailDto{
                EmailAddress = "sabari@gmail.com",
                Type = "Home"
            };
            emailList.Add(email);
            return new UserDto{
                FirstName = "sabari",
                LastName = "palanivel",
                UserId = "vidya",
                Password = "OnePiece@123",
                Email = emailList,
                Address = addressList,
                Phone = phoneList
                
            };
        }

        public static UserModel NewUser(){
             Guid userId = Guid.NewGuid();
            var addressList = new List<AddressDto>();
            var address = new AddressDto{
                DoorNo = "M-5",
                StreetName = "Anna Nagar",
                City = "Chennai",
                State = "Tamil Nadu",
                Country = "India",
                ZipCode = "637289",
                Type = "Home"
            };
            addressList.Add(address);
            var phoneList = new List<PhoneDto>();
            var phone = new PhoneDto{
                PhoneNumber = "9081723465",
                Type = "Home"
            };
            var emailList = new List<EmailDto>();
            var email = new EmailDto{
                EmailAddress = "sabarithan@gmail.com",
                Type = "Home"
            };
            
            return new UserModel{
                Id = userId,
                FirstName = "sabari",
                LastName = "palanivel",
                UserId = "sabarithan",
                Password = "OnePiece@123"
                
            };
        }

        public static UserDto NewUserDto(){
             Guid userId = Guid.NewGuid();
            var addressList = new List<AddressDto>();
            var address = new AddressDto{
                DoorNo = "M-5",
                StreetName = "Anna Nagar",
                City = "Chennai",
                State = "Tamil Nadu",
                Country = "India",
                ZipCode = "637289",
                Type = "Home"
            };
            addressList.Add(address);
            var phoneList = new List<PhoneDto>();
            var phone = new PhoneDto{
                PhoneNumber = "9081723465",
                Type = "Home"
            };
            var emailList = new List<EmailDto>();
            var email = new EmailDto{
                EmailAddress = "sabari@gmail.com",
                Type = "Home"
            };
            emailList.Add(email);
            return new UserDto{
                FirstName = "sabari",
                LastName = "palanivel",
                UserId = "sabari",
                Password = "OnePiece@123",
                Email = emailList,
                Address = addressList,
                Phone = phoneList
                
            };
        }
        public static UserDto NewUserSameIdDto(){
             Guid userId = Guid.NewGuid();
            var addressList = new List<AddressDto>();
            var address = new AddressDto{
                DoorNo = "M-5",
                StreetName = "Anna Nagar",
                City = "Chennai",
                State = "Tamil Nadu",
                Country = "India",
                ZipCode = "637289",
                Type = "Home"
            };
            addressList.Add(address);
            var phoneList = new List<PhoneDto>();
            var phone = new PhoneDto{
                PhoneNumber = "9081723465",
                Type = "Home"
            };
            phoneList.Add(phone);
            var emailList = new List<EmailDto>();
            var email = new EmailDto{
                EmailAddress = "sabari123@gmail.com",
                Type = "Home"
            };
            emailList.Add(email);
            return new UserDto{
                FirstName = "sabari",
                LastName = "palanivel",
                UserId = "sabarithan",
                Password = "OnePiece@123",
                Email = emailList,
                Address = addressList,
                Phone = phoneList
                
            };
        }

        public static EmailModel Email(){
             var id = Guid.NewGuid();
            return new EmailModel{
                Id = Guid.NewGuid(),
                EmailAddress = "sabari@gmail.com",
                Type = id
            };
        }
    
        public static PhoneDto Phone(){
             
            return new PhoneDto{
                PhoneNumber = "9081723465",
                Type = "Home"
            };
        }

        public static AddressDto Address(){
             
            return new AddressDto{
                DoorNo = "M-5",
                StreetName = "Anna Nagar",
                City = "Chennai",
                State = "Tamil Nadu",
                Country = "India",
                ZipCode = "637289",
                Type = "Home"
            };
        }
    
        public static RefsetModel Refset(){
            return new RefsetModel{
                RefSetKey = "PHONE_TYPE",
                Description = "phone"
            };
        }
        public static RefsetModel Refset1(){
            return new RefsetModel{
                RefSetKey = "EMAIL_TYPE",
                Description = "Email"
            };
        }
        public static RefsetModel Refset2(){
            return new RefsetModel{
                RefSetKey = "ADDRESS_TYPE",
                Description = "Address"
            };
        }

        public static ReftermModel Refterm(){
            return new ReftermModel{
                RefTermKey = "Home",
                Description = "home"
            };
        }

        public static UserDto NewUserWithPasswordIncompatability(){
             Guid userId = Guid.NewGuid();
            var addressList = new List<AddressDto>();
            var address = new AddressDto{
                DoorNo = "M-5",
                StreetName = "Anna Nagar",
                City = "Chennai",
                State = "Tamil Nadu",
                Country = "India",
                ZipCode = "637289",
                Type = "Home"
            };
            addressList.Add(address);
            var phoneList = new List<PhoneDto>();
            var phone = new PhoneDto{
                PhoneNumber = "9081723465",
                Type = "Home"
            };
            var emailList = new List<EmailDto>();
            var email = new EmailDto{
                EmailAddress = "sabarithan@gmail.com",
                Type = "Home"
            };
            
            return new UserDto{
                FirstName = "sabari",
                LastName = "palanivel",
                UserId = "vidya",
                Password = "onepiece",
                Email = emailList,
                Address = addressList,
                Phone = phoneList
                
            };
        }
    }
}