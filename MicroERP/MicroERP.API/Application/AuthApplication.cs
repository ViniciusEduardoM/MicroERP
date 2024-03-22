using MicroERP.API.Domain.Users;
using MicroERP.ModelsDB.Models;

namespace MicroERP.API.Application
{
    public class AuthApplication
    {
        public static LoginResponse Login(Login login)
        {
            login.VerifyLogin();

            return login.MakeLoginAsync();
        }

        public static async Task Register(Register register)
        {
            register.VerifyRegisterAllreadyExists();

            await register.RegisterNewUser();
        }

        public static async Task RecoverPassword(RecoverPassword recover)
        {
            recover.VerifyUserToRecoveryPassword();

            recover.MakeRenewEmailWithRecovryCode();

            await recover.RegisteryCodeRecoveryInContext();
        }

        public static async Task RenewPassword(RenewPassword renewPassword)
        {
            renewPassword.VerifyUserAndCodeExpires();
            await renewPassword.UpdateNewPasswordInContext();
        }
    }
}
