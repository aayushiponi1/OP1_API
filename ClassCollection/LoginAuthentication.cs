using System.Data.SqlClient;
using System.Data;
using OP1_API.Areas.Login.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OP1_API.ClassCollection
{
    public class LoginAuthentication
    {
        GlobalClass _cl;
        IConfiguration _configuration;
        public LoginAuthentication(GlobalClass cl, IConfiguration confuguration)
        {
            _configuration = confuguration;
            _cl = cl;
        }


        public LoginResultModel login(LoginParam p) {
            //TokenClass tc = new TokenClass();
            LoginResultModel r = new LoginResultModel();
            r.token = "";
            //HttpContext.Session.SetString("finYear", p.finYear);

            _cl.pwdEnc(p.username, p.password, p.finYear);
            string passWord = _cl.rj_encrypt(p.password);
            DataTable dt = new DataTable();
            dt = _cl.Rj_LoadTableWithProc("wms_userLogin_v3", new SqlParameter[]
            {
                new SqlParameter("@username", p.username),
                new SqlParameter("@password", passWord),
                new SqlParameter("@ipAddress", p.ipAddress)
            }, p.finYear);
            if (dt.Rows.Count > 0)
            {
                r.result = dt.Rows[0]["type"].ToString();
                r.retval = dt.Rows[0]["sno"].ToString();
                r.msg = dt.Rows[0]["msg"].ToString();
                r.userCode = dt.Rows[0]["sno"].ToString();
                r.userRole = dt.Rows[0]["opGroup"].ToString();
                r.finYear = p.finYear;
                r.userCompany = "0";
                if (r.result == "OK" || r.result== "LOGGEDIN")
                {
                    r.result = "OK";
                    r.token = GetToken(r);
                }
                else
                {
                    r.token = "";
                }

            }
            else
            {
                r.result = "FAILED";
                r.retval = "0";
                r.msg = "Incorrect Username and Password !";
                r.token = "";
                r.userCode = "0";
                r.userRole = "0";
                r.userCompany = "0";
            }
            return r;
        }

        public string GetToken(LoginResultModel _userData)
        {

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("userCode", _userData.userCode.ToString()),
                new Claim("retval", _userData.retval.ToString()),
                new Claim("userRole", _userData.userRole.ToString()),
                new Claim("result", _userData.result.ToString()),
                new Claim("finYear", _userData.finYear.ToString())
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
