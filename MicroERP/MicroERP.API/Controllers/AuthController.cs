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
using MicroERP.ModelsDB.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace MicroERP.API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private DataContext? _context = default;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(Login userLogin)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState.ValidationState);

                _context = DbContextFactory.CreateWithCompany(userLogin.CompanyDB);                
                
                var user = _context.User.FirstOrDefault(e => e.Name == userLogin.UserName || e.Email == userLogin.UserName);

                if (user == null)
                    return BadRequest("Usuário não existe");

                if (VerifyPasswordHash(userLogin.Password, user.Password))
                {
                    var token = CreateToken(user, userLogin.CompanyDB);
                    return Ok(new LoginResponse { Token = new JwtSecurityTokenHandler().WriteToken(token), ValidFrom = token.ValidFrom, ValidTo = token.ValidTo });
                }               

                else
                    return Unauthorized("A senha está incorreta");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, ex.InnerException?.Message, 400, "Um erro do servidor ocorreu");
            }
        }

        private JwtSecurityToken CreateToken(User user, string companyDB)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("CompanyDB", companyDB)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return token;
        }

        private bool VerifyPasswordHash(string password, string dbPassword)
        {
            return CreatePasswordHash(password) == dbPassword;
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
