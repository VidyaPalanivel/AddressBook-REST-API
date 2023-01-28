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
    public class TestAdminService{
        private readonly AddressBookContext _context;
        private Mock<ILoggerManager> _logger;
        public TestAdminService ()
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
        public void MetaData_Return200SuccessStatus()
        {
        var metaData = MockDataDetails.NewMetaData();
       
        var adminService = new AdminService(_logger.Object,_context);

        var result = (Tuple<bool, string>)adminService.InsertMetaData(metaData);

        Assert.True(result.Item1 == true);

        }
        [Fact]
        public void MetaData_Return409Error()
        {
           var metaData = MockDataDetails.NewMetaDataWithMissingData();
       
        var adminService = new AdminService(_logger.Object,_context);

        var result = (Tuple<bool, string>)adminService.InsertMetaData(metaData);

        Assert.True(result.Item1 == false);

        }


    }
}