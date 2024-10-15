using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Talabat.Apis.DTOS;
using Talabat.Apis.Errors;
using Talabat.Apis.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.Apis.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckExistingEmail(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "This Email Already Exists" } });  

            var user = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                DisplayName = model.DisplayName,
                PhoneNumber = model.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if(!result.Succeeded) return BadRequest(new ApiResponse(400));

            var userDTO = new UserDto()
            {
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                Token = await _tokenService.CreateTokenAsync(user, _userManager),
            };
            return Ok(userDTO);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null) return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            var userDTO = new UserDto()
            {
                Email = user.Email,
                UserName = user.UserName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager),
            };
            return Ok(userDTO);
        }

        //Get Current login user
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            return (new UserDto()
            {
                Email = user.Email,
                UserName = user.UserName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)

            });
        }
        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserWithAddress(User);
            
            var address = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto updatedAddress)
        {
            var address = _mapper.Map<AddressDto, Address>(updatedAddress);
            var user = await _userManager.FindUserWithAddress(User);

            address.Id = user.Address.Id;

            user.Address = address; 

            var result =  await _userManager.UpdateAsync(user);
            if(!result.Succeeded) return BadRequest(new ApiResponse(400));

            return Ok(updatedAddress);
        }

        [HttpGet("existemail")]
        public async Task<ActionResult<bool>> CheckExistingEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
