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
using Microsoft.AspNetCore.Identity.Data;
using System.Net.Mail;
using NuGet.Protocol.Plugins;

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

        [HttpPost("Register")]
        public async Task<ActionResult<string>> Register(Register registerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ValidationState);

            _context = DbContextFactory.CreateWithCompany(registerRequest.CompanyDB);

            if (_context.User.Any(e => e.Name == registerRequest.UserName))
                return BadRequest($"O usuário {registerRequest.UserName} já existe");

            if (_context.User.Any(e => e.Email == registerRequest.Email))
                return BadRequest($"O email {registerRequest.Email} já existe");

            var user = await _context.User.AddAsync(new User
            {
                Name = registerRequest.UserName,
                Email = registerRequest.Email,
                Password = CreatePasswordHash(registerRequest.Password),
                RoleId = 1
            });
            await _context.SaveChangesAsync();

            return Ok($"Usuário {registerRequest.UserName} criado com sucesso!");
        }

        [HttpPost("RecoverPassword")]
        public async Task<ActionResult<string>> RecoverPassword(RecoverPassword recover)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ValidationState);

            _context = DbContextFactory.CreateWithCompany(recover.CompanyDB);

            var user = _context.User.FirstOrDefault(e => e.Name == recover.UserName || e.Email == recover.UserName);

            if (user == null)
                return BadRequest("Usuário não existe");

            string remetente = "E_MAIL_EMPRESA";
            // string destinatario = BancoDeVariaveis.Email_do_usuario; para testes vou enviar para mim mesmo
            string senha = "SENHA_EMAIL_EMPRESA";
            int porta = 587;
            string host = "smtp.gmail.com";

            Random rnd = new Random();

            int numCodigo = rnd.Next(100000, 999999);
            string codigo = numCodigo.ToString();

            string tema = "MicroERP - Código de recuperação de senha";
            string corpo = "" +
                "<h2 style=\"color: #2e6c80;\"><span style=\"color: #000000;\">Ol&aacute;!</span></h2>" +
                "<p>Seu c&oacute;digo recuperação de senha no MicroERP &eacute;:</p>" +
                "<h1><strong>" + codigo + "</strong></h1>" +
                "<p>Insira esse c&oacute;digo no campo de confirmação de código</p>" +
                "<h1><strong> Este código irá expirar em 30 minutos </strong></h1>";

            MailMessage mail = new MailMessage();
            mail.To.Add(recover.Email);
            mail.From = new MailAddress(remetente);
            mail.Subject = tema;
            mail.Body = corpo;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient(host, porta);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(remetente, senha);

            smtp.Send(mail);

            _context.PasswordRecovery.Add(new Models.InternalDBTables.PasswordRecory
            {
                UserName = recover.UserName,
                Email = recover.Email,
                CodeRecovery = numCodigo,
                Expiration = DateTime.UtcNow.AddMinutes(30)
            });
            await _context.SaveChangesAsync();

            return Ok(numCodigo);


        }

        [HttpPost("RenewPassword")]
        public async Task<ActionResult<string>> RecoverPassword(RenewPassword renewPassword)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ValidationState);

            _context = DbContextFactory.CreateWithCompany(renewPassword.CompanyDB);

            var user = _context.User.FirstOrDefault(e => e.Name == renewPassword.UserName || e.Email == renewPassword.UserName);

            if (user == null)
                return BadRequest("Usuário não existe");

            if (!_context.PasswordRecovery.Any(x => x.UserName == renewPassword.UserName && x.Expiration < DateTime.UtcNow))
                return BadRequest("Código de recuperação inexistente ou expirado");

            if (!_context.PasswordRecovery.Any(x => x.CodeRecovery != renewPassword.CodeRecovery && x.Expiration < DateTime.UtcNow))
                return BadRequest("Código informado está incorreto");

            user.Password = CreatePasswordHash(renewPassword.NewPassword);

            _context.Update(user);

            await _context.SaveChangesAsync();

            return Ok("Senha alterada com sucesso");

        }

        private JwtSecurityToken CreateToken(User user, string companyDB)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, _context.Roles.First(r => r.Id == user.RoleId).Name),
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
