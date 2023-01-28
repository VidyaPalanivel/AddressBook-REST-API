using System;
using System.Collections.Generic;
using Xunit;
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
using Moq;
using AddressBook.Entities.DTOs;
using AddressBookUnitTest.MockData;
using System.Linq;
using AddressBook.Logger.IManager;
using NLog;
namespace AddressBookUnitTest.Service{
    public class TestUserService{
        private readonly AddressBookContext _context;
        private Mock<ILoggerManager> _logger;
        public TestUserService ()
        {
            var dependencySetupFixture = new DependencySetupFixture();
        _logger = new Mock<ILoggerManager>();
        var options = new DbContextOptionsBuilder<AddressBookContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

        _context = new AddressBookContext(options);

        _context.Database.EnsureCreated();
        }

        [Fact]
        public void CreateUser_ReturnsSuccess(){
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset());
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset1());
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset2());
            _context.RefTerm.AddRange(MockData.MockDataDetails.Refterm());
            var userService = new UserService(_logger.Object,_context);
            _context.User.AddRange(MockData.MockDataDetails.NewUser());
            _context.SaveChanges();
            var userDto = MockDataDetails.NewUserDto();
            var result = (Tuple<int, Guid>)userService.CreateUser(userDto);

            Assert.True(result.Item1 == 4);
        }
        
        [Fact]
        public void CreateUser_ReturnsPasswordIncompatability(){
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset());
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset1());
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset2());
            _context.RefTerm.AddRange(MockData.MockDataDetails.Refterm());
            var userService = new UserService(_logger.Object,_context);
            _context.User.AddRange(MockData.MockDataDetails.NewUser());
            _context.SaveChanges();
            var userDto = MockDataDetails.NewUserWithPasswordIncompatability();
            var result = (Tuple<int, Guid>)userService.CreateUser(userDto);

            Assert.True(result.Item1 == 5);
        }
        [Fact]
        public void CreateUser_ReturnEmailAlreadyExists(){
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset());
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset1());
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset2());
            _context.RefTerm.AddRange(MockData.MockDataDetails.Refterm());
            var userService = new UserService(_logger.Object,_context);
            _context.User.AddRange(MockData.MockDataDetails.NewUser());
            _context.Email.AddRange(MockData.MockDataDetails.Email());
            _context.SaveChanges();
            var userDto = MockDataDetails.NewUserWithSameEmailId();
            var result = (Tuple<int, Guid>)userService.CreateUser(userDto);
            Assert.True(result.Item1 == 3);
        }

        [Fact]
        public void CreateUser_ReturnMetadataNotExists(){ 
            var userService = new UserService(_logger.Object,_context);
            _context.User.AddRange(MockData.MockDataDetails.NewUser());
            _context.SaveChanges();
            var userDto = MockDataDetails.NewUserDto();
            var result = (Tuple<int, Guid>)userService.CreateUser(userDto);
            Assert.True(result.Item1 == 2);
        }

        [Fact]
        public void CreateUser_ReturnUserIdAlreadyExits(){
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset());
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset1());
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset2());
            _context.RefTerm.AddRange(MockData.MockDataDetails.Refterm());
            var userService = new UserService(_logger.Object,_context);
            _context.User.AddRange(MockData.MockDataDetails.NewUser());
            _context.SaveChanges();
            var userDto = MockDataDetails.NewUserSameIdDto();
            var result = (Tuple<int, Guid>)userService.CreateUser(userDto);
            Assert.True(result.Item1 == 1);
        }
        [Fact]
        public void UpdateUser_ReturnsSuccess(){
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset());
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset1());
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset2());
            _context.RefTerm.AddRange(MockData.MockDataDetails.Refterm());
            var userService = new UserService(_logger.Object,_context);
            _context.User.AddRange(MockData.MockDataDetails.NewUser());
            _context.SaveChanges();
            var userDto = MockDataDetails.NewUserDto();
            var insertedData = (Tuple<int, Guid>)userService.CreateUser(userDto);
            var result = (Tuple<bool, string>)userService.UpdateUser(insertedData.Item2,userDto);
            Assert.True(result.Item1 == true);
        }

        [Fact]
         public void UpdateUser_ReturnsEmailAlreadyExists(){
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset());
            _context.RefTerm.AddRange(MockData.MockDataDetails.Refterm());
            var userService = new UserService(_logger.Object,_context);
            _context.User.AddRange(MockData.MockDataDetails.NewUser());
            _context.Email.AddRange(MockData.MockDataDetails.Email());
            _context.SaveChanges();
            var userDto = MockDataDetails.NewUserDto();
            var insertedData = (Tuple<int, Guid>)userService.CreateUser(userDto);
            var userDetailsWithSameUserId = MockDataDetails.NewUserWithSameEmailId();
            var result = (Tuple<bool, string>)userService.UpdateUser(insertedData.Item2,userDetailsWithSameUserId);
            Assert.True(result.Item1 == false);
        }

        [Fact]
        public void UpdateUser_ReturnsUserNotFoundError(){
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset());
            _context.RefTerm.AddRange(MockData.MockDataDetails.Refterm());
            var userService = new UserService(_logger.Object,_context);
            var userDto = MockDataDetails.NewUserDto();
            var result = (Tuple<bool, string>)userService.UpdateUser(Guid.NewGuid(),userDto);
            Assert.True(result.Item1 == false);
        }
        [Fact]
        public void DeleteUser_ReturnSuccess(){
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset());
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset1());
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset2());
            _context.RefTerm.AddRange(MockData.MockDataDetails.Refterm());
            var userService = new UserService(_logger.Object,_context);
            _context.User.AddRange(MockData.MockDataDetails.NewUser());
            _context.SaveChanges();
            var userDto = MockDataDetails.NewUserDto();
            var insertedData = (Tuple<int, Guid>)userService.CreateUser(userDto);
            var result = (bool)userService.DeleteUser(insertedData.Item2);
            Assert.True(result == true);
        }

        [Fact]
        public void DeleteUser_ReturnNotFound(){
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset());
            _context.RefTerm.AddRange(MockData.MockDataDetails.Refterm());
            var userService = new UserService(_logger.Object,_context);
            _context.User.AddRange(MockData.MockDataDetails.NewUser());
            _context.SaveChanges();
            var result = (bool)userService.DeleteUser(Guid.NewGuid());
            Assert.True(result == false);
        }
    
        [Fact]
        public void GetUserDetails_RetturnSuccess(){
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset());
            _context.RefTerm.AddRange(MockData.MockDataDetails.Refterm());
            var userService = new UserService(_logger.Object,_context);
            _context.User.AddRange(MockData.MockDataDetails.NewUser());
            _context.SaveChanges();
            var userDto = MockDataDetails.NewUserDto();
            var result = (Tuple<bool, List<UserDto>>)userService.GetUserDetails();
            Assert.True(result.Item1 == true);

        }
    
        [Fact]
        public void GetSpecificUserDetails_ReturnSuccess(){
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset());
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset1());
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset2());
            _context.RefTerm.AddRange(MockData.MockDataDetails.Refterm());
            var userService = new UserService(_logger.Object,_context);
            _context.User.AddRange(MockData.MockDataDetails.NewUser());
            _context.SaveChanges();
            var userDto = MockDataDetails.NewUserDto();
            var insertedData = (Tuple<int, Guid>)userService.CreateUser(userDto);
            var result = (Tuple<bool,UserDto>)userService.GetUserDetails(insertedData.Item2);
            Assert.True(result.Item1 == true);

        }
    
        [Fact]
        public void GetUserCount_ReturnSucess(){
            _context.RefSet.AddRange(MockData.MockDataDetails.Refset());
            _context.RefTerm.AddRange(MockData.MockDataDetails.Refterm());
            var userService = new UserService(_logger.Object,_context);
            _context.User.AddRange(MockData.MockDataDetails.NewUser());
            _context.SaveChanges();
            var userDto = MockDataDetails.NewUserDto();
            var result = (int)userService.GetUserCount();
            Assert.True(result == 1);

        }
    

    }
}