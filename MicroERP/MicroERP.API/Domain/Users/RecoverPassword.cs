using MicroERP.API.Infra.Data;
using MicroERP.API.Infra.Email;
using MicroERP.API.Models.InternalDBTables;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace MicroERP.API.Domain.Users
{
    public class RecoverPassword
    {
        public required string CompanyDB { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }

        private readonly DataContext _context;

        private int _codeRecover;

        RecoverPassword()
        {
            _context = DbContextFactory.CreateWithCompany(CompanyDB);

            var user = _context.User.FirstOrDefault(e => e.Name == UserName || e.Email == UserName);

            if (user == null)
                throw new Exception($"User {UserName} don't exists");
        }

        internal void VerifyUserToRecoveryPassword()
        {
            var user = _context.User.FirstOrDefault(e => e.Name == UserName || e.Email == UserName);

            if (user == null)
                throw new Exception("Usuário não existe");
        }

        internal SandEmail MakeRenewEmailWithRecovryCode()
        {
            Random rnd = new Random();

            _codeRecover = rnd.Next(100000, 999999);

            string subject = "MicroERP - Código de recuperação de senha";

            string body = "<h2 style=\"color: #2e6c80;\"><span style=\"color: #000000;\">Hello!</span></h2>\r\n" +
                          "<p>Your password recovery code in MicroERP is:</p>\r\n" +
                         $"<h1><strong>{_codeRecover}</strong></h1>\r\n" +
                          "<p>Enter this code into the code confirmation field.</p>\r\n" +
                          "<h1><strong>This code will expire in 30 minutes.</strong></h1>\r\n";

            return new SandEmail(subject, body, Email);
        }

        internal async Task RegisteryCodeRecoveryInContext()
        {
            _context.PasswordRecovery.Add(new PasswordRecory
            {
                UserName = UserName,
                Email = Email,
                CodeRecovery = _codeRecover,
                Expiration = DateTime.UtcNow.AddMinutes(30)
            });

            await _context.SaveChangesAsync();
        }
    }
}
