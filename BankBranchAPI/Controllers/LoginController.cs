using BankBranchAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace BankBranchAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly BankContext _context;

     
        private readonly TokenService service;

        public LoginController( TokenService service, BankContext context)
        {
            this.service = service;
            _context = context;
        }
        [HttpPost]
        public IActionResult Login(UserRegistration userRegistration)
        {
            var response = this.service.GenerateToken(userRegistration.Username, userRegistration.Password);

            if (response.IsValid)
            {
                return Ok(new { Token = response.Token });
            }
            else
            {
                return BadRequest("Username and/or Password is wrong");
            }

        }
        [HttpPost("Register")]
        public IActionResult Register([FromBody] UserRegistration userRegistration)
        {
            bool isAdmin = false;
            if (_context.Users.Count() == 0)
            {
                isAdmin = true;
            }

            var newAccount = UserAccount.Create(userRegistration.Username, userRegistration.Password, isAdmin);

            _context.Users.Add(newAccount);
            _context.SaveChanges();

            return Ok(new { Message = "User Created" });
        }

    }
}
