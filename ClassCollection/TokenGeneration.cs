using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OP1_API.Areas.Login.Models;

namespace OP1_API.ClassCollection
{
    public class TokenGeneration
    {
        GlobalClass _cl;
        IConfiguration _configuration;
        public TokenGeneration(GlobalClass cl, IConfiguration confuguration) {
            _configuration = confuguration;
            _cl = cl;
        }

        public string GetToken(LoginResultModel _userData)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("result", _userData.result.ToString()),
                new Claim("retval", _userData.retval.ToString()),
                new Claim("msg", _userData.msg.ToString()),
                new Claim("userCode", _userData.userCode.ToString()),
                new Claim("userRole", _userData.userRole.ToString()),
                new Claim("finYear", _userData.finYear.ToString()),
                new Claim("userCompany", _userData.userCompany.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(1440),
                signingCredentials: singIn
                );
            string Token = new JwtSecurityTokenHandler().WriteToken(token);

            return Token;
        }


    }
}
