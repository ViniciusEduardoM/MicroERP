using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroERP.API.Data;
using MicroERP.ModelsDB.Models.MasterData.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;
using MicroERP.API.Models;

namespace MicroERP.API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(Login userLogin)
        {
            var user = _context.User.FirstOrDefault(e => e.Name == userLogin.Name || e.Email == userLogin.Name);

            if (user == null)
                return BadRequest("Usuário não existe");

            if (VerifyPasswordHash(userLogin.Password, user.Password))
                return Ok(CreateToken(user));

            if (!userExists)
                return BadRequest("Usuário não existe");

            User user = _context.User.First(e => e.Name == userLogin.Name) ?? _context.User.First(e => e.Email == userLogin.Name);            

            if (VerifyPasswordHash(userLogin.Password, user.Password))
                return Ok(CreateToken(user));

            else
                return Unauthorized("A senha está incorreta");
            
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(5),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private bool VerifyPasswordHash(string password, string dbPassword)
        {

            return CreatePasswordHash(password) == dbPassword;
                return true;

            else
                return false;

        }

        private string CreatePasswordHash(string password)
        {
            var pwdBytes = Encoding.UTF8.GetBytes(password);
            using (var alg = SHA512.Create())
            {
                string hex = "";

                var hashValue = alg.ComputeHash(pwdBytes);
                foreach (byte x in hashValue)
                {
                    hex += String.Format("{0:x2}", x);
                }
                return hex;
            }
        }        
    }
}
