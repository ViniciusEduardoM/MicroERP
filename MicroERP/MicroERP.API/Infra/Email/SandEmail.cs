using MicroERP.API.Infra.Data;
using MicroERP.API.Models.InternalDBTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net.Mail;

namespace MicroERP.API.Infra.Email
{
    public class SandEmail
    {
        private string? _email;
        private string? _password;

        private int _porta = 587;
        private string _host = "smtp.gmail.com";

        SmtpClient _smtp;

        private string _subject;
        private string _body;
        private IEnumerable<string> _addresses;

        public void SendEmailAsync()
            => _smtp.SendAsync(MakeEmailBody(), null);

        public SandEmail(string subject, string body, params string[] addresses)
        {
            _subject = subject;
            _body = body;
            _addresses = addresses;

            _email = Environment.GetEnvironmentVariable("EMAIL_EMPRESA");
            _password = Environment.GetEnvironmentVariable("SENHA_EMAIL_EMPRESA");

            if (string.IsNullOrEmpty(_password) || string.IsNullOrEmpty(_email))
                throw new Exception("Erro interno, credenciais de email não cadastradas");

            _smtp = new SmtpClient(_host, _porta);
            _smtp.EnableSsl = true;
            _smtp.UseDefaultCredentials = false;
            _smtp.Credentials = new System.Net.NetworkCredential(_email, _password);
        }

        private MailMessage MakeEmailBody()
        {
            MailMessage mail = new MailMessage();

            foreach (var address in _addresses)
                mail.To.Add(address);

            mail.From = new MailAddress(_email);
            mail.Subject = _subject;
            mail.Body = _body;
            mail.IsBodyHtml = true;

            return mail;
        }
    }
}
