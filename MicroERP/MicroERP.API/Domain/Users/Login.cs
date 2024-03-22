using MicroERP.API.Domain.Common.Utils;
using MicroERP.API.Infra.Data;
using MicroERP.ModelsDB.Models;
using MicroERP.ModelsDB.Models.MasterData.Users;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MicroERP.API.Domain.Users
{
    public class Login
    {
        public required string CompanyDB { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }

        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        User _User;

        Login(IConfiguration configuration)
        {
            _context = DbContextFactory.CreateWithCompany(CompanyDB);
            _configuration = configuration;
        }

        private bool VerifyUserLoginIsValid() => _context.User.Any(e => e.Name == UserName || e.Email == UserName);

        private User GetDTOUserLogin() => _context.User.First(e => e.Name == UserName || e.Email == UserName);

        internal void VerifyLogin()
        {
            if (!VerifyUserLoginIsValid())
                throw new Exception($"User {UserName} is invalid");

            _User = GetDTOUserLogin();

            if (!Auth.CompareHashes(Password, _User.Password))
                throw new Exception($"Password is wrong");            
        }

        internal LoginResponse MakeLoginAsync()
        {
            JwtSecurityToken authToken = CreateLoginToken(_User);

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(authToken),
                ValidFrom = authToken.ValidFrom,
                ValidTo = authToken.ValidTo
            };
        }

        private JwtSecurityToken CreateLoginToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, _context.Roles.First(r => r.Id == user.RoleId).Name),
                new Claim("CompanyDB", CompanyDB)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return token;
        }

    }
}
