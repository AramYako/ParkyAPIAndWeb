using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Models.Instances;
using ParkyApi.Models.Models;
using ParkyApi.Models.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ParkyApi.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository _userRepo;
        private readonly ITokenRepository _iTokenRepo;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepo, ITokenRepository tokenRepository, IMapper mapper)
        {
            this._userRepo = userRepo;
            this._iTokenRepo = tokenRepository;
            this._mapper = mapper;
        }


        [HttpPost("authenticate")]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] AuthenticationModel model)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, ModelState);
            
            var user = _userRepo.Authenticate(model.Username, model.Password);

            if (user is null)
            {
                ModelState.AddModelError("UserNotFound", "Username or password not found");
                return BadRequest(ModelState);
            }

            user.Token = this._iTokenRepo.GenerateToken(user);

            return Ok(user);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] AuthenticationModel model)
        {
            if (!ModelState.IsValid)
                return StatusCode(400, ModelState);

            if (!this._userRepo.IsUniqueUser(model.Username))
                return BadRequest("Username already exists");

            var user = this._userRepo.Register(model.Username, model.Password);
           
            if (user == null)
            {
                return BadRequest("Error while registering");

            }

            return Ok(user);
        }
    }
}
