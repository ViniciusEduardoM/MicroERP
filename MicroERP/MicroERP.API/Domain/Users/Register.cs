using MicroERP.API.Domain.Common.Utils;
using MicroERP.API.Infra.Data;
using MicroERP.ModelsDB.Models.MasterData.Users;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace MicroERP.API.Domain.Users
{
    public class Register
    {
        public required string CompanyDB { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }

        private readonly DataContext _context;

        Register() =>        
            _context = DbContextFactory.CreateWithCompany(CompanyDB);

        internal void VerifyRegisterAllreadyExists()
        {
            if (_context.User.Any(e => e.Name == UserName))
                throw new Exception($"User {UserName} allready exists");

            if (_context.User.Any(e => e.Email == Email))
                throw new Exception($"Email {Email} allready exists");
        }

        internal async Task RegisterNewUser()
        {
            await _context.User.AddAsync(new User
            {
                Name = UserName,
                Email = Email,
                Password = Auth.CreateHash(Password),
                RoleId = 1
            });

            await _context.SaveChangesAsync();
        }

    }
}
