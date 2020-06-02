using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OSS.Business.DTOs;

namespace OSS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IConfiguration config;

        public LoginController(IConfiguration config)
        {
            this.config = config;
        }

        [HttpPost]
        public IActionResult Login([FromBody] AccountDto account)
        {
            if (account == null)
            {
                account = new AccountDto
                {
                    Email = "test1Email",
                    FullName = "test1FullName",
                    Password = "test1Password",
                    Username = "test1Username"
                };
            }

            AccountDto resultAccount = AuthenticateUser(account);

            if (resultAccount != null)
            {
                var token = GenerateJsonWebToken(resultAccount);

                return Ok(token);
            }

            return Unauthorized();
        }

        private object GenerateJsonWebToken(AccountDto account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecretKey"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, account.Username),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, account.Email),
                new Claim("fullname", account.FullName),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private AccountDto AuthenticateUser(AccountDto account)
        {
            var accounts = new AccountDto[]
            {
                new AccountDto
                {
                    Email = "test1Email",
                    FullName ="test1FullName",
                    Password = "test1Password",
                    Username = "test1Username"
                },
                new AccountDto
                {
                    Email = "test2Email",
                    FullName ="test2FullName",
                    Password = "test2Password",
                    Username = "test2Username"
                },
                new AccountDto
                {
                    Email = "test3Email",
                    FullName ="test3FullName",
                    Password = "test3Password",
                    Username = "test3Username"
                },
                new AccountDto
                {
                    Email = "test4Email",
                    FullName ="test4FullName",
                    Password = "test4Password",
                    Username = "test4Username"
                },
                new AccountDto
                {
                    Email = "test5Email",
                    FullName ="test5FullName",
                    Password = "test5Password",
                    Username = "test5Username"
                },
                new AccountDto
                {
                    Email = "test6Email",
                    FullName ="test6FullName",
                    Password = "test6Password",
                    Username = "test6Username"
                }
            };

            var foundAccount = accounts.FirstOrDefault(a => a.Username == account.Username && a.Password == account.Password);

            if (foundAccount != null)
            {
                return new AccountDto
                {
                    Email = foundAccount.Email,
                    FullName = foundAccount.FullName,
                    Username = foundAccount.Username
                };
            }

            return null;
        }
    }
}
