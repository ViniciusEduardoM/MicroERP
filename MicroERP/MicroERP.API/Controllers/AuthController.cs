using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
using MicroERP.API.Models.InternalDBTables;
using MicroERP.API.Domain.Users;
using MicroERP.API.Domain.Common.Utils;
using MicroERP.API.Infra.Data;

namespace MicroERP.API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController()
        {
            
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(Login userLogin)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ValidationState);

            return Ok(Application.AuthApplication.Login(userLogin));
        }

        [HttpPost("Register")]
        public async Task<ActionResult<string>> Register(Register registerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ValidationState);

            await Application.AuthApplication.Register(registerRequest);

            return Created();
        }

        [HttpPost("RecoverPassword")]
        public async Task<ActionResult<string>> RecoverPassword(RecoverPassword recover)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ValidationState);

            await Application.AuthApplication.RecoverPassword(recover);

            return NoContent();
        }


        [HttpPost("RenewPassword")]
        public async Task<ActionResult<string>> RecoverPassword(RenewPassword renewPassword)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ValidationState);

            await Application.AuthApplication.RenewPassword(renewPassword);

            return Ok("Senha alterada com sucesso");

        }      
    }
}
