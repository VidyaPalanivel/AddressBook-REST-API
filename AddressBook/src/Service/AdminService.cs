using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using AddressBook.Contracts;
using AddressBook.Entities.DTOs;
using AddressBook.Entities.Models;
using Microsoft.Extensions.Configuration;
using System.Text;
using AddressBook.Logger.IManager;
using AddressBook.Util;

namespace AddressBook.Service
{
    public class AdminService : IAdminDetailsService
    {
        private readonly ILoggerManager _logger;
        private readonly AddressBookContext _context;
        
        public AdminService(ILoggerManager logger, AddressBookContext context)
        {
            _logger = logger;
            _context = context;
        }

        public TokenDto GenerateJSONWebToken(string userId, string firstName, string lastName)
        {
        _logger.LogDebug("Entering into GenerateJSONWebToken() in AdminService");
        var tokenHandler = new JwtSecurityTokenHandler();
        var keyValue = "some_big_key_value_here_secret";
		var tokenKey = Encoding.UTF8.GetBytes(keyValue);
		var tokenDescriptor = new SecurityTokenDescriptor
		{
		  Subject = new ClaimsIdentity(new Claim[]
		  {
            new Claim("user_id", userId),
            new Claim("first_name", firstName),
            new Claim("last_name", firstName),
		  }),
		   Expires = DateTime.Now.AddYears(1),
           Issuer = "AddressBook",
            Audience = "AddressBook",
		   SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),SecurityAlgorithms.HmacSha256Signature)
		};
		var token = tokenHandler.CreateToken(tokenDescriptor);
        _logger.LogDebug("Exiting from GenerateJSONWebToken() in AdminService");
		return new TokenDto { Token = tokenHandler.WriteToken(token),
        Type = "Bearer" };
        }
       
        public Tuple<int,TokenDto> Login(LoginDto loginDto)
        {
                _logger.LogDebug("Entering into Login method inside AdminService");
                var userDetails = _context.User.Where(x=> x.UserId == loginDto.UserId && x.IsActive == true).FirstOrDefault();
                if(userDetails == null)
                {
                    _logger.LogDebug("Exiting from Login method inside AdminService");
                    return Tuple.Create(2,(TokenDto)null);
                }
                var password = StringExtensions.DecodeFrom64(userDetails.Password);
                if(loginDto.Password == password)
                {
                    _logger.LogDebug("Exiting from Login method inside AdminService");
                    return Tuple.Create(1,GenerateJSONWebToken(userDetails.UserId,userDetails.FirstName,userDetails.LastName));
                }
                else
                {
                    _logger.LogDebug("Exiting from Login method inside AdminService");
                    return Tuple.Create(3,(TokenDto)null);
                }
        }

        public Tuple<bool,string> InsertMetaData(List<MetadataDto> metadataDto){
            try{
                _logger.LogDebug("Entering into InsertMetaData method inside AdminService");
                Tuple<bool,string> result = Tuple.Create(true,"");
                for(int i =0 ;i<metadataDto.Count; i++)
                {
                    result = InsertRefsetAndReftermDetails(metadataDto[i]);
                    if(!result.Item1)
                        return result;
                }
                
                _logger.LogDebug("Exiting from InsertMetaData method inside AdminService");
                return Tuple.Create(true, "");
            
            }
            catch{
                _logger.LogError("An error occurred in InsertMetaData in AdminService");
                return Tuple.Create(false,"Error occured while inserting meta data");
            }
           
        }

        public Tuple<bool,string> InsertRefsetAndReftermDetails(MetadataDto metadataDto){
            _logger.LogDebug("Entering into InsertRefsetAndReftermDetails method inside AdminService");
            if(metadataDto.Refset == null || metadataDto.Refterm.Count == 0)
                    return Tuple.Create(false, "Details are not sufficient");
                var refsetId = Guid.NewGuid();
                var listOfRefTerm = metadataDto.Refterm;
                _context.RefSet.Add(new RefsetModel{
                    Id = refsetId,
                    RefSetKey = metadataDto.Refset.RefSetKey,
                    Description = metadataDto.Refset.Description
                });
                var refTermKeyValues = _context.RefTerm.Where(x=>x.IsActive == true).Select(x => x.RefTermKey).ToList();
                for (int i=0;i<listOfRefTerm.Count; i++)
                {
                    var reftermId = Guid.NewGuid();
                    if(!(refTermKeyValues.Contains(listOfRefTerm[i].RefTermKey)))
                    {    
                        _context.RefTerm.Add(new ReftermModel{
                        Id = reftermId,
                        RefTermKey = listOfRefTerm[i].RefTermKey,
                        Description = listOfRefTerm[i].Description
                        });    
                    }
                    else{
                        reftermId = _context.RefTerm.Where(x => x.RefTermKey == listOfRefTerm[i].RefTermKey && x.IsActive == true).Select(x => x.Id).FirstOrDefault();
                    }
                    InsertSetRefTermData(refsetId, reftermId);
                }
                _context.SaveChanges();
                _logger.LogDebug("Exiting from InsertRefsetAndReftermDetails method inside AdminService");
                return Tuple.Create(true, "");
                
        }
    
        public void InsertSetRefTermData(Guid refsetId, Guid reftermId)
        {
            _logger.LogDebug("Entering into InsertSetRefTermData method inside AdminService");
            _context.SetRefTerm.Add(new SetreftermModel{
                Id = Guid.NewGuid(),
                RefSetId = refsetId,
                RefTermId = reftermId
            });
            _context.SaveChanges();
            _logger.LogDebug("Exiting from InsertSetRefTermData method inside AdminService");
        }
    
        public Tuple<bool,string> AddRefTermToTheExistingRefset(MetadataDto metadataDto){
            try{
            _logger.LogDebug("Entering into InsertRefTerm method inside AdminService");
            if(metadataDto.Refset == null || metadataDto.Refterm.Count == 0)
                return Tuple.Create(false, "Details are not sufficient");
            var refsetId = _context.RefSet.Where(x=> x.RefSetKey == metadataDto.Refset.RefSetKey && x.IsActive == true).Select(x=>x.Id).First();
            var listOfRefTerm = metadataDto.Refterm;
            var refTermKeyValues = _context.RefTerm.Where(x=>x.IsActive == true).Select(x => x.RefTermKey).ToList();
            for (int i=0;i<listOfRefTerm.Count; i++)
            {
                var reftermId = Guid.NewGuid();
                if(!(refTermKeyValues.Contains(listOfRefTerm[i].RefTermKey)))
                {    
                    _context.RefTerm.Add(new ReftermModel{
                    Id = reftermId,
                    RefTermKey = listOfRefTerm[i].RefTermKey,
                    Description = listOfRefTerm[i].Description
                    });    
                }
                else{
                    reftermId = _context.RefTerm.Where(x => x.RefTermKey == listOfRefTerm[i].RefTermKey && x.IsActive == true).Select(x => x.Id).FirstOrDefault();
                }
                InsertSetRefTermData(refsetId, reftermId);
            }
            _context.SaveChanges();
            _logger.LogDebug("Exiting from InsertRefTerm method inside AdminService");
            return Tuple.Create(true, "");
            }
            catch{
                _logger.LogError("An error occurred in InsertRefTerm in AdminService");
                return Tuple.Create(false,"Error occured while inserting meta data");
            }
        }
    }
}