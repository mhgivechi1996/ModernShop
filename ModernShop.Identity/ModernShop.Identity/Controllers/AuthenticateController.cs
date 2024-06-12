using Google.Api;
using Identity.API.Data;
using Identity.API.Entities;
using Identity.API.Infra;
using Identity.API.Models.Dto;
using Identity.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController:ControllerBase
    {
        private readonly IdentityDbContext identityDbContext;
        private readonly EncryptionUtility encryptionUtility;
        private readonly IPermissionStateRepository permissionStateRepository;
        //private readonly IDistributedCache _cache;


        public AuthenticateController(IdentityDbContext identityDbContext, EncryptionUtility encryptionUtility,
            IPermissionStateRepository permissionStateRepository)
        {
            this.identityDbContext = identityDbContext;
            this.encryptionUtility = encryptionUtility;
            this.permissionStateRepository = permissionStateRepository;
            //_cache = cache;
        }

        //[HttpGet("Test")]
        //public async Task<IActionResult> Test()
        //{
        //    var options = new DistributedCacheEntryOptions(); // create options object
        //    options.SetSlidingExpiration(TimeSpan.FromMinutes(1));
        //    options.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

        //    await _cache.SetStringAsync("ali", "123456", options);
        //    return Ok();
        //}

        //[HttpGet("Test2")]
        //public async Task<string> Test2()
        //{
        //    var value = await _cache.GetStringAsync("ali");
        //    return value;
        //}

        [HttpGet("{userName}")]
        public async Task<IActionResult> Ok([FromRoute] string userName)
        {
            var user = await identityDbContext.Users.SingleOrDefaultAsync(q => q.UserName == userName);
            var result = await permissionStateRepository.GetUserPermissionsState(user.Id);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(LoginDto login)
        {
            //1 => check username
            var user = await identityDbContext.Users.SingleOrDefaultAsync(q => q.UserName == login.UserName);

            if (user == null) return BadRequest("invalid user name");
            if (!user.IsActive) return BadRequest("user is not active !!!");

            var hashPassword = encryptionUtility.GenerateSHA256(login.Password, user.PasswordSalt);
            if (user.Password != hashPassword) return BadRequest("Invalid password");

            //2 => generate token
            var token = encryptionUtility.GenerateToken(user.Id);

            //3 => save user permission in redis state
            var userPermission = GetUserPermissions(user.Id); //from db
            await permissionStateRepository.SaveUserPermissionsState(user.Id, userPermission);

            var result = new AuthenticateDto
            {
                Token = token,
                UserName = user.UserName
            };

            return Ok(result);

        }

        private List<UserPermissionDto> GetUserPermissions(Guid userId)
        {
            var userPermissions = new List<UserPermissionDto>();
            userPermissions.Add(new UserPermissionDto
            {
                AppName = "Catalog",
                PermissioKeys = { "Category_Add", "Category_Delete" }, //, "Category_Get_All" },
            });

            userPermissions.Add(new UserPermissionDto
            {
                AppName = "FileServer",
                PermissioKeys = { "FileManager_Upload", "FileManager_Download" },
            });

            return userPermissions;

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var existsUserName = await identityDbContext.Users.AnyAsync(q => q.UserName == registerDto.UserName);
            if (existsUserName) return BadRequest("userName already exists!");

            var salt = Guid.NewGuid().ToString();
            var hashPassword = encryptionUtility.GenerateSHA256(registerDto.Password, salt);

            var user = new User
            {
                IsActive = true,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                RegisterDate = DateTime.Now,
                UserName = registerDto.UserName,
                Password = hashPassword,
                PasswordSalt = salt,
                Id = Guid.NewGuid(),
            };

            await identityDbContext.AddAsync(user);
            await identityDbContext.SaveChangesAsync();


            return Ok();
        }
    }
}
