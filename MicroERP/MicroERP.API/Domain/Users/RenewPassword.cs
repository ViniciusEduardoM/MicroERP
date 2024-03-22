using MicroERP.API.Domain.Common.Utils;
using MicroERP.API.Infra.Data;
using MicroERP.ModelsDB.Models.MasterData.Users;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace MicroERP.API.Domain.Users
{
    public class RenewPassword
    {
        public string CompanyDB { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public int CodeRecovery { get; set; }

        private readonly DataContext _context;

        User? _User;

        RenewPassword(IConfiguration configuration)
        {
            _context = DbContextFactory.CreateWithCompany(CompanyDB);
        }

        internal void VerifyUserAndCodeExpires()
        {
            _User = _context.User.FirstOrDefault(e => e.Name == UserName || e.Email == UserName);

            if (_User == null)
                throw new Exception("Usuário não existe");

            if (!_context.PasswordRecovery.Any(x => x.UserName == UserName && x.Expiration > DateTime.UtcNow))
                throw new Exception("Código de recuperação inexistente ou expirado");

            if (!_context.PasswordRecovery.Any(x => x.CodeRecovery == CodeRecovery && x.Expiration > DateTime.UtcNow))
                throw new Exception("Código informado está incorreto");
        }

        internal async Task UpdateNewPasswordInContext()
        {
            _User.Password = Auth.CreateHash(NewPassword);

            _context.Update(_User);

            await _context.SaveChangesAsync();
        }
    }
}
